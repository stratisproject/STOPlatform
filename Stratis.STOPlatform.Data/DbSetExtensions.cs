using Microsoft.EntityFrameworkCore;
using Stratis.STOPlatform.Data.Docs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Data
{
    public static class DbSetExtensions
    {
        public async static Task<T> GetAsync<T>(this DbSet<Document> documents)
        {
            var key = typeof(T).Name;
            var doc = await documents.FirstAsync(d => d.Key == key);

            return doc.As<T>();
        }
    }
}
