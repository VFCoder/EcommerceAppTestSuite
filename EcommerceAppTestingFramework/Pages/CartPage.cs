using EcommerceAppTestingFramework.Drivers;
using EcommerceAppTestingFramework.Models.UiModels;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Pages
{
    public class CartPage
    {

        private readonly IDriverActions _driver;

        public CartPage(IDriverActions driver)
        {
            _driver = driver;
        }

        private IReadOnlyCollection<IWebElement> CartRows => _driver.FindElements(By.CssSelector(".cart tbody tr"));
        private IReadOnlyCollection<IWebElement> CartProductSKU => _driver.FindElements(By.CssSelector("td.sku .sku-number"));
        private IReadOnlyCollection<IWebElement> CartProductImage => _driver.FindElements(By.CssSelector("td.product-picture img"));
        private IReadOnlyCollection<IWebElement> CartProductName => _driver.FindElements(By.CssSelector("td.product .product-name"));
        private IReadOnlyCollection<IWebElement> CartUnitPrice => _driver.FindElements(By.CssSelector("td.unit-price .product-unit-price"));
        private IReadOnlyCollection<IWebElement> CartQuantity => _driver.FindElements(By.CssSelector("td.quantity .qty-input"));
        private IReadOnlyCollection<IWebElement> CartTotalPrice => _driver.FindElements(By.CssSelector("td.subtotal .product-subtotal"));
        private IWebElement TermsOfServiceCheckbox => _driver.FindElementWait(By.Id("termsofservice"));
        private IWebElement CheckoutBtn => _driver.FindElementWait(By.Id("checkout"));
        private IWebElement PriceBoxSubtotal => _driver.FindElementWait(By.CssSelector(".order-subtotal .value-summary"));
        private IWebElement PriceBoxShipping => _driver.FindElementWait(By.CssSelector(".shipping-cost .value-summary"));
        private IWebElement PriceBoxTax => _driver.FindElementWait(By.CssSelector(".tax-value .value-summary"));
        private IWebElement PriceBoxTotal => _driver.FindElementWait(By.CssSelector(".order-total .value-summary"));
        private IWebElement PriceBoxPoints => _driver.FindElementWait(By.CssSelector(".earn-reward-points .value-summary"));
        private IWebElement ProductItemByCartOrder(int number) => _driver.FindElementWait(By.CssSelector($"tr:nth-of-type({number}) td.product"));
        private IWebElement CartLink => _driver.FindElementWait(By.CssSelector(".ico-cart"));
        private IWebElement CartQuantityIcon => _driver.FindElementWait(By.CssSelector(".ico-cart .cart-qty"));
        private IWebElement RemoveProductFromCartBtn => _driver.FindElementWait(By.CssSelector(".remove-btn"));
        private IWebElement EmptyCartMsg => _driver.FindElementWait(By.CssSelector(".order-summary-content .no-data"));


        public string pageTitle = "Shopping Cart";

        public bool AreProductsDisplayedInCart()
        {
            try
            {
                return CartRows.Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public int GetProductsInCartCount()
        {
            return CartRows.Count;
        }

        public string? GetProductNameByCartOrder(int order)
        {
            try
            {
                return ProductItemByCartOrder(order).Text;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<CartItem> GetCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>();

            IReadOnlyCollection<IWebElement> cartRows = CartRows;

            for (int i = 0; i < cartRows.Count; i++)
            {
                string productSKU = CartProductSKU.ElementAt(i).Text;
                string productImage = CartProductImage.ElementAt(i).GetAttribute("src");
                string productName = CartProductName.ElementAt(i).Text;
                string unitPrice = CartUnitPrice.ElementAt(i).Text;
                string quantity = CartQuantity.ElementAt(i).GetAttribute("value");
                string totalPrice = CartTotalPrice.ElementAt(i).Text;

                productImage = productImage[..^8];

                CartItem cartItem = new CartItem
                {
                    ProductSKU = productSKU,
                    ProductImage = productImage,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    Quantity = quantity,
                    TotalPrice = totalPrice
                };

                cartItems.Add(cartItem);

                Console.WriteLine();
                Console.WriteLine($"Cart Item {i + 1}:");
                Console.WriteLine($"Product Name: {cartItem.ProductName}");
                Console.WriteLine($"Product SKU: {cartItem.ProductSKU}");
                Console.WriteLine($"Product Image: {cartItem.ProductImage}");
                Console.WriteLine($"Unit Price: {cartItem.UnitPrice}");
                Console.WriteLine($"Quantity: {cartItem.Quantity}");
                Console.WriteLine($"Total Price: {cartItem.TotalPrice}");
            }

            return cartItems;
        }

        public PriceInfo GetPriceInfo()
        {
            string subtotal = PriceBoxSubtotal.Text;
            string shipping = PriceBoxShipping.Text;
            string tax = PriceBoxTax.Text;
            string total = PriceBoxTotal.Text;
            string points = PriceBoxPoints.Text;

            return new PriceInfo
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
            Console.WriteLine($"Points: {priceInfo.Points}");
        }

        public void ClickTermsOfService()
        {
            TermsOfServiceCheckbox.Click();
        }

        public void ClickCheckoutButton()
        {
            CheckoutBtn.Click();
        }


        public void ClearCart()
        {
            CartLink.Click();

            IReadOnlyCollection<IWebElement> cartRows = CartRows;

            foreach(var row  in cartRows)
            {
                RemoveProductFromCartBtn.Click();
            }

            Assert.That(EmptyCartMsg.Text, Does.Contain("Your Shopping Cart is empty!"), "Empty cart message was not displayed");
        }

        public void ConfirmCartIsCleared()
        {
            if (CartQuantityIcon.Text != "(0)")
            {
                ClearCart();
            }
        }

    }
}

