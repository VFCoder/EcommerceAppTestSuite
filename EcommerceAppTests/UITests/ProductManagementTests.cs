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
        [TestCase(Category.Computers, ProductPageTitle.Computers, true)]
        [TestCase(Category.Electronics, ProductPageTitle.Electronics, true)]
        [TestCase(Category.Apparel, ProductPageTitle.Apparel, true)]
        [TestCase(Category.Desktops, ProductPageTitle.Desktops)]
        [TestCase(Category.Notebooks, ProductPageTitle.Notebooks)]
        [TestCase(Category.Software, ProductPageTitle.Software)]
        [TestCase(Category.CameraPhoto, ProductPageTitle.CameraPhoto)]
        [TestCase(Category.CellPhones, ProductPageTitle.CellPhones)]
        [TestCase(Category.Others, ProductPageTitle.Others)]
        [TestCase(Category.Shoes, ProductPageTitle.Shoes)]
        [TestCase(Category.Clothing, ProductPageTitle.Clothing)]
        [TestCase(Category.Accessories, ProductPageTitle.Accessories)]
        [TestCase(Category.DigitalDownloads, ProductPageTitle.DigitalDownloads)]
        [TestCase(Category.Books, ProductPageTitle.Books)]
        [TestCase(Category.Jewelry, ProductPageTitle.Jewelry)]
        [TestCase(Category.GiftCards, ProductPageTitle.GiftCards)]

        public void VerifyAllProductListings(string productCategory, string productPageTitle, bool isSubcategory = false)
        {
            //verify product listing page
            VerifyListing_Base(productCategory, productPageTitle, isSubcategory);
        }

        [Test]
        [Category("Smoke_Test")]
        [Category("Positive_Test")]
        [TestCase(Category.Desktops, ProductPageTitle.Desktops, 2)]

        public void VerifyProductsDetailPage(string productCategory, string productPageTitle, int productIndex)
        {
            //verify product listing page
            VerifyListing_Base(productCategory, productPageTitle);

/*            //select product by product id:

            Product selectedProduct = _productPage.GetProductFromListById(productId);
            _productPage.SelectProductDetailsById(productId);*/

            //select product details by index
            Product selectedProduct = _productPage.GetProductFromListByIndex(productIndex);
            _productPage.SelectProductDetailsByIndex(productIndex);

            Assert.That(_basePage.PageLoaded(selectedProduct.Name), Is.True, "Product details page did not load");

            //verify product details are correct
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
