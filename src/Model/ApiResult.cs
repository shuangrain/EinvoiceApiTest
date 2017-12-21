using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EinvoiceApiTest.Model
{
    public class ApiResult
    {
        public int RtnCode { get; set; }

        public object RtnData { get; set; }
    }
}
