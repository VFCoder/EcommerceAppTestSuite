using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Drivers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.BasePage;
using static EcommerceAppTestingFramework.Pages.CartPage;
using static EcommerceAppTestingFramework.Pages.CheckoutPage;
using static EcommerceAppTestingFramework.Pages.RegisterPage;
using static EcommerceAppTestingFramework.Pages.UserDataAndOrderVerifier;
using EcommerceAppTestingFramework.TestData;

namespace EcommerceAppTests.UITests.OldTests
{
    [TestFixture]
    [Category("UI_EndToEndTests")]
    [Parallelizable]
    public class EndToEndTests : TestBase
    {

        [Test]
        public void TestBasicAppFunctionalitiesEndToEnd()
        {
            _extentReporting.LogInfo("Starting End to End test - TestBasicAppFunctionalitiesEndToEnd");

            //navigate to home page:

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");
            _extentReporting.LogInfo("Navigated to base url");

            //login:

            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");

            _loginPage.EnterLoginEmail(ValidUserData.Email);
            _loginPage.EnterLoginPassword(ValidUserData.Password);
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Login was not successful.");
            _extentReporting.LogInfo("Submitted login credentials and logged in.");

            //confirm product listing page:

            _basePage.SelectCategoryLink(Category.Desktops);
            Assert.That(_basePage.PageLoaded("Desktops"), Is.True, "Desktops page did not load correctly");

            var products = _productPage.GetAllProductsList();
            Assert.That(products.Count > 0, Is.True, "No products are displayed on the page.");

            foreach (var product in products)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(string.IsNullOrWhiteSpace(product.Name), Is.False, "Product name is empty.");
                    Assert.That(string.IsNullOrWhiteSpace(product.Price), Is.False, "Product price is empty.");
                    Assert.That(string.IsNullOrWhiteSpace(product.ImageUrl), Is.False, "Product image URL is empty.");
                });
            }
            _extentReporting.LogInfo($"Selected product category '{Category.Desktops}', products displayed correctly");

            //confirm product details:

            int productIndexToAddToCart = 3;

            Product selectedProduct = _productPage.GetProductFromList(productIndexToAddToCart);

            _productPage.SelectProductDetailsByIndex(productIndexToAddToCart);
            Assert.That(_basePage.PageLoaded(selectedProduct.Name), Is.True, "Product details page did not load");

            Product productDetails = _productPage.GetProductDetails(productIndexToAddToCart);

            Assert.Multiple(() =>
            {
                Assert.That(productDetails.Name, Is.EqualTo(selectedProduct.Name), "Product Name mismatch");
                Assert.That(productDetails.Price, Is.EqualTo(selectedProduct.Price), "Product Price mismatch");
                Assert.That(productDetails.ImageUrl, Is.EqualTo(selectedProduct.ImageUrl), "Product Image URL mismatch");
            });
            _extentReporting.LogInfo($"Navigated to product details page for product index {productIndexToAddToCart} and confirmed product details");

            //add product to cart and confirm cart items:

            _productPage.AddProductToCartFromDetailsPage();
            Assert.That(_productPage.NotificationSuccessDisplayed(), Is.True, "Product added notification did not display");
            _productPage.CloseNotificationBar();

            _basePage.ClickCartLink();
            Assert.That(_basePage.PageLoaded("Shopping Cart"), Is.True, "Cart did not load");

            var cartVerifier = new UserDataAndOrderVerifier(_driver, isCartPageContext: true);
            List<ProductTable> cartItems = cartVerifier.GetProductTableItems();

            PriceInfoBox cartPriceInfo = _verifier.GetPriceInfo();
            _verifier.PrintPriceInfo();

            bool productFoundInCart = false;
            bool productQuantityCorrect = false;

            foreach (var cartItem in cartItems)
            {
                if (cartItem.ProductName == productDetails.Name &&
                    cartItem.UnitPrice == productDetails.Price &&
                    cartItem.ProductSKU == productDetails.SKU)
                {
                    productFoundInCart = true;

                    if (cartItem.Quantity == productDetails.Quantity)
                    {
                        productQuantityCorrect = true;
                        break;
                    }
                }
            }

            Assert.Multiple(() =>
            {
                Assert.That(productFoundInCart, Is.True, $"The product '{productDetails.Name}' with price '{selectedProduct.Price}' was not found in the cart.");
                Assert.That(productQuantityCorrect, Is.True, $"Quantity does not match for product '{productDetails.Name}'");
            });
            _extentReporting.LogInfo($"Confirmed product {productDetails.Name} and price {selectedProduct.Price} in cart");


            //Checkout:

            _cartPage.ClickTermsOfService();
            _cartPage.ClickCheckoutButton();

            Assert.That(_basePage.PageLoaded(_checkoutPage.pageTitle), Is.True, "Checkout page did not load");

            _checkoutPage.ClickContinueFromBillingAddress();

            _checkoutPage.SelectShippingtMethod(ShippingMethod.NextDayAir);
            string selectedShippingMethod = _checkoutPage.GetSelectedShippingMethod(ShippingMethod.NextDayAir);
            _checkoutPage.ClickContinueFromShippingMethod();

            _checkoutPage.SelectPaymentMethod(PaymentMethod.CreditCard);
            string selectedPaymentMethod = _checkoutPage.GetSelectedPaymentMethod(PaymentMethod.CreditCard);
            _checkoutPage.ClickContinueFromPaymentMethod();

            _checkoutPage.SelectCreditCartType(CreditCardType.MasterCard);
            _checkoutPage.InputCardHolderName(ValidUserData.FullName);
            _checkoutPage.InputCardNumber(_bogusData.CardNumber);
            _checkoutPage.SelectExpireMonth(_bogusData.ExpMonth);
            _checkoutPage.SelectExpireYear(_bogusData.ExpYear);
            _checkoutPage.InputCardCode(_bogusData.CVC);
            _checkoutPage.ClickContinueFromPaymentInfo();
            _extentReporting.LogInfo($"Checked out and submitted shipping and payment details");

            //verify checkout order details:

            string billingAddressText = _checkoutPage.GetAddressText(AddressType.Billing);
            string shippingAddressText = _checkoutPage.GetAddressText(AddressType.Shipping);
            string paymentMethodText = _checkoutPage.GetPaymentMethodText();
            string shippingMethodText = _checkoutPage.GetShippingMethodText();

            Assert.Multiple(() =>
            {
                Assert.That(paymentMethodText, Does.Contain(selectedPaymentMethod), "Selected payment method is not in billing address");
                Assert.That(shippingMethodText, Does.Contain(selectedShippingMethod), "Selected shipping method is not in shipping address");
                Assert.That(billingAddressText, Is.EqualTo(shippingAddressText), "Selected payment method is not in billing address");

            });
            _extentReporting.LogInfo($"Confirmed address details");

            var checkoutVerifier = new UserDataAndOrderVerifier(_driver);
            List<ProductTable> checkoutItems = checkoutVerifier.GetProductTableItems();

            _verifier.CompareItems(cartItems, checkoutItems, (expected, actual, index) =>
            {
                Assert.That(actual.ProductName, Is.EqualTo(expected.ProductName), $"Product Name for item {index} does not match.");
                Assert.That(actual.ProductSKU, Is.EqualTo(expected.ProductSKU), $"Product SKU for item {index} does not match.");
                Assert.That(actual.ProductImage, Is.EqualTo(expected.ProductImage), $"Product Image for item {index} does not match.");
                Assert.That(actual.UnitPrice, Is.EqualTo(expected.UnitPrice), $"Unit Price for item {index} does not match.");
                Assert.That(actual.Quantity, Is.EqualTo(expected.Quantity), $"Quantity for item {index} does not match.");
                Assert.That(actual.TotalPrice, Is.EqualTo(expected.TotalPrice), $"Total Price for item {index} does not match.");
            });
            _extentReporting.LogInfo($"Confirmed product details");

            PriceInfoBox checkoutPriceInfo = _verifier.GetPriceInfo();
            _verifier.PrintPriceInfo();

            _verifier.CompareItems(cartPriceInfo, checkoutPriceInfo, (expected, actual, index) =>
            {
                Assert.That(actual.Subtotal, Is.EqualTo(expected.Subtotal), $"Subtotal does not match for item {index}.");
                Assert.That(actual.Shipping, Is.EqualTo(expected.Shipping), $"Shipping does not match for item {index}.");
                Assert.That(actual.Tax, Is.EqualTo(expected.Tax), $"Tax does not match for item {index}.");
                Assert.That(actual.Total, Is.EqualTo(expected.Total), $"Total does not match for item {index}.");
                Assert.That(actual.Points, Is.EqualTo(expected.Points), $"Points do not match for item {index}.");
            });
            _extentReporting.LogInfo($"Confirmed price details");

            //submit order and confirm order number:

            _checkoutPage.ClickConfirmOrderBtn();
            Assert.That(_checkoutPage.IsConfirmOrderMessageDisplayed(), Is.True, "Confirm order message not displayed");
            _extentReporting.LogInfo($"Placed ordrer");

            string orderNumberCheckout = _checkoutPage.GetOrderNumber();

            _checkoutPage.ClickOrderDetailsLink();
            Assert.That(_basePage.PageLoaded(_orderDetailsPage.pageTitle), Is.True, "Order Details page did not load");
            _extentReporting.LogInfo($"Navigated to order details page");

            string orderNumberOrderDetails = _orderDetailsPage.GetOrderNumber();
            Assert.That(orderNumberCheckout, Is.EqualTo(orderNumberOrderDetails), "Order numbers do not match");
            _extentReporting.LogInfo($"Confirmed order number");

            //verify order details after purchase:

            var orderDetailsVerifier = new UserDataAndOrderVerifier(_driver);
            List<ProductTable> orderDetailsItems = orderDetailsVerifier.GetProductTableItems();

            _verifier.CompareItems(checkoutItems, orderDetailsItems, (expected, actual, index) =>
            {
                Assert.That(actual.ProductName, Is.EqualTo(expected.ProductName), $"Product Name for item {index} does not match.");
                Assert.That(actual.ProductSKU, Is.EqualTo(expected.ProductSKU), $"Product SKU for item {index} does not match.");
                Assert.That(actual.UnitPrice, Is.EqualTo(expected.UnitPrice), $"Unit Price for item {index} does not match.");
                Assert.That(actual.Quantity, Is.EqualTo(expected.Quantity), $"Quantity for item {index} does not match.");
                Assert.That(actual.TotalPrice, Is.EqualTo(expected.TotalPrice), $"Total Price for item {index} does not match.");
            });
            _extentReporting.LogInfo($"Confirmed product details");

            PriceInfoBox orderDetailsPriceInfo = _verifier.GetPriceInfo();
            _verifier.PrintPriceInfo();

            _verifier.CompareItems(checkoutPriceInfo, orderDetailsPriceInfo, (expected, actual, index) =>
            {
                Assert.That(actual.Subtotal, Is.EqualTo(expected.Subtotal), $"Subtotal does not match for item {index}.");
                Assert.That(actual.Shipping, Is.EqualTo(expected.Shipping), $"Shipping does not match for item {index}.");
                Assert.That(actual.Tax, Is.EqualTo(expected.Tax), $"Tax does not match for item {index}.");
                Assert.That(actual.Total, Is.EqualTo(expected.Total), $"Total does not match for item {index}.");
            });
            _extentReporting.LogInfo($"Confirmed price details");

        }

    }
}
