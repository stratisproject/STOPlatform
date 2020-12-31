﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Stratis.STOPlatform.Data;

namespace Stratis.STOPlatform.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Stratis.STOPlatform.Data.Docs.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasFilter("[Key] IS NOT NULL");

                    b.ToTable("Documents");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Json = "{\"name\":\"Your STO\",\"symbol\":\"TOKEN\",\"totalSupply\":0,\"showTotalContribution\":false,\"pageCover\":\"Lorem ipsum dolor sit amet, pro an agam audire euismod, pro et tritani persequeris. Graece accumsan et eos. Harum doming inermis ut vis, sea eu adipiscing complectitur.\\r\\n\\r\\nEos ad legimus inimicus, dico purto cu qui, et percipit torquatos interpretaris mea. Ex solum consequat percipitur vim, quas melius delicatissimi mel ei.\",\"websiteUrl\":\"https://mystowebsite.com\",\"blogPostUrl\":\"https://stratisplatform.com/blog/\",\"termsAndConditionsUrl\":\"https://stratisplatform.com/terms-of-use\",\"logoSrc\":\"/images/default-logo.png\",\"backgroundSrc\":\"/images/default-bg.png\",\"loginBackgroundSrc\":\"/images/default-bg.png\"}",
                            Key = "STOSetting"
                        },
                        new
                        {
                            Id = 2,
                            Json = "{\"done\":false,\"currentStep\":1}",
                            Key = "SetupSetting"
                        });
                });

            modelBuilder.Entity("Stratis.STOPlatform.Data.Entities.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("EarnedToken")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("Invested")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("Refunded")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TransactionId", "UserId")
                        .IsUnique();

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("Stratis.STOPlatform.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastCheck")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<string>("WalletAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Address")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Stratis.STOPlatform.Data.Entities.Deposit", b =>
                {
                    b.HasOne("Stratis.STOPlatform.Data.Entities.User", "User")
                        .WithMany("Deposits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}