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
    [TestFixture]
    [Category("DatabaseTests")]
    [Parallelizable]
    public class Read_DbTests
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
        public void ReadCustomerIdQuery()
        {
            using SqlConnection connection = new(_sqlConnection);

            string columnValue = "admin@vfcoder.com";
            string query = $"SELECT Id FROM Customer WHERE Email = '{columnValue}'";
            connection.PrintQueryResults(query);
        }

        [Test]
        public void VerifyDataInTable()
        {
            using SqlConnection connection = new(_sqlConnection);

            //Locate data to verify:

            string tableName = "Address";
            string columnName = "Company";
            string whereClause = "Id = 2";
            object expectedData = "VFCoder Company";
            bool convertData = false;

            //verify data:

            connection.VerifyCellData(tableName, columnName, whereClause, expectedData, convertData);
        }
    }
}
