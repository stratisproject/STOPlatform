using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using Serilog;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Filters;
using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Core.Services;
using Stratis.STOPlatform.Core.Swagger;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.SmartContracts;
using Stratis.SmartContracts.CLR.Serialization;
using Stratis.SmartContracts.Networks;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Serializer = Stratis.SmartContracts.CLR.Serialization.Serializer;

namespace Stratis.STOPlatform
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            services.ConfigureExternalCookie(
                options =>
                {
                    options.Cookie.SecurePolicy = Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
                    options.Cookie.HttpOnly = true;
                });

            services.AddSingleton(c => Configuration.Get<AppSetting>());
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            services.AddHttpClient();
            services.AddScoped<ICoinInfoProvider, CoinInfoProvider>();

            services.AddRefitClient<ISwaggerApi>()
                    .ConfigureHttpClient((service, client) =>
                    {
                        var setting = service.GetRequiredService<AppSetting>();
                        client.BaseAddress = setting.SwaggerUrl;
                    });

            services.AddScoped<ISwaggerClient, SwaggerClient>();
            services.AddTransient<ISTOService, STOService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton(Log.Logger);
            services.AddSingleton(Environment.WebRootFileProvider);
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddScoped<ISerializer>(c => new Serializer(new ContractPrimitiveSerializer(new SmartContractsPoARegTest())));
            services.AddMvc(options =>
            {
                options.Filters.Add<WebApiRequestLogFilter>();
                options.Filters.Add<SetupRedirectionAttribute>();
                options.Filters.Add<VerifyRegistrationAttribute>();
            });

            services.AddHsts(options =>
            {
                options.Preload = false;
                options.IncludeSubDomains = false;
                options.MaxAge = TimeSpan.FromDays(30 * 7);// 7 months
            });


            services.AddControllersWithViews();

            services.AddAuthorization();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("cookie")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Events.OnTokenValidated += async ctx =>
                {
                    var userService = ctx.HttpContext.RequestServices.GetRequiredService<IUserService>();
                    var swaggerClient = ctx.HttpContext.RequestServices.GetRequiredService<ISwaggerClient>();

                    var setting = ctx.HttpContext.RequestServices.GetRequiredService<AppSetting>();
                    var identityAddress = ctx.Principal.FindFirst(JwtClaimTypes.Subject).Value;
                    var fullname = ctx.Principal.FindFirst(JwtClaimTypes.Name)?.Value;

                    var primaryAddress = await swaggerClient.GetPrimaryAddressAsync(setting.MapperContractAddress, identityAddress);

                    var user = await userService.UpsertAsync(identityAddress, primaryAddress, fullname);

                    var claims = new ClaimsIdentity();
                    if (user.Id == 1)
                    {
                        claims.AddClaim(new Claim(ClaimTypes.Role, AppConstants.AdminRole));
                    }
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                    if (fullname != null)
                        claims.AddClaim(new Claim(ClaimTypes.Name, user.FullName));

                    if (primaryAddress != null)
                        claims.AddClaim(new Claim("PrimaryAddress", primaryAddress));

                    ctx.Principal.AddIdentity(claims);
                };
                options.Authority = Configuration["Identity:Url"];
                options.ClientId = Configuration["Identity:ClientId"];
                options.SignInScheme = "cookie";
                options.SaveTokens = true;
                options.Scope.Add("profile");
                options.Scope.Add("kyc");
                options.GetClaimsFromUserInfoEndpoint = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ApplicationDbContext database)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var usCulture = new CultureInfo("en-Us");
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(usCulture.Name),
                // Formatting numbers, dates, etc.
                SupportedCultures = new[] { usCulture },
                // UI strings that we have localized.
                SupportedUICultures = new[] { usCulture }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=account}/{action=login}/{id?}");
            });

            app.UseCookiePolicy();

            database.Database.Migrate();
        }
    }
}
