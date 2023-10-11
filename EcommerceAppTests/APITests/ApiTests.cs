using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using static EcommerceAppTestingFramework.Pages.BasePage;
using EcommerceAppTestingFramework.Helpers;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json.Linq;
using RestSharp;
using EcommerceAppTestingFramework;
using EcommerceAppTestingFramework.Models.ApiModels;

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    [Category("UI_ApiTests")]
    [Parallelizable]
    public class CustomerAppApiTest
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

            var authRequestBody = new AuthenticationRequestBody
            {
                guest = false,
                username = "admin@vfcoder.com",
                password = "adminvfcoder",
                remember_me = true 
            };
            _request = new RestRequest("/token", Method.Post);
            _request.AddJsonBody(authRequestBody);
            _response = _apiClient.Execute(_request);
            Assert.True(_response.IsSuccessful);

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
            Assert.IsTrue(isAuthorizationHeaderPresent, "Authorization header is not present.");
        }

        [Test]
        public void GetAllCustomers()
        {
            //Get authorization:

            GetAPIAccessToken();

            //Send get request to 'customers' endpoing:

            _request = new RestRequest("/api/customers", Method.Get);
            _response = _apiClient.Execute(_request);
            Assert.True(_response.IsSuccessful);

            //verify status code successful:

            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(200), $"Status code is not 200");

            //validate properties:

            GlobalObjects.ResponseContent = _response.Content;
            JObject jsonObject = JObject.Parse(GlobalObjects.ResponseContent);
            string propertyPath = "customers[1].billing_address.first_name";
            string expectedValue = "Steve";

            ApiExtensionMethods.ValidateJsonProperty(jsonObject, propertyPath, expectedValue);

            //view response body:

            ApiExtensionMethods.PrintApiResponse(GlobalObjects.ResponseContent);

        }
    }
}
