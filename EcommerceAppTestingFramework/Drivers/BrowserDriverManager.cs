using EcommerceAppTestingFramework.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

public class BrowserDriverManager : IDisposable
{
    private IWebDriver driver;
    private readonly TestConfiguration _testConfig;

    public IWebDriver Driver => driver;

    public BrowserDriverManager(TestConfiguration testConfig, BrowserType browserType, ChromeOptions? chromeOptions = null)
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
                driver = CreateDriver(browserType); // Use the WebDriverManager to create the driver
            }
        }
        else if (testRunType == TestRunType.Grid)
        {
            driver = CreateRemoteDriver(testConfig, browserType);
        }
        else
        {
            throw new ArgumentException("Invalid TestRunType specified.");
        }
    }

    public static IWebDriver CreateDriver(BrowserType browserType)
    {
        switch (browserType)
        {
            case BrowserType.Chrome:
                new DriverManager().SetUpDriver(new ChromeConfig());
                return new ChromeDriver();
            case BrowserType.Firefox:
                new DriverManager().SetUpDriver(new FirefoxConfig());
                return new FirefoxDriver();
            case BrowserType.Edge:
                new DriverManager().SetUpDriver(new EdgeConfig());
                return new EdgeDriver();
            default:
                throw new WebDriverException("Unsupported browser type");
        }
    }

    public static IWebDriver CreateRemoteDriver(TestConfiguration testConfig, BrowserType browserType)
    {
        string GridUrl = testConfig.GetSetting("GridUrl"); 
        return browserType switch
        {
            BrowserType.Chrome => new RemoteWebDriver(new Uri(GridUrl), new ChromeOptions()),
            BrowserType.Firefox => new RemoteWebDriver(new Uri(GridUrl), new FirefoxOptions()),
            BrowserType.Edge => new RemoteWebDriver(new Uri(GridUrl), new EdgeOptions()),
            _ => throw new ArgumentException("Invalid browserName"),
        };
    }

    public void Dispose()
    {
        Teardown();
    }

    private void Teardown()
    {
        driver.Quit();
    }
}
