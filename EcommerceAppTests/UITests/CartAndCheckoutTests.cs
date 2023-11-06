using EcommerceAppTestingFramework;
using EcommerceAppTestingFramework.Models.UiModels;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Reports;
using EcommerceAppTestingFramework.TestData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.BasePage;
using static EcommerceAppTestingFramework.Pages.CartPage;
using static EcommerceAppTestingFramework.Pages.CheckoutPage;
using static EcommerceAppTestingFramework.Pages.UserDataAndOrderVerifier;

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    //[Parallelizable(ParallelScope.All)]

    public class CartAndCheckoutTests : TestBase
    {
        protected HomePage _homePage;
        protected ProductPage _productPage;
        protected UserDataAndOrderVerifier _verifier;
        protected CartPage _cartPage;
        protected CheckoutPage _checkoutPage;
        protected OrderDetailsPage _orderDetailsPage;
        protected LoginPage _loginPage;
        protected DataGenerator _dataGenerator;
        protected BogusData _bogusData;
        protected UserAuthenticationTests _userAuthTests;

        //static variables to share data between tests instead of global objects
        private static List<ProductTable> cartItems;
        private static List<ProductTable> checkoutItems;
        private static PriceInfoBox cartPriceInfo;
        private static PriceInfoBox checkoutPriceInfo;
        private static string selectedPaymentMethod;
        private static string selectedShippingMethod;
        private static Product selectedProduct;
        private static Product productDetails;

        [SetUp] 
        public void SetUp() 
        {         
            _userAuthTests = new UserAuthenticationTests();
            _homePage = new HomePage(_driver);
            _productPage = new ProductPage(_driver);
            _verifier = new UserDataAndOrderVerifier(_driver);
            _cartPage = new CartPage(_driver);
            _checkoutPage = new CheckoutPage(_driver);
            _orderDetailsPage = new OrderDetailsPage(_driver);
            _loginPage = new LoginPage(_driver);
            _dataGenerator = new DataGenerator();
            _bogusData = _dataGenerator.GenerateData();
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        [TestCase(Category.Desktops, ProductPageTitle.Desktops, 2)]
        [TestCase(Category.CameraPhoto, ProductPageTitle.CameraPhoto, 2)]
        public void AddProductToCartFromProductListingPage(string categoryToSelect, string expectedPageTitle, int productIndexToSelect)
        {
            //empty cart contents if any
            _cartPage.ConfirmCartIsCleared();

            //select product category
            _basePage.SelectCategoryLink(categoryToSelect);
            Assert.That(_basePage.PageLoaded(expectedPageTitle), Is.True, $"{categoryToSelect} page did not load correctly.");

/*            //select product by id:

            //int productIdToSelect = 1;
            //Product selectedProduct = _productPage.GetProductFromListById(productIdToSelect);
            //_productPage.AddProductToCartById(productIdToSelect);*/

            //select product by index
            selectedProduct = _productPage.GetProductFromListByIndex(productIndexToSelect);
            _productPage.AddProductToCartByIndex(productIndexToSelect);

            Assert.That(_productPage.NotificationSuccessDisplayed(), Is.True, "Product added notification did not display");
            _productPage.CloseNotificationBar();

            //verify cart contents
            VerifyCartFromProductListingPage();
        }

        private void VerifyCartFromProductListingPage()
        {
            _basePage.ClickShoppingCartLink();
            Assert.That(_basePage.PageLoaded("Shopping Cart"), Is.True, "Cart did not load");

            var cartVerifier = new UserDataAndOrderVerifier(_driver, isCartPageContext: true);

            cartItems = cartVerifier.GetProductTableItems();

            cartPriceInfo = _verifier.GetPriceInfo();

            _verifier.PrintPriceInfo();

            bool productFoundInCart = false;

            foreach (var cartItem in cartItems)
            {
                Console.WriteLine($"Cart item: {cartItem.ProductName} {cartItem.UnitPrice} {cartItem.ProductSKU} {cartItem.Quantity}");

                if (cartItem.ProductName == selectedProduct.Name &&
                    cartItem.UnitPrice == selectedProduct.Price)
                {
                    productFoundInCart = true;
                }
            }

            Assert.Multiple(() =>
            {
                Assert.That(productFoundInCart, Is.True, $"The product '{selectedProduct.Name}' with price '{selectedProduct.Price}' was not found in the cart.");
            });

            _extentReporting.LogInfo($"Confirmed product {selectedProduct.Name} and price {selectedProduct.Price} in cart");
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        [TestCase(Category.Desktops, ProductPageTitle.Desktops, 2)]
        [TestCase(Category.CameraPhoto, ProductPageTitle.CameraPhoto, 2)]
        public void AddProductToCartFromProductDetailsPage(string categoryToSelect, string expectedPageTitle, int productIndexToSelect)
        {
            //empty cart contents if any
            _cartPage.ConfirmCartIsCleared();

            //select product category
            _basePage.SelectCategoryLink(categoryToSelect);
            Assert.That(_basePage.PageLoaded(expectedPageTitle), Is.True, $"{categoryToSelect} page did not load correctly.");

/*            //select product by id:

            //Product selectedProduct = _productPage.GetProductFromListById(productIdToSelect);
            //_productPage.SelectProductDetailsById(productIdToSelect);*/

            //select product by index
            Product selectedProduct = _productPage.GetProductFromListByIndex(productIndexToSelect);
            _productPage.SelectProductDetailsByIndex(productIndexToSelect);

            Assert.That(_basePage.PageLoaded(selectedProduct.Name), Is.True, "Product details page did not load");

            //verify cart contents
            VerifyCartFromProductDetailsPage();
        }

        private void VerifyCartFromProductDetailsPage()
        {
            productDetails = _productPage.GetProductDetails();

            _productPage.AddProductToCartFromDetailsPage();
            Assert.That(_productPage.NotificationSuccessDisplayed(), Is.True, "Product added notification did not display");
            _productPage.CloseNotificationBar();

            _basePage.ClickShoppingCartLink();
            Assert.That(_basePage.PageLoaded("Shopping Cart"), Is.True, "Cart did not load");

            var cartVerifier = new UserDataAndOrderVerifier(_driver, isCartPageContext: true);
            cartItems = cartVerifier.GetProductTableItems();

            cartPriceInfo = _verifier.GetPriceInfo();
            _verifier.PrintPriceInfo();

            bool productFoundInCart = false;
            bool productQuantityCorrect = false;

            foreach (var cartItem in cartItems)
            {
                if (cartItem.ProductName == productDetails.Name &&
                    cartItem.UnitPrice == productDetails.Price)
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
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        [TestCase(Category.Desktops, ProductPageTitle.Desktops, 2)]

        public void EmptyCart(string categoryToSelect, string expectedPageTitle, int productIndexToSelect)
        {
            AddProductToCartFromProductListingPage(categoryToSelect, expectedPageTitle, productIndexToSelect);
            _cartPage.ConfirmCartIsCleared();
        }

        [Test]
        [Category("End_to_End_Test")]
        [Category("Positive_Test")]
        [TestCase(Category.Desktops, ProductPageTitle.Desktops, 2)]

        public void CheckoutFlow_LoginDuringCheckout(string categoryToSelect, string expectedPageTitle, int productIndexToSelect)
        {

            //Add product to cart
            AddProductToCartFromProductListingPage(categoryToSelect, expectedPageTitle, productIndexToSelect);

            //click checkout
            _cartPage.ClickTermsOfService();
            _cartPage.ClickCheckoutButton();

            //login
            _loginPage.LoginHelper(ValidUserData.Email, ValidUserData.Password);

            Assert.That(_basePage.PageLoaded(_cartPage.pageTitle), Is.True, "Cart page page did not load");

            CompleteCheckoutSteps();

            //verify all order information before confirming order
            VerifyOrderInformationBeforePlacingOrder();

            //submit order
            _checkoutPage.ClickConfirmOrderBtn();
            Assert.That(_checkoutPage.IsConfirmOrderMessageDisplayed(), Is.True, "Confirm order message not displayed");
            _extentReporting.LogInfo($"Placed ordrer");

            //verify order details after confirming order
            VerifyOrderInformationAfterPlacingOrder();
        }

        private void CompleteCheckoutSteps()
        {
            //click checkout
            _cartPage.ClickTermsOfService();
            _cartPage.ClickCheckoutButton();

            //address info is already added
            _checkoutPage.ClickContinueFromBillingAddress();

            //select shipping method
            _checkoutPage.SelectShippingtMethod(ShippingMethod.NextDayAir);
            selectedShippingMethod = _checkoutPage.GetSelectedShippingMethod(ShippingMethod.NextDayAir);
            _checkoutPage.ClickContinueFromShippingMethod();

            //select payment method
            _checkoutPage.SelectPaymentMethod(PaymentMethod.CreditCard);
            selectedPaymentMethod = _checkoutPage.GetSelectedPaymentMethod(PaymentMethod.CreditCard);
            _checkoutPage.ClickContinueFromPaymentMethod();

            //complete credit card form
            _checkoutPage.CreditCardFormHelper(CreditCardType.MasterCard, ValidUserData.FullName, _bogusData.CardNumber, _bogusData.ExpMonth, _bogusData.ExpYear, _bogusData.CVC);
            _extentReporting.LogInfo($"Checked out and submitted shipping and payment details");
        }

        private void VerifyOrderInformationBeforePlacingOrder()
        {

            //verify address, payment and shipping details
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

            //verify product details
            var checkoutVerifier = new UserDataAndOrderVerifier(_driver);
            checkoutItems = _verifier.GetProductTableItems();

            Console.WriteLine(checkoutItems.Count);

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

            //verify price details
            checkoutPriceInfo = _verifier.GetPriceInfo();
            _verifier.PrintPriceInfo();

            _verifier.CompareItems(cartPriceInfo, checkoutPriceInfo, (expected, actual, index) =>
            {
                Assert.That(actual.Subtotal, Is.EqualTo(expected.Subtotal), $"Subtotal does not match for item {index}.");
                Assert.That(actual.Shipping, Is.EqualTo(expected.Shipping), $"Shipping does not match for item {index}.");
                Assert.That(actual.Tax, Is.EqualTo(expected.Tax), $"Tax does not match for item {index}.");
                Assert.That(actual.Total, Is.EqualTo(expected.Total), $"Total does not match for item {index}.");
            });
            _extentReporting.LogInfo($"Confirmed price details");
        }

        private void VerifyOrderInformationAfterPlacingOrder()
        {
            //verify order number
            string orderNumberCheckout = _checkoutPage.GetOrderNumber();

            _checkoutPage.ClickOrderDetailsLink();
            Assert.That(_basePage.PageLoaded(_orderDetailsPage.pageTitle), Is.True, "Order Details page did not load");
            _extentReporting.LogInfo($"Navigated to order details page");

            string orderNumberOrderDetails = _orderDetailsPage.GetOrderNumber();
            Assert.That(orderNumberCheckout, Is.EqualTo(orderNumberOrderDetails), "Order numbers do not match");
            _extentReporting.LogInfo($"Confirmed order number {orderNumberOrderDetails}");

            //verify product details after completing purchase:
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

            //verify price details after completing purchase
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
