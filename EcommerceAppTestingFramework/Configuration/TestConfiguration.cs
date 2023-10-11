using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Configuration
{
    public class TestConfiguration
    {
        private readonly IConfiguration _configuration;

        public TestConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Local";

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public string GetSetting(string key)
        {
            return _configuration[key];
        }

        public string GetBaseUrl() => _configuration["BaseUrl"];
        public string GetAdminUrl() => _configuration["AdminUrl"];
        public string GetApiUrl() => _configuration["ApiUrl"];
        public string GetAuthUrl() => _configuration["AuthUrl"];
        public string GetSqlConnection() => _configuration["SqlConnection"];

    }

}
