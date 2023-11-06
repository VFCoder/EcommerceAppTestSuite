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
    public class Update_DbTests
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
        public void UpdateProductReviewColumnInProductTable()
        {
            using SqlConnection connection = new(_sqlConnection);

            //Locate target column to retrieve value:

            string tableName = "Product";
            string conditionColumnName = "Id";
            object conditionValue = 2;
            string ratingSumColumn = "ApprovedRatingSum";

            //Return previous ApprovedRatingSum int:

            int prevRatingSumInt = connection.GetSingleValue<int>(tableName, ratingSumColumn, conditionColumnName, conditionValue);

            //Calculate new rating sum:

            object newRatingSum = prevRatingSumInt + 5; //add new review rating

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

        [Test]
        public void UpdateColumnCustomerAddress()
        {
            using SqlConnection connection = new(_sqlConnection);

            //Locate cell to update and update value:

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
}
