using EcommerceAppTestingFramework.Drivers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

public class WebDriverFactory
{
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

}
