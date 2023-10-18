using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.TestData
{
    public class DataGenerator
    {
        Faker<BogusData> bogusData;
        private static Faker<string> randomCountry;

        public DataGenerator()
        {

            // Randomizer.Seed = new Random(123); 

            bogusData = new Faker<BogusData>()
                .RuleFor(f => f.FirstName, r => r.Name.FirstName())
                .RuleFor(f => f.LastName, r => r.Name.LastName())
                .RuleFor(f => f.Email, (r, f) => r.Internet.Email(f.FirstName, f.LastName))
                .RuleFor(f => f.Password, r => r.Internet.Password())
                .RuleFor(f => f.DOBDay, r => r.Random.Int(1, 31).ToString())
                .RuleFor(f => f.DOBMonth, r => r.Date.Month())
                .RuleFor(f => f.DOBYear, r => r.Random.Int(1960, 2005).ToString())
                .RuleFor(f => f.Company, r => r.Company.CompanyName())
                .RuleFor(f => f.Street, r => r.Address.StreetAddress())
                .RuleFor(f => f.Country, r => GetRandomCountry())
                .RuleFor(f => f.City, r => r.Address.City())
                .RuleFor(f => f.State, r => r.Address.StateAbbr())
                .RuleFor(f => f.Zip, r => r.Address.ZipCode())
                .RuleFor(f => f.PhoneNumber, r => r.Phone.PhoneNumber())
                .RuleFor(f => f.Subject, r => r.Random.Word())
                .RuleFor(f => f.Message, r => r.Rant.Review())
                .RuleFor(f => f.CardNumber, r => r.Finance.CreditCardNumber())
                .RuleFor(f => f.CVC, r => r.Finance.CreditCardCvv())
                .RuleFor(f => f.ExpMonth, r => r.Random.Int(1, 12).ToString())
                .RuleFor(f => f.ExpYear, r => r.Random.Int(2020, 2030).ToString())

                .RuleFor(f => f.Letters1, r => r.Lorem.Letter(1))
                .RuleFor(f => f.Letters8, r => r.Lorem.Letter(8))
                .RuleFor(f => f.Letters30, r => r.Lorem.Letter(30))
                .RuleFor(f => f.Numbers1, r => r.Random.Int(0, 9).ToString())
                .RuleFor(f => f.Numbers8, r => r.Random.Int(10000000, 99999999).ToString())
                .RuleFor(f => f.Numbers30, r => r.Random.String2(30, "0123456789"))
                .RuleFor(f => f.SpecialChars, r => r.Random.String2(8, "!@#$%^&*()_+{}|:<>?~[];',./"))
                .RuleFor(f => f.Empty, string.Empty)
                .RuleFor(f => f.Space, r => " ")
                .RuleFor(f => f.SpaceFrontLetters, r => " " + r.Lorem.Letter(6))
                .RuleFor(f => f.SpaceMiddleLetters, r => r.Lorem.Letter(3) + " " + r.Lorem.Letter(3))
                .RuleFor(f => f.SpaceEndLetters, r => r.Lorem.Letter(6) + " ")
                .RuleFor(f => f.SpaceFrontNumbers, r => " " + r.Random.Int(100000, 999999).ToString())
                .RuleFor(f => f.SpaceMiddleNumbers, r => r.Random.Int(100, 999).ToString() + " " + r.Random.Int(100, 999).ToString())
                .RuleFor(f => f.SpaceEndNumbers, r => r.Lorem.Letter(6) + " ");

            randomCountry = new Faker<string>()
                .CustomInstantiator(f => f.Random
                .ArrayElement(new string[]
                {"United States", "India", "Canada", "Australia", "Israel", "New Zealand", "Singapore"}));

        }

        public BogusData GenerateData()
        {
            return bogusData.Generate();
        }

        private static string GetRandomCountry()
        {
            return randomCountry.Generate();
        }

    }
}
