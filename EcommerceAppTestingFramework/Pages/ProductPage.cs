using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.CheckoutPage;
using static EcommerceAppTestingFramework.Utils.UserDataAndOrderVerifier;

namespace EcommerceAppTestingFramework.Pages
{
    public class ProductPage
    {
        private readonly IDriverActions _driver;

        public ProductPage(IDriverActions driver)
        {
            _driver = driver;
        }

        private IReadOnlyCollection<IWebElement> ProductItems => _driver.FindElements(By.CssSelector(".product-item"));
        private IWebElement PageTitle => _driver.FindElementWait(By.CssSelector(".page-title"));
        private IWebElement ProductListingName => _driver.FindElementWait(By.CssSelector(".product-title a"));
        private IWebElement ProductListingPrice => _driver.FindElementWait(By.CssSelector(".price.actual-price"));
        private IWebElement ProductListingImage => _driver.FindElementWait(By.CssSelector(".picture img"));
        private IReadOnlyCollection<IWebElement> SubcategoryItems => _driver.FindElements(By.CssSelector(".sub-category-item"));
        private IWebElement SubcategoryName => _driver.FindElementWait(By.CssSelector(".sub-category-item .title a"));
        private IWebElement SubcategoryImage => _driver.FindElementWait(By.CssSelector(".sub-category-item .picture img"));
        private IWebElement ProductByIndex(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}']"));
        private IWebElement ProductNameByIndex(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .product-title a"));
        private IWebElement ProductPriceByIndex(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .price.actual-price"));
        private IWebElement ProductImageByIndex(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .picture img"));
        private IWebElement ProductDetailsName(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .product-name h1"));
        private IWebElement ProductDetailsPrice(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .product-price span"));
        private IWebElement ProductDetailsImage(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .product-essential .picture img"));
        private IWebElement ProductDetailsQuantity(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .add-to-cart-panel .qty-input"));
        private IWebElement ProductDetailsSKU(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .product-essential .sku .value"));
        private IWebElement AddToCartBtnByIndex(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'][class*='add-to-cart-button']"));
        private IWebElement AddToCartBtnDetailsPage => _driver.FindElementWait(By.CssSelector(".add-to-cart-button"));
        private IWebElement NotificationAddedToCartSuccess => _driver.FindElementWait(By.CssSelector(".bar-notification.success"));
        private IWebElement CloseNotificationBtn => _driver.FindElementWait(By.CssSelector(".close"));


        public bool AreProductsDisplayed()
        {
            try
            {
                return ProductItems.Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public int GetProductCount()
        {
            return ProductItems.Count;
        }

        public List<Product>? GetAllProductsList()
        {
            List<Product> products = new List<Product>();

            if (ProductItems.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Number of products displayed for {PageTitle.Text}: {ProductItems.Count}");

                foreach (var productContainer in ProductItems)
                {
                    IWebElement ProductListingName = productContainer.FindElement(By.CssSelector(".product-title a"));
                    IWebElement ProductListingPrice = productContainer.FindElement(By.CssSelector(".price.actual-price"));
                    IWebElement ProductListingImage = productContainer.FindElement(By.CssSelector(".picture img"));

                    string productName = ProductListingName.Text;
                    string productPrice = ProductListingPrice.Text;
                    string productImageUrl = ProductListingImage.GetAttribute("src");

                    Assert.That(ProductListingImage.Displayed, Is.True, "product image(s) not displayed correctly");


                    productImageUrl = productImageUrl[..^9];

                    Product product = new Product
                    {
                        Name = productName,
                        Price = productPrice,
                        ImageUrl = productImageUrl
                    };

                    products.Add(product);

                    Console.WriteLine();
                    Console.WriteLine($"Product Name: {product.Name}");
                    Console.WriteLine($"Product Price: {product.Price}");
                    Console.WriteLine($"Product Image URL: {product.ImageUrl}");
                }

                return products;
            }
            else
            {
                Console.WriteLine("No products found on the category page.");
                return null;
            }
        }
        
        public List<Subcategory>? GetAllSubcategories()
        {
            List<Subcategory> subcategories = new List<Subcategory>();

            if (SubcategoryItems.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Number of subcategories displayed for {PageTitle.Text}: {SubcategoryItems.Count}");

                foreach (var subcategoryContainer in SubcategoryItems)
                {
                    IWebElement SubcategoryName = subcategoryContainer.FindElement(By.CssSelector(".title a"));
                    IWebElement SubcategoryImage = subcategoryContainer.FindElement(By.CssSelector(".picture img"));

                    string subcategoryName = SubcategoryName.Text;
                    string subcategoryImageUrl = SubcategoryImage.GetAttribute("src");

                    Assert.That(SubcategoryImage.Displayed, Is.True, "Subcategory image(s) not displayed correctly");

                    Subcategory subcategory = new Subcategory
                    {
                        Name = subcategoryName,
                        ImageUrl = subcategoryImageUrl
                    };

                    subcategories.Add(subcategory);

                    Console.WriteLine();
                    Console.WriteLine($"Subcategory Name: {subcategory.Name}");
                    Console.WriteLine($"Subcategory Image URL: {subcategory.ImageUrl}");
                }

                return subcategories;
            }
            else
            {
                Console.WriteLine("No subcategories found on the category page.");
                return null;
            }
        }

        public Product? GetProductFromList(int index)
        {
            //int index = 3;
            if (ProductByIndex(index).Displayed)
            {
                string productName = ProductNameByIndex(index).Text;
                string productPrice = ProductPriceByIndex(index).Text;
                string productImage = ProductImageByIndex(index).GetAttribute("src");

                productImage = productImage[..^9];

                Product prod = new Product
                {
                    Name = productName,
                    Price = productPrice,
                    ImageUrl = productImage
                };

                Console.WriteLine();
                Console.WriteLine($"Product Id {index}:");
                Console.WriteLine($"Product Name: {prod.Name}");
                Console.WriteLine($"Product Price: {prod.Price}");
                Console.WriteLine($"Product Image URL: {prod.ImageUrl}");

                return prod;
            }
            else
            {
                Console.WriteLine($"Product with index {index} not found.");
                return null;
            }
        }

        public Product GetProductDetails(int index)
        {
            //int index = 3;

            string productName = ProductDetailsName(index).Text;
            string productPrice = ProductDetailsPrice(index).Text;
            string productImage = ProductDetailsImage(index).GetAttribute("src");
            string productQuantity = ProductDetailsQuantity(index).GetAttribute("value");
            string productSKU = ProductDetailsSKU(index).Text;

            productImage = productImage[..^9];


            Product productDetails = new Product
            {
                Name = productName,
                Price = productPrice,
                ImageUrl = productImage,
                Quantity = productQuantity,
                SKU = productSKU
            };

            Console.WriteLine();
            Console.WriteLine($"Product Details Page:");
            Console.WriteLine($"Product Name: {productDetails.Name}");
            Console.WriteLine($"Product Price: {productDetails.Price}");
            Console.WriteLine($"Product SKU: {productDetails.SKU}");
            Console.WriteLine($"Product Image URL: {productDetails.ImageUrl}");
            Console.WriteLine($"Product Quantity: {productDetails.Quantity}");

            return productDetails;
        }

        public void SelectProductDetailsByIndex(int index)
        {
            ProductNameByIndex(index).Click();
        }

        public void AddProductToCartByIndex(int index)
        {
            AddToCartBtnByIndex(index).Click();
        }

        public void AddProductToCartFromDetailsPage()
        {
            AddToCartBtnDetailsPage.Click();
        }

        public bool NotificationSuccessDisplayed()
        {
            return NotificationAddedToCartSuccess.Displayed;
        }

        public void CloseNotificationBar()
        {
            CloseNotificationBtn.Click();
        }

    }

    public class Product
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
        public string Quantity { get; set; }
        public string SKU { get; set; }
    }
    
    public class Subcategory
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ProductPageTitle
    {
        public const string Computers = "Computers";
        public const string Desktops = "Desktops";
        public const string Notebooks = "Notebooks";
        public const string Software = "Software";
        public const string Electronics = "Electronics";
        public const string CameraPhoto = "Camera & photo";
        public const string CellPhones = "Cell phones";
        public const string Others = "Others";
        public const string Apparel = "Apparel";
        public const string Shoes = "Shoes";
        public const string Clothing = "Clothing";
        public const string Accessories = "Accessories";
        public const string DigitalDownloads = "Digital downloads";
        public const string Books = "Books";
        public const string Jewelry = "Jewelry";
        public const string GiftCards = "Gift Cards";
    }
}

