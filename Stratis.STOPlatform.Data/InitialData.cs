using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stratis.STOPlatform.Data
{
    public class InitialData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            InsertSTOSetting(modelBuilder);
            InsertSetupSettings(modelBuilder);
        }


        #region Document inserts
        private static void InsertSTOSetting(ModelBuilder modelBuilder)
        {
            var data = new STOSetting
            {
                Name = "Your STO",
                Symbol = "TOKEN",
                PageCover = @"Lorem ipsum dolor sit amet, pro an agam audire euismod, pro et tritani persequeris. Graece accumsan et eos. Harum doming inermis ut vis, sea eu adipiscing complectitur.

Eos ad legimus inimicus, dico purto cu qui, et percipit torquatos interpretaris mea. Ex solum consequat percipitur vim, quas melius delicatissimi mel ei.",
                WebsiteUrl = "https://mystowebsite.com",
                BlogPostUrl = "https://stratisplatform.com/blog/",
                TermsAndConditionsUrl = "https://stratisplatform.com/terms-of-use",
                BackgroundSrc = "/images/default-bg.png",
                LoginBackgroundSrc = "/images/default-bg.png",
                LogoSrc = "/images/default-logo.png"
            };

            modelBuilder.Entity<Document>().HasData(new Document(data) { Id = 1 });
        }

        private static void InsertSetupSettings(ModelBuilder modelBuilder)
        {
            var data = new SetupSetting { CurrentStep = 1 };

            modelBuilder.Entity<Document>().HasData(new Document(data) { Id = 2 });
        }

        #endregion
    }
}
