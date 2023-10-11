
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Models.ApiModels
{
    public class AuthenticationRequestBody
    {
        public bool guest { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool remember_me { get; set; }
    }

}
