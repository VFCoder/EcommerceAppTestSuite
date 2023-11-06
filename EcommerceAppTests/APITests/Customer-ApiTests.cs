using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using static EcommerceAppTestingFramework.Pages.BasePage;
using EcommerceAppTestingFramework.Helpers;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json.Linq;
using RestSharp;
using EcommerceAppTestingFramework;
using EcommerceAppTestingFramework.Models.ApiModels;

namespace EcommerceAppTests.APITests
{
    [TestFixture]
    [Category("ApiTests")]
    [Parallelizable]
    public class ApiTests
    {
        private TestConfiguration _testConfig;
        private IDriverActions _driver;
        public RestClient _apiClient;
        public RestClient _apiAuth;
        public RestRequest _request;
        public RestResponse _response;
        private string _authUrl;
        private string _apiUrl;

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");

            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig, chromeOptions);
            _authUrl = _testConfig.GetAuthUrl();
            _apiUrl = _testConfig.GetApiUrl();
            _apiAuth = new RestClient(_authUrl);
            _apiClient = new RestClient(_apiUrl);
            _request = new RestRequest();
            _response = new RestResponse();

        }

        [TearDown]
        public void Teardown()
        {
            _driver.Dispose();
        }

        [Test]

        public void GetAPIAccessToken()
        {
            //enter json body with valid credentials and send post request to /token endpoint:

            string adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");


            var authRequestBody = new AuthenticationRequestBody
            {
                guest = false,
                username = "admin@vfcoder.com",
                password = adminPassword,
                remember_me = true 
            };
            _request = new RestRequest("/token", Method.Post);
            _request.AddJsonBody(authRequestBody);
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Post request at \"/token\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 201");

            //Add token to authorization header:

            GlobalObjects.AccessTokenResponseContent = _response.Content;
            JObject jobj = JObject.Parse(GlobalObjects.AccessTokenResponseContent);
            GlobalObjects.AccessToken = jobj.SelectToken("access_token").ToString();
            Console.WriteLine("Access token: " + GlobalObjects.AccessToken);

            _apiClient.AddDefaultHeader("Authorization", $"Bearer {GlobalObjects.AccessToken}");

            //verify authorization:

            bool isAuthorizationHeaderPresent = _apiClient.DefaultParameters.Any(p => p.Name == "Authorization");
            Assert.That(isAuthorizationHeaderPresent, Is.True, "Authorization header is not present.");
        }

        [Test]
        public void GetAllCustomers()
        {
            //Get authorization:

            GetAPIAccessToken();

            //Send get request to 'customers' endpoint:

            _request = new RestRequest("/api/customers", Method.Get);
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Get request at \"/api/customers\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //validate properties:

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);

            string propertyPath = "customers[1].billing_address.first_name";
            string expectedValue = "Steve";

            ApiExtensionMethods.ValidateJsonProperty(jsonObject, propertyPath, expectedValue);

            //view response body in console:

            ApiExtensionMethods.PrintApiResponse(GlobalObjects.ResponseContent);

        }

        [Test]
        public void GetCustomerById()
        {
            //Get authorization:

            GetAPIAccessToken();

            //Send get request to 'customers' endpoint with 'id' param:

            int id = 6;

            _request = new RestRequest($"/api/customers/{id}", Method.Get);
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Get request at \"/api/customers/{id}\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //validate properties:

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);

            string propertyPath = "customers[0].id";
            string expectedValue = id.ToString();

            ApiExtensionMethods.ValidateJsonProperty(jsonObject, propertyPath, expectedValue);

            //view response body:

            ApiExtensionMethods.PrintApiResponse(GlobalObjects.ResponseContent);

        }

        [Test]
        public void SearchForCustomer()
        {
            //Get authorization:

            GetAPIAccessToken();

            //enter 'Query' search parameter and send get request to 'customers/search' endpoint:

            string username = "Jake@email.com";

            _request = new RestRequest("/api/customers/search", Method.Get);
            _request.AddQueryParameter("Query", $"username: {username}");
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Get request at \"/api/customers/search\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //validate properties:

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);

            string userNamePropertyPath = "customers[0].username";
            string expectedUsername = username;

            ApiExtensionMethods.ValidateJsonProperty(jsonObject, userNamePropertyPath, expectedUsername);

            //view response body:

            ApiExtensionMethods.PrintApiResponse(GlobalObjects.ResponseContent);

        }

        [Test]
        public void CreateNewCustomer()
        {
            //Get authorization:

            GetAPIAccessToken();

            //enter json body with customer details and send post request to 'customers' endpoint:

            var createCustomerRequestBody = @"
            {
           
              ""customer"": {
                ""first_name"": ""Api"",
                ""last_name"": ""Guy"",
                ""username"": ""apiUser3@email.com"",
                ""password"": ""UserPassword123"",
                ""email"": ""apiUser3@email.com"",
                ""registered_in_store_id"": 1,
                ""role_ids"": [4],
              }
            }";

            _request = new RestRequest("/api/customers", Method.Post);
            _request.AddJsonBody(createCustomerRequestBody);
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Post request at \"/api/customers\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //validate properties:

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);

            string userNamePropertyPath = "customers[0].username";
            string expectedUsername = "apiUser3@email.com";

            ApiExtensionMethods.ValidateJsonProperty(jsonObject, userNamePropertyPath, expectedUsername);

            //view response body:

            ApiExtensionMethods.PrintApiResponse(GlobalObjects.ResponseContent);

        }


        [Test]
        public void ChangeCustomerDetails()
        {
            //Get authorization:

            GetAPIAccessToken();

            int id = GetCustomerId("first_name", "Valid");

            //enter json body with customer details and send put request to 'customers' endpoint:

            var createCustomerRequestBody = @"
            {
           
              ""customer"": {
/*                ""first_name"": ""Valid"",
                ""last_name"": ""Guy"",
                ""username"": ""apiUser3@email.com"",
                ""password"": ""UserPassword123"",
                ""email"": ""apiUser3@email.com"",
                ""registered_in_store_id"": 1,
                ""role_ids"": [4],*/
                ""password"": ""UserPassword123""
              }
            }";

            _request = new RestRequest($"/api/customers/{id}", Method.Put);
            _request.AddJsonBody(createCustomerRequestBody);
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Put request at \"/api/customers/id\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //validate properties:

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);

            string propertyPath = "customers[0].password";
            string expectedValue = "UserPassword123";

            ApiExtensionMethods.ValidateJsonProperty(jsonObject, propertyPath, expectedValue);

            //view response body:

            ApiExtensionMethods.PrintApiResponse(GlobalObjects.ResponseContent);

        }

        private int GetCustomerId(string property, string value)
        {

            //search for customer based on property/value:

            _request = new RestRequest("/api/customers/search", Method.Get);
            _request.AddQueryParameter("Query", $"{property}: {value}");
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Get request at \"/api/customers/search\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //Get customer ID from json response:

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);

            string propertyPath = $"customers[0].id";

            JToken token = jsonObject.SelectToken(propertyPath);

            if (token.Type == JTokenType.Integer)
            {
                Console.WriteLine($"Id {token.Value<int>()} found for customer with {property}: {value}");
                return token.Value<int>();
            }
            else
            {
                Console.WriteLine($"Property {propertyPath} was not in Json response");
                throw new Exception("Property not found in JSON response");
            }           
        }

        [Test]
        public void DeleteCustomerById()
        {
            //Create dummy customer:

            CreateNewCustomer();

            //Get new customer's Id:

            int id = GetCustomerId("first_name", "Api");

            //Send delete request to 'customers/{id}' endpoint with 'id' param:

            _request = new RestRequest($"/api/customers/{id}", Method.Delete);
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Delete request at \"/api/customers/{id}\" was not successful");

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //verify that customer's "deleted" property is set to true:

            _request = new RestRequest($"/api/customers/{id}", Method.Get);
            _response = _apiClient.Execute(_request);
            Assert.That(_response.IsSuccessful, Is.True, "Get request at \"/api/customers/{id}\" was not successful");

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);

            string propertyPath = "customers[0].deleted";
            string expectedValue = "True";

            ApiExtensionMethods.ValidateJsonProperty(jsonObject, propertyPath, expectedValue);

        }
    }
}
