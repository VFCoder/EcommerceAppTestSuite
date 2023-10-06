using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Utils;
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
using static EcommerceAppTestingFramework.Utils.UserDataAndOrderVerifier;

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    [Category("UI_SmokeTests")]
    [Parallelizable]
    public class CustomerAppSmokeTest
    {
        private TestConfiguration _testConfig;
        private IDriverActions _driver;
        private BasePage _basePage;
        private HomePage _homePage;
        private LoginPage _loginPage;
        private RegisterPage _registerPage;
        private ProductPage _productPage;
        private UserDataAndOrderVerifier _verifier;
        private CartPage _cartPage;
        private CheckoutPage _checkoutPage;
        private OrderDetailsPage _orderDetailsPage;
        private string _baseUrl;
        private string _adminUrl;
        private string _apiUrl;
        private string _sqlConnection;

        [SetUp]
        public void Setup()
        {

            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig);
            _basePage = new BasePage(_driver);
            _homePage = new HomePage(_driver);
            _registerPage = new RegisterPage(_driver);
            _loginPage = new LoginPage(_driver);
            _productPage = new ProductPage(_driver);
            _verifier = new UserDataAndOrderVerifier(_driver);
            _cartPage = new CartPage(_driver);
            _checkoutPage = new CheckoutPage(_driver);
            _orderDetailsPage = new OrderDetailsPage(_driver);
            _baseUrl = _testConfig.GetBaseUrl();
            _adminUrl = _testConfig.GetAdminUrl();
            _apiUrl = _testConfig.GetApiUrl();
            _sqlConnection = _testConfig.GetSqlConnection();

        }

        [TearDown]
        public void Teardown()
        {
            _driver.Dispose();
        }

        [Test]
        public void RegisterNewCustomerTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.ClickRegisterLink();
            Assert.That(_basePage.PageLoaded(_registerPage.pageTitle), Is.True, "Register page did not load correctly.");

            _registerPage.SelectGender(Gender.Female);
            _registerPage.EnterFirstName("Mary");
            _registerPage.EnterLastName("Hicks");
            _registerPage.SelectBirthDateDay("13");
            _registerPage.SelectBirthDateMonth("June");
            _registerPage.SelectBirthDateYear("1995");
            _registerPage.EnterEmail("MaryHicks123@email.com");
            _registerPage.EnterCompany("");
            _registerPage.EnterPassword("UserPassword123");
            _registerPage.EnterConfirmPassword("UserPassword123");
            _registerPage.ClickRegisterButton();
            Assert.That(_registerPage.IsRegistrationCompleted(), Is.True, "Registration completed message was not displayed correctly.");

        }

        [Test]
        public void LoginCustomerTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");

            _loginPage.EnterLoginEmail("MaryHicks123@email.com");
            _loginPage.EnterLoginPassword("UserPassword123");
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Login was not successful.");

        }

        [Test]
        public void LogoutCustomerTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");

            _loginPage.EnterLoginEmail("MaryHicks123@email.com");
            _loginPage.EnterLoginPassword("UserPassword123");
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Login was not successful.");

            _basePage.ClickLogoutLink();
            Assert.That(_basePage.IsLoginLinkDisplayed, Is.True, "Logout was not successful.");

        }

        [Test]
        public void VerifyProductListingsTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            VerifyListing(Category.Computers, ProductPageTitle.Computers, true);
            VerifyListing(Category.Electronics, ProductPageTitle.Electronics, true);
            VerifyListing(Category.Apparel, ProductPageTitle.Apparel, true);
            VerifyListing(Category.Desktops, ProductPageTitle.Desktops);
            VerifyListing(Category.Notebooks, ProductPageTitle.Notebooks);
            VerifyListing(Category.Software, ProductPageTitle.Software);
            VerifyListing(Category.CameraPhoto, ProductPageTitle.CameraPhoto);
            VerifyListing(Category.CellPhones, ProductPageTitle.CellPhones);
            VerifyListing(Category.Others, ProductPageTitle.Others);
            VerifyListing(Category.Shoes, ProductPageTitle.Shoes);
            VerifyListing(Category.Clothing, ProductPageTitle.Clothing);
            VerifyListing(Category.Accessories, ProductPageTitle.Accessories);
            VerifyListing(Category.DigitalDownloads, ProductPageTitle.DigitalDownloads);
            VerifyListing(Category.Books, ProductPageTitle.Books);
            VerifyListing(Category.Jewelry, ProductPageTitle.Jewelry);
            VerifyListing(Category.GiftCards, ProductPageTitle.GiftCards);
        }

        private void VerifyListing(string category, string expectedPageTitle, bool isSubcategory = false)
        {
            _basePage.SelectCategoryLink(category);
            Assert.That(_basePage.PageLoaded(expectedPageTitle), Is.True, $"{category} page did not load correctly.");

            if (isSubcategory)
            {
                _productPage.GetAllSubcategories();
            }
            else
            {
                _productPage.GetAllProductsList();
            }
        }



        [Test]
        public void TestBasicAppFunctionalityEndToEnd()
        {
            //login:

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");

            _loginPage.EnterLoginEmail("admin@vfcoder.com");
            _loginPage.EnterLoginPassword("adminvfcoder");
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.AdminLoggedIn(), Is.True, "Admin login failed.");

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

            //add product to cart and confirm cart:

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
            _checkoutPage.InputCardHolderName("Bob Jones");
            _checkoutPage.InputCardNumber("1111222233334444");
            _checkoutPage.SelectExpireMonth("04");
            _checkoutPage.SelectExpireYear("2030");
            _checkoutPage.InputCardCode("123");
            _checkoutPage.ClickContinueFromPaymentInfo();

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

            PriceInfoBox checkoutPriceInfo = _verifier.GetPriceInfo();
            _verifier.PrintPriceInfo();

            //CompareCartWithCheckout(cartItems, checkoutItems);


            _verifier.CompareItems(cartPriceInfo, checkoutPriceInfo, (expected, actual, index) =>
            {
                Assert.That(actual.Subtotal, Is.EqualTo(expected.Subtotal), $"Subtotal does not match for item {index}.");
                Assert.That(actual.Shipping, Is.EqualTo(expected.Shipping), $"Shipping does not match for item {index}.");
                Assert.That(actual.Tax, Is.EqualTo(expected.Tax), $"Tax does not match for item {index}.");
                Assert.That(actual.Total, Is.EqualTo(expected.Total), $"Total does not match for item {index}.");
                Assert.That(actual.Points, Is.EqualTo(expected.Points), $"Points do not match for item {index}.");
            });

            //ComparePriceInfo(cartPriceInfo, checkoutPriceInfo);

            _checkoutPage.ClickConfirmOrderBtn();
            Assert.That(_checkoutPage.IsConfirmOrderMessageDisplayed(), Is.True, "Confirm order message not displayed");

            string orderNumberCheckout = _checkoutPage.GetOrderNumber();

            _checkoutPage.ClickOrderDetailsLink();

            Assert.That(_basePage.PageLoaded(_orderDetailsPage.pageTitle), Is.True, "Order Details page did not load");

            string orderNumberOrderDetails = _orderDetailsPage.GetOrderNumber();
            Assert.That(orderNumberCheckout, Is.EqualTo(orderNumberOrderDetails), "Order numbers do not match");

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

            PriceInfoBox orderDetailsPriceInfo = _verifier.GetPriceInfo();
            _verifier.PrintPriceInfo();

            _verifier.CompareItems(checkoutPriceInfo, orderDetailsPriceInfo, (expected, actual, index) =>
            {
                Assert.That(actual.Subtotal, Is.EqualTo(expected.Subtotal), $"Subtotal does not match for item {index}.");
                Assert.That(actual.Shipping, Is.EqualTo(expected.Shipping), $"Shipping does not match for item {index}.");
                Assert.That(actual.Tax, Is.EqualTo(expected.Tax), $"Tax does not match for item {index}.");
                Assert.That(actual.Total, Is.EqualTo(expected.Total), $"Total does not match for item {index}.");
            });

        }



        /*        private void CompareCartWithCheckout(List<ProductTable> cartItems, List<ProductTable> checkoutItems)
                {
                    Assert.That(checkoutItems.Count, Is.EqualTo(cartItems.Count), $"Number of cart items {cartItems.Count} does not match checkout items {checkoutItems.Count}.");

                    for (int i = 0; i < cartItems.Count; i++)
                    {
                        var cartItem = cartItems[i];
                        var checkoutItem = checkoutItems[i];
                        Assert.Multiple(() =>
                        {
                            Assert.That(checkoutItem.ProductName, Is.EqualTo(cartItem.ProductName), $"Product Name for item {i + 1} does not match.");
                            Assert.That(checkoutItem.ProductSKU, Is.EqualTo(cartItem.ProductSKU), $"Product SKU for item {i + 1} does not match.");
                            Assert.That(checkoutItem.ProductImage, Is.EqualTo(cartItem.ProductImage), $"Product SKU for item {i + 1} does not match.");
                            Assert.That(checkoutItem.UnitPrice, Is.EqualTo(cartItem.UnitPrice), $"Unit Price for item {i + 1} does not match.");
                            Assert.That(checkoutItem.Quantity, Is.EqualTo(cartItem.Quantity), $"Quantity for item {i + 1} does not match.");
                            Assert.That(checkoutItem.TotalPrice, Is.EqualTo(cartItem.TotalPrice), $"Total Price for item {i + 1} does not match.");
                        });
                    }
                }

                private void ComparePriceInfo(PriceInfoBox cartPriceInfo, PriceInfoBox checkoutPriceInfo)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checkoutPriceInfo.Subtotal, Is.EqualTo(cartPriceInfo.Subtotal), "Subtotal does not match.");
                        Assert.That(checkoutPriceInfo.Shipping, Is.EqualTo(cartPriceInfo.Shipping), "Shipping does not match.");
                        Assert.That(checkoutPriceInfo.Tax, Is.EqualTo(cartPriceInfo.Tax), "Tax does not match.");
                        Assert.That(checkoutPriceInfo.Total, Is.EqualTo(cartPriceInfo.Total), "Total does not match.");
                        Assert.That(checkoutPriceInfo.Points, Is.EqualTo(cartPriceInfo.Points), "Points do not match.");
                    });
                }

                private void ComparePriceInfo2(PriceInfoBox checkoutPriceInfo, PriceInfoBox orderDetailsPriceInfo)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checkoutPriceInfo.Subtotal, Is.EqualTo(orderDetailsPriceInfo.Subtotal), "Subtotal does not match.");
                        Assert.That(checkoutPriceInfo.Shipping, Is.EqualTo(orderDetailsPriceInfo.Shipping), "Shipping does not match.");
                        Assert.That(checkoutPriceInfo.Tax, Is.EqualTo(orderDetailsPriceInfo.Tax), "Tax does not match.");
                        Assert.That(checkoutPriceInfo.Total, Is.EqualTo(orderDetailsPriceInfo.Total), "Total does not match.");
                    });
                }

                private void CompareCheckoutWithOrderDetails(List<ProductTable> checkoutItems, List<ProductTable> orderDetailsItems)
                {
                    Assert.That(orderDetailsItems.Count, Is.EqualTo(checkoutItems.Count), $"Number of checkout items {checkoutItems.Count} does not match order details items {orderDetailsItems.Count}.");

                    for (int i = 0; i < checkoutItems.Count; i++)
                    {
                        var checkoutItem = checkoutItems[i];
                        var orderDetailsItem = orderDetailsItems[i];
                        Assert.Multiple(() =>
                        {
                            Assert.That(orderDetailsItem.ProductName, Is.EqualTo(checkoutItem.ProductName), $"Product Name for item {i + 1} does not match.");
                            Assert.That(orderDetailsItem.ProductSKU, Is.EqualTo(checkoutItem.ProductSKU), $"Product SKU for item {i + 1} does not match.");
                            Assert.That(orderDetailsItem.UnitPrice, Is.EqualTo(checkoutItem.UnitPrice), $"Unit Price for item {i + 1} does not match.");
                            Assert.That(orderDetailsItem.Quantity, Is.EqualTo(checkoutItem.Quantity), $"Quantity for item {i + 1} does not match.");
                            Assert.That(orderDetailsItem.TotalPrice, Is.EqualTo(checkoutItem.TotalPrice), $"Total Price for item {i + 1} does not match.");
                        });
                    }


                }*/


    }
}
