using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Commons
{
    public class AppConfiguration
    {
        public string DatabaseConnection {  get; set; }
        public JWTSection JWTSection { get; set; }
        
    }
    public class JWTSection
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
    
}
