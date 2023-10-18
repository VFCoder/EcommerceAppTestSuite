/*using EcommerceAppTestingFramework.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Configuration.TestConfiguration;

namespace EcommerceAppTestingFramework.Drivers
{
    public class CrossBrowserDriver
    {
        protected IWebDriver driver;

        public void Setup(String browserName)
        {
            if (browserName == "Chrome")
            {
                driver = new ChromeDriver();
            }
            else if (browserName == "Firefox")
            {
                driver = new FirefoxDriver();
            }
            else if (browserName == "Edge")
            {
                driver = new EdgeDriver();
            }
            else
            {
                throw new ArgumentException("Invalid browserName");
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }



    }

}
*/

using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

public class BrowserDriver : IDisposable
{
    private IWebDriver driver;
    private readonly TestConfiguration _testConfig;

    public IWebDriver Driver => driver;

    public BrowserDriver(TestConfiguration testConfig, string browserType, ChromeOptions? chromeOptions = null)
    {
        _testConfig = testConfig;
        var testRunType = Enum.Parse<TestRunType>(testConfig.GetSetting("TestRunType"), ignoreCase: true);

        if (testRunType == TestRunType.Local)
        {
            if (chromeOptions != null)
            {
                driver = new ChromeDriver(chromeOptions);
            }
            else
            {
                GetDriver(browserType);
            }
        }
        else if (testRunType == TestRunType.Grid)
        {
            GetRemoteDriver(browserType);
        }
        else
        {
            throw new ArgumentException("Invalid TestRunType specified.");
        }
    }

    public static IWebDriver CreateDriver(string browserName)
    {
        return browserName switch
        {
            "Chrome" => new ChromeDriver(),
            "Firefox" => new FirefoxDriver(),
            "Edge" => new EdgeDriver(),
            _ => throw new ArgumentException("Invalid browserName"),
        };
    }

    public static IWebDriver CreateRemoteDriver(string browserName, string GridUrl)
    {
        return browserName switch
        {
            "Chrome" => new RemoteWebDriver(new Uri(GridUrl), new ChromeOptions()),
            "Firefox" => new RemoteWebDriver(new Uri(GridUrl), new FirefoxOptions()),
            "Edge" => new RemoteWebDriver(new Uri(GridUrl), new EdgeOptions()),
            _ => throw new ArgumentException("Invalid browserName"),
        };
    }

    public void GetDriver(string browserName)
    {
        driver = CreateDriver(browserName);
    }

    public void GetRemoteDriver(string browserName)
    {
        string GridUrl = _testConfig.GetSetting("GridUrl");
        driver = CreateRemoteDriver(browserName, GridUrl);
    }

    public void Dispose()
    {
        Teardown();
    }

    private void Teardown()
    {
        {
            driver.Quit();
        }
    }
}
