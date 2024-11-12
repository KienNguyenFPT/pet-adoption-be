using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Commons
{
    public class PaypalSettings
    {
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public string ApiUrl { get; set; }
    }
}
