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
    public class Delete_DbTests
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
        public void DeleteRowTest()
        {
            using SqlConnection connection = new(_sqlConnection);

            connection.DeleteRow("TableName", "targetColumn", "targetValue");
        }
    }
}
