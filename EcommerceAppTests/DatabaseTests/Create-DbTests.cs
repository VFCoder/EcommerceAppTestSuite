using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using EcommerceAppTestingFramework.Helpers;
using Microsoft.Data.SqlClient;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTests.DatabaseTests
{
    public class Create_DbTests
    {

        private TestConfiguration _testConfig;
        private IDriverActions _driver;

        private string _sqlConnection;
        private Update_DbTests _updateDbTests;

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");

            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig, chromeOptions);
            _sqlConnection = _testConfig.GetSqlConnection();

            _updateDbTests = new Update_DbTests();
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Dispose();
        }

        [Test]
        public void InsertReviewIntoProductReviewTable()
        {
            using SqlConnection connection = new(_sqlConnection);

            //Insert required fields into table:

            string tableName = "ProductReview";
            string reviewTitle = "Best Computer Ever";
            string reviewText = "I've never owned such a powerful machine.";

            var data = new Dictionary<string, object>
                {

                    { "CustomerId", 7 },
                    { "ProductId", 2 },
                    { "StoreId", 1 },
                    { "IsApproved", 1 },
                    { "Title", reviewTitle},
                    { "ReviewText", reviewText },
                    { "CustomerNotifiedOfReply", 1 },
                    { "Rating", 5 },
                    { "HelpfulYesTotal", 1 },
                    { "HelpfulNoTotal", 0 },
                    { "CreatedOnUtc", DateTime.UtcNow },
                };

            connection.InsertRecord(tableName, data);

            //return review id:

            int reviewId = connection.GetSingleValue<int>(tableName, "Id", "Title", reviewTitle);

            //verify insert:

            connection.VerifyCellData(tableName, "Title", "Id", reviewId, reviewTitle);
            connection.VerifyCellData(tableName, "ReviewText", "Id", reviewId, reviewText);

            //Update product table to display review on app:

            _updateDbTests.UpdateProductReviewColumnInProductTable();
        }

    }
}
