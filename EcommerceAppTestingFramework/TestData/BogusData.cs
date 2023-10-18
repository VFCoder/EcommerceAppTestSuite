using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.TestData
{
    public class BogusData
    {

        #region Valid Data
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? DOBDay { get; set; }
        public string? DOBMonth { get; set; }
        public string? DOBYear { get; set; }
        public string? Company { get; set; }
        public string? Country { get; set; }
        public string? Street { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CardNumber { get; set; }
        public string? CVC { get; set; }
        public string? ExpMonth { get; set; }
        public string? ExpYear { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }

        #endregion

        #region Invalid Data
        public string? Letters1 { get; set; }
        public string? Letters8 { get; set; }
        public string? Letters30 { get; set; }
        public string? Numbers1 { get; set; }
        public string? Numbers8 { get; set; }
        public string? Numbers30 { get; set; }
        public string? SpecialChars { get; set; }
        public string? Emojis { get; set; }
        public string? Empty { get; set; }
        public string? Space { get; set; }
        public string? SpaceFrontLetters { get; set; }
        public string? SpaceMiddleLetters { get; set; }
        public string? SpaceEndLetters { get; set; }
        public string? SpaceFrontNumbers { get; set; }
        public string? SpaceMiddleNumbers { get; set; }
        public string? SpaceEndNumbers { get; set; }


        #endregion

    }
}
