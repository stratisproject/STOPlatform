using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Swagger
{
    public class SwaggerErrorResponse
    {
        public Error[] errors { get; set; }
    }

    public class Error
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }

}
