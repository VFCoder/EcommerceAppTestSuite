using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Helpers
{
    public static class ApiExtensionMethods
    {
        public static void PrintApiResponse(string jsonResponse)
        {
            JToken jtoken = JToken.Parse(jsonResponse);

            if (jtoken is JArray)
            {
                JArray jarray = (JArray)jtoken;

                foreach (var item in jarray)
                {
                    JObject jobj = (JObject)item;
                    Console.WriteLine("API Response Object: " + jobj);
                }
            }
            else if (jtoken is JObject)
            {
                JObject jobj = (JObject)jtoken;
                Console.WriteLine("API Response Object: " + jobj);
            }
            else
            {
                Console.WriteLine("Unexpected JSON response format: " + jtoken.GetType().Name);
            }
        }

        public static void ValidateJsonProperty(JObject jsonObject, string propertyPath, string expectedValue)
        {
            JToken token = jsonObject.SelectToken(propertyPath);

            if (token != null)
            {
                string actualValue = token.ToString();
                if (actualValue == expectedValue)
                {
                    Console.WriteLine($"Validation for '{propertyPath}' succeeded. Expected: {expectedValue}, Actual: {actualValue}");
                }
                else
                {
                    Console.WriteLine($"Validation for '{propertyPath}' failed. Expected: {expectedValue}, Actual: {actualValue}");
                }
            }
            else
            {
                Console.WriteLine($"Property '{propertyPath}' not found in JSON response.");
            }
        }

    }
}
