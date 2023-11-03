using EcommerceAppTestingFramework.Models.UiModels;
using EcommerceAppTestingFramework.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.BasePage;

namespace EcommerceAppTests.UITests
{
    public class ProductManagementTests : TestBase
    {
        private HomePage _homePage;
        private ProductPage _productPage;

        [SetUp] 
        public void SetUp() 
        {
            _homePage = new HomePage(_driver);
            _productPage = new ProductPage(_driver);
        }

        private void VerifyListing_Base(string category, string expectedPageTitle, bool isSubcategory = false)
        {
            _basePage.SelectCategoryLink(category);
            Assert.That(_basePage.PageLoaded(expectedPageTitle), Is.True, $"{category} page did not load correctly.");

            if (isSubcategory)
            {
                _productPage.GetAllSubcategories();
            }
            else
            {
                var products = _productPage.GetAllProductsList();
                Assert.That(products.Count > 0, Is.True, "No products are displayed on the page.");   
                
                foreach (var product in products)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(string.IsNullOrWhiteSpace(product.Name), Is.False, "Product name is empty.");
                        //Assert.That(string.IsNullOrWhiteSpace(product.Price), Is.False, "Product price is empty.");
                        Assert.That(string.IsNullOrWhiteSpace(product.ImageUrl), Is.False, "Product image URL is empty.");
                    });
                }
            }

            _extentReporting.LogInfo($"Selected product category '{category}', products displayed correctly");
        }

        [Test]
        [Category("Smoke_Test")]
        [Category("Positive_Test")]
        public void VerifyAllProductListings()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            VerifyListing_Base(Category.Computers, ProductPageTitle.Computers, true);
            VerifyListing_Base(Category.Electronics, ProductPageTitle.Electronics, true);
            VerifyListing_Base(Category.Apparel, ProductPageTitle.Apparel, true);
            VerifyListing_Base(Category.Desktops, ProductPageTitle.Desktops);
            VerifyListing_Base(Category.Notebooks, ProductPageTitle.Notebooks);
            VerifyListing_Base(Category.Software, ProductPageTitle.Software);
            VerifyListing_Base(Category.CameraPhoto, ProductPageTitle.CameraPhoto);
            VerifyListing_Base(Category.CellPhones, ProductPageTitle.CellPhones);
            VerifyListing_Base(Category.Others, ProductPageTitle.Others);
            VerifyListing_Base(Category.Shoes, ProductPageTitle.Shoes);
            VerifyListing_Base(Category.Clothing, ProductPageTitle.Clothing);
            VerifyListing_Base(Category.Accessories, ProductPageTitle.Accessories);
            VerifyListing_Base(Category.DigitalDownloads, ProductPageTitle.DigitalDownloads);
            VerifyListing_Base(Category.Books, ProductPageTitle.Books);
            VerifyListing_Base(Category.Jewelry, ProductPageTitle.Jewelry);
            VerifyListing_Base(Category.GiftCards, ProductPageTitle.GiftCards);
        }

        [Test]
        [Category("Smoke_Test")]
        [Category("Positive_Test")]
        public void VerifyProductsDetailPage()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            VerifyListing_Base(Category.Desktops, ProductPageTitle.Desktops);

            //select product by product id:

            //int productIdToVerify = 2;
            //Product selectedProduct = _productPage.GetProductFromListById(productIdToVerify);
            //_productPage.SelectProductDetailsById(productIdToVerify);

            //select product by index:

            int productIndexToVerify = 3;
            Product selectedProduct = _productPage.GetProductFromListByIndex(productIndexToVerify);
            _productPage.SelectProductDetailsByIndex(productIndexToVerify);

            Assert.That(_basePage.PageLoaded(selectedProduct.Name), Is.True, "Product details page did not load");

            Product productDetails = _productPage.GetProductDetails();

            Assert.Multiple(() =>
            {
                Assert.That(productDetails.Name, Is.EqualTo(selectedProduct.Name), "Product Name mismatch");
                Assert.That(selectedProduct.Price, Does.Contain(productDetails.Price), "Product Price mismatch");
                Assert.That(productDetails.ImageUrl, Is.EqualTo(selectedProduct.ImageUrl), "Product Image URL mismatch");
            });
            _extentReporting.LogInfo($"Navigated to product details page for product index {selectedProduct.Name} and confirmed product details");
        }

    }
}
