using EcommerceAppTestingFramework.Drivers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Pages
{
    public class OrderDetailsPage
    {
        private readonly IDriverActions _driver;

        public OrderDetailsPage(IDriverActions driver)
        {
            _driver = driver;
        }

        private IWebElement OrderNumberText => _driver.FindElementWait(By.CssSelector(".order-number strong"));

        public string pageTitle = "Order Details";

        public string GetOrderNumber()
        {
            string orderNumberText = OrderNumberText.Text[7..];
            return orderNumberText;
        }
    }
}
