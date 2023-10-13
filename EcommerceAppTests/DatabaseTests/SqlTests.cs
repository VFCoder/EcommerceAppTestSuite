using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using static EcommerceAppTestingFramework.Pages.BasePage;
using Microsoft.Data.SqlClient;
using EcommerceAppTestingFramework.Helpers;
using OpenQA.Selenium.Chrome;

namespace EcommerceAppTests.DatabaseTests
{
    [TestFixture]
    [Category("DatabaseTests")]
    [Parallelizable]
    public class CustomerAppDatabaseTest
    {
        private TestConfiguration _testConfig;
        private IDriverActions _driver;

        private string _sqlConnection;

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");

            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig, chromeOptions);

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

                UpdateProductReviewColumn();

            }
        }
        [Test]
        public void UpdateProductReviewColumn()
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {

                string tableName = "Product";
                string conditionColumnName = "Id";
                object conditionValue = 2;

                string ratingSumColumn = "ApprovedRatingSum";

                //Return ApprovedRatingSum int:
                int prevRatingSumInt = connection.GetSingleValue<int>(tableName, ratingSumColumn, conditionColumnName, conditionValue);

                //Calculate new rating sum:
                object newRatingSum = prevRatingSumInt + 5;

                //Update ApprovedRatingSum:
                connection.UpdateColumnValue(tableName, ratingSumColumn, newRatingSum, conditionColumnName, conditionValue);

                string totalReviewsColumn = "ApprovedTotalReviews";

                //Return ApprovedTotalReviews int:
                int prevTotalReviewsInt = connection.GetSingleValue<int>(tableName, totalReviewsColumn, conditionColumnName, conditionValue);

                //Calculate new total reviews:
                object newTotalReviews = prevTotalReviewsInt + 1;

                //Update ApprovedTotalReviews:
                connection.UpdateColumnValue(tableName, totalReviewsColumn, newTotalReviews, conditionColumnName, conditionValue);

                //verify update:

                connection.VerifyCellData(tableName, ratingSumColumn, conditionColumnName, conditionValue, newRatingSum);
                connection.VerifyCellData(tableName, totalReviewsColumn, conditionColumnName, conditionValue, newTotalReviews);
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
