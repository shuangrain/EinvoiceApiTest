using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EinvoiceApiTest.Model
{
    public class ApiListModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string ApiName { get; set; }
        public string ApiUrl { get; set; }
        public Dictionary<string, string> Param { get; set; }
    }
}
