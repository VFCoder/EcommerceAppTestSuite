using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Drivers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.BasePage;
using static EcommerceAppTestingFramework.Pages.CartPage;
using static EcommerceAppTestingFramework.Pages.CheckoutPage;
using static EcommerceAppTestingFramework.Pages.RegisterPage;
using static EcommerceAppTestingFramework.Pages.UserDataAndOrderVerifier;
using Microsoft.Data.SqlClient;
using EcommerceAppTestingFramework.Helpers;
using OpenQA.Selenium.Chrome;
using EcommerceAppTestingFramework.Models.DatabaseModels;

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    [Category("UI_DatabaseTests")]
    [Parallelizable]
    public class CustomerAppDatabaseTest
    {
        private TestConfiguration _testConfig;
        private IDriverActions _driver;
        private BasePage _basePage;
        private HomePage _homePage;
        private LoginPage _loginPage;
        private RegisterPage _registerPage;
        private ProductPage _productPage;
        private SearchPage _searchPage;
        private UserDataAndOrderVerifier _verifier;
        private CartPage _cartPage;
        private CheckoutPage _checkoutPage;
        private OrderDetailsPage _orderDetailsPage;
        private string _baseUrl;
        private string _adminUrl;
        private string _apiUrl;
        private string _sqlConnection;

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");

            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig, chromeOptions);

            _basePage = new BasePage(_driver);
            _homePage = new HomePage(_driver);
            _registerPage = new RegisterPage(_driver);
            _loginPage = new LoginPage(_driver);
            _productPage = new ProductPage(_driver);
            _searchPage = new SearchPage(_driver);
            _verifier = new UserDataAndOrderVerifier(_driver);
            _cartPage = new CartPage(_driver);
            _checkoutPage = new CheckoutPage(_driver);
            _orderDetailsPage = new OrderDetailsPage(_driver);
            _baseUrl = _testConfig.GetBaseUrl();
            _adminUrl = _testConfig.GetAdminUrl();
            _apiUrl = _testConfig.GetApiUrl();
            _sqlConnection = _testConfig.GetSqlConnection();

        }

        [TearDown]
        public void Teardown()
        {
            _driver.Dispose();
        }

        [Test]
        public void InsertProductReviewIntoProductReviewTable()
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {
                var data = new Dictionary<string, object>
                {
                    { "CustomerId", 2 },
                    { "ProductId", 1 },
                    { "StoreId", 1 },
                    { "IsApproved", 1 },
                    { "Title", "Great Computer" },
                    { "ReviewText", "Excellent computer, high quality, very fast and quiet." },
                    { "CustomerNotifiedOfReply", 1 },
                    { "Rating", 5 },
                    { "HelpfulYesTotal", 2 },
                    { "HelpfulNoTotal", 0 },
                    { "CreatedOnUtc", DateTime.UtcNow },
                };

                connection.InsertRecord("ProductReview", data);
            }
        }

        [Test]
        public void VerifyDataInTable()
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {
                string tableName = "Address";
                string columnName = "Company";
                string whereClause = "Id = 2";
                object expectedData = "VFCoder Company";
                bool convertData = false;

                connection.VerifyCellData(tableName, columnName, whereClause, expectedData, convertData);
            }
        }

        [Test]
        public void DeleteRowTest()
        {
            using SqlConnection connection = new SqlConnection(_sqlConnection);

            connection.DeleteRow("TableName", "targetColumn", "targetValue");
        }

        [Test]
        public void UpdateColumnCustomerAddress()
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {

                string tableName = "Address";
                string targetColumnName = "Address2";
                object newValue = "Apt. 6"; 
                string conditionColumnName = "Id"; 
                object conditionValue = 1; 

                connection.UpdateColumnValue(tableName, targetColumnName, newValue, conditionColumnName, conditionValue);

                //verify update:

                object expectedData = newValue;

                connection.VerifyCellData(tableName, targetColumnName, conditionColumnName, conditionValue, expectedData);
            }

        }

        [Test]
        public void UpdateColumnProductReviews()
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {

                string tableName = "Product";
                string targetColumnName = "ApprovedRatingSum";
                object newValue = 9;
                string conditionColumnName = "Id";
                object conditionValue = 1;

                connection.UpdateColumnValue(tableName, targetColumnName, newValue, conditionColumnName, conditionValue);

                //verify update:

                object expectedData = newValue;

                connection.VerifyCellData(tableName, targetColumnName, conditionColumnName, conditionValue, expectedData);
            }

        }

        [Test]
        public void GetTableDataFromQuery()
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {
                string query = "SELECT * FROM [Order] WHERE CustomerId = 1";
                connection.PrintQueryResults(query);
            }
        }
        
        
        [Test]
        public void GetCustomerIdQuery()
        {
            string columnValue = "admin@vfcoder.com";

            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {
                string query = $"SELECT Id FROM Customer WHERE Email = '{columnValue}'";
                connection.PrintQueryResults(query);
            }
        }
    }
}
