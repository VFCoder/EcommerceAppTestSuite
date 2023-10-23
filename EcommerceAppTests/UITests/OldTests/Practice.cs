using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTests.UITests.OldTests
{
    public class Practice
    {
        //public string firstName;
        List<string> userNames = new List<string>()
        {
            "John",
            "Mike",
            "Mary",
            "Jane",
            "May",
            "Vince",
            "Saif",
            "Chris",
            "Taslima"

        };

        [Test]
        public void LoopNames()
        {
            foreach (string name in userNames)
            {

                if (name.Substring(0, 2).ToLower() == "ma")
                {
                    Console.WriteLine(name);
                }
            }
        }



    }
}
