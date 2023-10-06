using EcommerceAppTestingFramework.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Pages
{
    public class HomePage
    {
        private readonly IDriverActions _driver;

        public HomePage(IDriverActions driver)
        {
            _driver = driver;
        }

        private IWebElement LogoImage => _driver.FindElement(By.CssSelector("img[alt='Ecommerce Testing App']"));
        private IWebElement LoginLink => _driver.FindElement(By.CssSelector("a[href = '/login?returnUrl=%2F']"));
        private IWebElement SearchBox => _driver.FindElement(By.Id("small-search-box-form"));

        public string pageTitle = "Home page";


    }
}
