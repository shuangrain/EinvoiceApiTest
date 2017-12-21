using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EinvoiceApiTest.Model
{
    public class PostModel
    {
        [Required]
        public string ApiKey { get; set; }

        [Required]
        [Url]
        public string ApiUrl { get; set; }

        [Required]
        public string ApiRequest { get; set; }

        public bool IsNeedSignature { get; set; }

        public bool IsTestMode { get; set; }

        public bool IsClientMode { get; set; }
    }
}
