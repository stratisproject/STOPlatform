using Microsoft.AspNetCore.Http;
using Stratis.STOPlatform.Data.Docs;
using System.ComponentModel.DataAnnotations;

namespace Stratis.STOPlatform.Models.SettingViewModels
{
    public class ContentViewModel
    {
        [Display(Name = "Slogan")]
        public string Slogan { get; set; }

        [Required]
        [Display(Name = "Page Cover")]
        public string PageCover { get; set; }

        [Display(Name = "Logo Image")]
        public IFormFile LogoImage { get; set; }
        public string LogoSrc { get; set; }

        [Display(Name = "Background Image")]
        public IFormFile BackgroundImage { get; set; }

        public string BackgroundSrc { get; set; }

        [Display(Name = "Login Background Image")]
        public IFormFile LoginBackgroundImage { get; set; }

        public string LoginBackgroundSrc { get; set; }

        [Display(Name = "Custom CSS Style")]
        public string CssStyle { get; set; }

        public ContentViewModel()
        {

        }

        public ContentViewModel(STOSetting setting)
        {
            PageCover = setting.PageCover;
            Slogan = setting.Slogan;
            LogoSrc = setting.LogoSrc;
            BackgroundSrc = setting.BackgroundSrc;
            LoginBackgroundSrc = setting.LoginBackgroundSrc;
        }

        public void UpdateEntity(STOSetting setting)
        {
            setting.PageCover = PageCover;
            setting.Slogan = Slogan;
        }
    }
}
