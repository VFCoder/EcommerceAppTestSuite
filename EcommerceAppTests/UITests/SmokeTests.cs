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
        private SearchPage _searchPage;
        private UserDataAndOrderVerifier _verifier;
        private CartPage _cartPage;
        private CheckoutPage _checkoutPage;
        private OrderDetailsPage _orderDetailsPage;
        private string _baseUrl;
        private string _adminUrl;
        private string _apiUrl;
        private string _sqlConnection;
        private BrowserType _browserType;

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
            _searchPage = new SearchPage(_driver);
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
        public void TestBasicAppFunctionalitiesFull()
        {                        
            string customerEmail = "Jake@email.com";
            string customerPassword = "UserPassword123";

            //Navigate to home page and confirm navigation links/menus are loaded:

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            //Register new customer:

/*            _basePage.ClickRegisterLink();
            Assert.That(_basePage.PageLoaded(_registerPage.pageTitle), Is.True, "Register page did not load correctly.");

            _registerPage.SelectGender(Gender.Female);
            _registerPage.EnterFirstName("Jake");
            _registerPage.EnterLastName("Nime");
            _registerPage.SelectBirthDateDay("21");
            _registerPage.SelectBirthDateMonth("February");
            _registerPage.SelectBirthDateYear("1995");
            _registerPage.EnterEmail(customerEmail);
            _registerPage.EnterCompany("");
            _registerPage.EnterPassword(customerPassword);
            _registerPage.EnterConfirmPassword(customerPassword);
            _registerPage.ClickRegisterButton();
            Assert.That(_registerPage.IsRegistrationCompleted(), Is.True, "Registration completed message was not displayed correctly.");

            //Log out customer:

            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Logout link not displayed.");
            _basePage.ClickLogoutLink();
            Assert.That(_basePage.IsLoginLinkDisplayed, Is.True, "Logout was not successful.");

            //Log in customer:
            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");

            _loginPage.EnterLoginEmail(customerEmail);
            _loginPage.EnterLoginPassword(customerPassword);
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Logout link not displayed.");

            //Perform product search and confirm product is returned:

            string searchText = "camera";

            _basePage.EnterSearchText(searchText);
            _basePage.ClickSearchButton();
            Assert.That(_basePage.PageLoaded(_searchPage.pageTitle), Is.True, "Search page did not load correctly.");
            Assert.That(_productPage.GetProductCount(), Is.GreaterThan(0), "No products returned from search");

            //Confirm product listing page:

            _basePage.SelectCategoryLink(Category.CellPhones);
            Assert.That(_basePage.PageLoaded(ProductPageTitle.CellPhones), Is.True, "Desktops page did not load correctly");
            Assert.That(_productPage.GetProductCount(), Is.GreaterThan(0), "No products displayed on product page");

            //Confirm product details of first product listed:

            string productName = _productPage.GetProductNameByListingOrder(1);

            _productPage.ClickProductDetailsByListingOrder(1);
            Assert.That(_basePage.PageLoaded(productName), Is.True, "Product details page did not load correctly.");

            //Add product to cart:

            _productPage.AddProductToCartFromDetailsPage();
            Assert.That(_productPage.NotificationSuccessDisplayed(), Is.True, "Product added notification did not display");
            _productPage.CloseNotificationBar();

            //Confirm product is in cart and total price:

            _basePage.ClickCartLink();
            Assert.That(_basePage.PageLoaded("Shopping Cart"), Is.True, "Cart did not load");

            int productQuantity = _verifier.GetProductCount();

            Assert.That(productQuantity, Is.EqualTo(1), "Quantity of products in cart does not match");
            Assert.That(_cartPage.GetProductNameByCartOrder(1), Is.EqualTo(productName), "Product name in cart does not match");

            string cartTotalPrice = _verifier.GetTotalPrice();

            //Check out:

            _cartPage.ClickTermsOfService();
            _cartPage.ClickCheckoutButton();
            Assert.That(_basePage.PageLoaded(_checkoutPage.pageTitle), Is.True, "Checkout page did not load");

            //Complete address details:

            _checkoutPage.SelectBillingAddressCountryDropdown("United States of America");
            _checkoutPage.SelectBillingAddressStateDropdown("Florida");
            _checkoutPage.EnterBillingAddressCity("Tampa");
            _checkoutPage.EnterBillingAddressStreet("123 Tampa Ave");
            _checkoutPage.EnterBillingAddressZip("12345");
            _checkoutPage.EnterBillingAddressPhone("1234567890");
            _checkoutPage.ClickContinueFromBillingAddress();

            //Choose shipping method:

            _checkoutPage.SelectShippingMethod(ShippingMethod.NextDayAir);
            _checkoutPage.ClickContinueFromShippingMethod();

            //Choose payment method:

            _checkoutPage.SelectPaymentMethod(PaymentMethod.CreditCard);
            _checkoutPage.ClickContinueFromPaymentMethod();

            //Enter credit card details:

            _checkoutPage.SelectCreditCartType(CreditCardType.MasterCard);
            _checkoutPage.InputCardHolderName("Joe Bidson");
            _checkoutPage.InputCardNumber("1111222233334444");
            _checkoutPage.SelectExpireMonth("04");
            _checkoutPage.SelectExpireYear("2030");
            _checkoutPage.InputCardCode("123");
            _checkoutPage.ClickContinueFromPaymentInfo();

            //Verify correct product and total price:

            Assert.That(_verifier.GetProductCount(), Is.EqualTo(productQuantity), "Quantity of products in checkout does not match");
            Assert.That(_cartPage.GetProductNameByCartOrder(1), Is.EqualTo(productName), "Product name in cart does not match");

            string checkoutTotalPrice = _verifier.GetTotalPrice();

            Assert.That(cartTotalPrice, Is.EqualTo(checkoutTotalPrice), "Total price does not match");

            //Place order:

            _checkoutPage.ClickConfirmOrderBtn();
            Assert.That(_checkoutPage.IsConfirmOrderMessageDisplayed(), Is.True, "Confirm order message not displayed");

            //Confirm order number:

            string orderNumberCheckout = _checkoutPage.GetOrderNumber();

            _checkoutPage.ClickOrderDetailsLink();
            Assert.That(_basePage.PageLoaded(_orderDetailsPage.pageTitle), Is.True, "Order Details page did not load");

            string orderNumberOrderDetails = _orderDetailsPage.GetOrderNumber();

            Assert.That(orderNumberCheckout, Is.EqualTo(orderNumberOrderDetails), "Order numbers do not match");
*/
        }
    }
}
