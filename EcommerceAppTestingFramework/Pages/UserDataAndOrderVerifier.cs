using EcommerceAppTestingFramework.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Pages
{
    public class UserDataAndOrderVerifier
    {

        private readonly IDriverActions _driver;
        private bool IsCartPageContext { get; }

        public UserDataAndOrderVerifier(IDriverActions driver, bool isCartPageContext = false)
        {
            _driver = driver;
            IsCartPageContext = isCartPageContext;
        }

        private IReadOnlyCollection<IWebElement> ProductRows => _driver.FindElements(By.CssSelector(".table-wrapper tbody tr"));
        private IReadOnlyCollection<IWebElement> ProductSKU => _driver.FindElements(By.CssSelector("td.sku .sku-number"));
        private IReadOnlyCollection<IWebElement> ProductImage => _driver.FindElements(By.CssSelector("td.product-picture img"));
        private IReadOnlyCollection<IWebElement> ProductName => _driver.FindElements(By.CssSelector("td.product a"));
        private IReadOnlyCollection<IWebElement> UnitPrice => _driver.FindElements(By.CssSelector("td.unit-price .product-unit-price"));
        private IReadOnlyCollection<IWebElement> QuantityCart => _driver.FindElements(By.CssSelector("td.quantity .qty-input"));
        private IReadOnlyCollection<IWebElement> QuantityStatic => _driver.FindElements(By.CssSelector("td.quantity .product-quantity"));
        private IReadOnlyCollection<IWebElement> TotalPrice => _driver.FindElements(By.CssSelector(".product-subtotal"));
        private IWebElement PriceBoxSubtotal => _driver.FindElementWait(By.CssSelector("tr:nth-of-type(1) .cart-total-right span"));
        private IWebElement PriceBoxShipping => _driver.FindElementWait(By.CssSelector("tr:nth-of-type(2) .cart-total-right span"));
        private IWebElement PriceBoxTax => _driver.FindElementWait(By.CssSelector("tr:nth-of-type(3) .cart-total-right span"));
        private IWebElement PriceBoxTotal => _driver.FindElementWait(By.CssSelector("tr:nth-of-type(4) .cart-total-right span"));
        private IWebElement PriceBoxPoints => _driver.FindElementWait(By.CssSelector("tr:nth-of-type(5) .cart-total-right span"), 1);


        public string GetTotalPrice()
        {
            _driver.WaitForLoad();
            return PriceBoxTotal.Text;
        }

        public int GetProductCount()
        {
            _driver.WaitForLoad();
            return ProductRows.Count;
        }

        public List<ProductTable> GetProductTableItems()
        {
            bool isCartPage = IsCartPageContext;

            List<ProductTable> productTableItems = new List<ProductTable>();

            IReadOnlyCollection<IWebElement> productTableRows = ProductRows;

            for (int i = 0; i < productTableRows.Count; i++)
            {
                string productSKU = ProductSKU.ElementAt(i).Text;
                string? productImage = null;

                try
                {
                    productImage = ProductImage.ElementAt(i).GetAttribute("src");
                }
                catch (ArgumentOutOfRangeException) { }

                string productName = ProductName.ElementAt(i).Text;
                string unitPrice = UnitPrice.ElementAt(i).Text;
                string quantity = isCartPage
                    ? QuantityCart.ElementAt(i).GetAttribute("value")
                    : QuantityStatic.ElementAt(i).Text;
                string totalPrice = TotalPrice.ElementAt(i).Text;

                ProductTable productTableItem = new ProductTable
                {
                    ProductSKU = productSKU,
                    ProductImage = productImage,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    Quantity = quantity,
                    TotalPrice = totalPrice,
                };

                productTableItems.Add(productTableItem);

                Console.WriteLine();
                Console.WriteLine($"Product Table Item {i + 1}:");
                Console.WriteLine($"Product Name: {productTableItem.ProductName}");
                Console.WriteLine($"Product SKU: {productTableItem.ProductSKU}");
                if (productImage != null)
                {
                    Console.WriteLine($"Product Image: {productTableItem.ProductImage}");
                }
                Console.WriteLine($"Unit Price: {productTableItem.UnitPrice}");
                Console.WriteLine($"Quantity: {productTableItem.Quantity}");
                Console.WriteLine($"Total Price: {productTableItem.TotalPrice}");
            }

            return productTableItems;
        }

        public PriceInfoBox GetPriceInfo()
        {
            _driver.WaitForLoad();
            string subtotal = PriceBoxSubtotal.Text;
            string shipping = PriceBoxShipping.Text;
            string tax = PriceBoxTax.Text;
            string total = PriceBoxTotal.Text;
            string? points = null;
            try
            {
                points = PriceBoxPoints.Text;
            }
            catch (WebDriverTimeoutException) { }

            return new PriceInfoBox
            {
                Subtotal = subtotal,
                Shipping = shipping,
                Tax = tax,
                Total = total,
                Points = points
            };
        }

        public void PrintPriceInfo()
        {
            var priceInfo = GetPriceInfo();

            Console.WriteLine();
            Console.WriteLine("Price info:");
            Console.WriteLine($"Subtotal: {priceInfo.Subtotal}");
            Console.WriteLine($"Shipping: {priceInfo.Shipping}");
            Console.WriteLine($"Tax: {priceInfo.Tax}");
            Console.WriteLine($"Total: {priceInfo.Total}");
            if (priceInfo.Points != null)
            {
                Console.WriteLine($"Points: {priceInfo.Points}");
            }
        }

        public class ProductTable
        {
            public string ProductSKU { get; set; }
            public string? ProductImage { get; set; }
            public string ProductName { get; set; }
            public string UnitPrice { get; set; }
            public string Quantity { get; set; }
            public string TotalPrice { get; set; }
        }

        public class PriceInfoBox
        {
            public string Subtotal { get; set; }
            public string Shipping { get; set; }
            public string Tax { get; set; }
            public string Total { get; set; }
            public string? Points { get; set; }
        }

        public class AddressInfoBox
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Company { get; set; }
            public string Address1 { get; set; }
            public string CityStateZip { get; set; }
            public string Country { get; set; }
        }

        public void CompareItems<T>(T expectedItem, T actualItem, Action<T, T, int> comparisonLogic)
        {
            Assert.Multiple(() =>
            {
                comparisonLogic(expectedItem, actualItem, 1);
            });
        }

        public void CompareItems<T>(List<T> expectedItems, List<T> actualItems, Action<T, T, int> comparisonLogic)
        {
            Assert.That(actualItems.Count, Is.EqualTo(expectedItems.Count), $"Number of items {expectedItems.Count} does not match actual items {actualItems.Count}.");

            for (int i = 0; i < expectedItems.Count; i++)
            {
                var expectedItem = expectedItems[i];
                var actualItem = actualItems[i];

                Assert.Multiple(() =>
                {
                    comparisonLogic(expectedItem, actualItem, i + 1);
                });
            }
        }

    }
}
