﻿using EcommerceAppTestingFramework.Drivers;
using EcommerceAppTestingFramework.Models.UiModels;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.CheckoutPage;
using static EcommerceAppTestingFramework.Pages.UserDataAndOrderVerifier;

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
        private IReadOnlyCollection<IWebElement> SubcategoryItems => _driver.FindElements(By.CssSelector(".sub-category-item"));
        private IWebElement SubcategoryName => _driver.FindElementWait(By.CssSelector(".sub-category-item .title a"));
        private IWebElement SubcategoryImage => _driver.FindElementWait(By.CssSelector(".sub-category-item .picture img"));

        //product listing page:
        private IWebElement ProductListingNameGlobal => _driver.FindElementWait(By.CssSelector(".product-title a"));
        private IWebElement ProductListingPriceGlobal => _driver.FindElementWait(By.CssSelector(".price.actual-price"));
        private IWebElement ProductListingImageGlobal => _driver.FindElementWait(By.CssSelector(".picture img"));
        private IWebElement ProductById(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}']"));
        private IWebElement ProductNameById(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .product-title a"));
        private IWebElement ProductPriceById(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .price.actual-price"));
        private IWebElement ProductImageById(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .picture img"));
        private IWebElement AddToCartBtnById(int id) => _driver.FindElementWait(By.CssSelector($"div[data-productid='{id}'] .product-box-add-to-cart-button"));
        private IWebElement ProductByIndex(int index) => _driver.FindElementWait(By.CssSelector($".item-box:nth-of-type({index})"));
        private IWebElement ProductNameByIndex(int index) => _driver.FindElementWait(By.CssSelector($".item-box:nth-of-type({index}) .product-title a"));
        private IWebElement ProductPriceByIndex(int index) => _driver.FindElementWait(By.CssSelector($".item-box:nth-of-type({index}) .price.actual-price"));
        private IWebElement ProductImageByIndex(int index) => _driver.FindElementWait(By.CssSelector($".item-box:nth-of-type({index}) .picture img"));
        private IWebElement AddToCartBtnByIndex(int index) => _driver.FindElementWait(By.CssSelector($".item-box:nth-of-type({index})  .product-box-add-to-cart-button"));

        //product details page selectors:
        private IWebElement ProductDetailsName => _driver.FindElementWait(By.CssSelector(".product-name h1"));
        private IWebElement ProductDetailsPrice => _driver.FindElementWait(By.CssSelector(".product-price span"));
        private IWebElement ProductDetailsImage => _driver.FindElementWait(By.CssSelector(".product-essential .picture img"));
        private IWebElement ProductDetailsQuantity => _driver.FindElementWait(By.CssSelector(".add-to-cart-panel .qty-input"));
        private IWebElement ProductDetailsSKU => _driver.FindElementWait(By.CssSelector(".product-essential .sku .value"));
        private IWebElement ProductDetailsManufacturer => _driver.FindElementWait(By.CssSelector(".manufacturers a"));
        private IWebElement AddToCartBtnDetailsPage => _driver.FindElementWait(By.CssSelector(".add-to-cart-button"));
        private IWebElement CategoriesBarProductDetails => _driver.FindElementWait(By.CssSelector("ul[itemscope]"));

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

        public string? GetProductNameByIndex(int index)
        {
            try
            {
                return ProductNameByIndex(index).Text;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public int GetProductCount()
        {
            return ProductItems.Count;
        }

        public string GetManufacturer()
        {
            return ProductDetailsManufacturer.Text;
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

        public List<Product>? VerifySearchResults(string search)
        {
            List<Product> products = new List<Product>();

            if (ProductItems.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Number of search results displayed for {search}: {ProductItems.Count}");

                foreach (var productContainer in ProductItems)
                {
                    IWebElement ProductListingName = productContainer.FindElement(By.CssSelector(".product-title a"));
                    IWebElement ProductListingPrice = productContainer.FindElement(By.CssSelector(".price.actual-price"));
                    IWebElement ProductListingImage = productContainer.FindElement(By.CssSelector(".picture img"));

                    string productName = ProductListingName.Text;
                    string productPrice = ProductListingPrice.Text;
                    string productImageUrl = ProductListingImage.GetAttribute("src");

                    Assert.That(ProductListingImage.Displayed, Is.True, "Product image(s) not displayed correctly");
                    Assert.That(productName.ToLower().Contains(search.ToLower()), Is.True, $"Search results {productName} do not match search text {search}");


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
                Console.WriteLine($"No search results found for '{search}'.");
                return null;
            }
        }

        public List<Product>? VerifyAdvancedSearchResults(string search, string? category = null, string? manufacturer = null)
        {
            List<Product> products = new List<Product>();

            var productItems = _driver.FindElements(By.CssSelector(".product-item"));

            if (productItems.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Number of search results displayed for {search}: {productItems.Count}");

                for (int i = 0; i < productItems.Count; i++)
                {
                    var productContainer = productItems[i];

                    try
                    {
                        IWebElement ProductListingName = productContainer.FindElement(By.CssSelector(".product-title a"));
                        IWebElement ProductListingPrice = productContainer.FindElement(By.CssSelector(".price.actual-price"));
                        IWebElement ProductListingImage = productContainer.FindElement(By.CssSelector(".picture img"));

                        string productName = ProductListingName.Text;
                        string productPrice = ProductListingPrice.Text;
                        string productImageUrl = ProductListingImage.GetAttribute("src");

                        Assert.That(ProductListingImage.Displayed, Is.True, "Product image(s) not displayed correctly");
                        Assert.That(productName.ToLower().Contains(search.ToLower()), Is.True, $"Search results {productName} do not match search text {search}");

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

                        ProductListingName.Click();

                        if (category != null)
                        {
                            Assert.That(CategoriesBarProductDetails.Text.Contains(category), Is.True, "Product category does not match");
                        }
                        if (manufacturer != null)
                        {
                            Assert.That(GetManufacturer() == manufacturer, Is.True, "Product manufacturer does not match");
                        }

                        _driver.Navigate().Back();
                        productItems = _driver.FindElements(By.CssSelector(".product-item"));
                    }
                    catch (StaleElementReferenceException)
                    {
                        Console.WriteLine($"Stale element exception occurred for product item at index {i}. Retrying...");
                        productItems = _driver.FindElements(By.CssSelector(".product-item"));
                        i--;
                    }
                }

                return products;
            }
            else
            {
                Console.WriteLine($"No search results found for '{search}'.");
                return null;
            }
        }


        public Product? GetProductFromListById(int id)
        {
            //int index = 3;
            if (ProductById(id).Displayed)
            {
                string productName = ProductNameById(id).Text;
                string productPrice = ProductPriceById(id).Text;
                string productImage = ProductImageById(id).GetAttribute("src");

                productImage = productImage[..^9];

                Product prod = new Product
                {
                    Name = productName,
                    Price = productPrice,
                    ImageUrl = productImage
                };

                Console.WriteLine();
                Console.WriteLine($"Product Id {id}:");
                Console.WriteLine($"Product Name: {prod.Name}");
                Console.WriteLine($"Product Price: {prod.Price}");
                Console.WriteLine($"Product Image URL: {prod.ImageUrl}");

                return prod;
            }
            else
            {
                Console.WriteLine($"Product with index {id} not found.");
                return null;
            }
        }

        public Product? GetProductFromListByIndex(int index)
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

        public Product GetProductDetails()
        {
            //int index = 3;

            string productName = ProductDetailsName.Text;
            string productPrice = ProductDetailsPrice.Text;
            string productImage = ProductDetailsImage.GetAttribute("src");
            string productQuantity = ProductDetailsQuantity.GetAttribute("value");
            string productSKU = ProductDetailsSKU.Text;

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
            try
            {
                ProductNameByIndex(index).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SelectProductDetailsById(int id)
        {
            try
            {
                ProductNameById(id).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddProductToCartById(int id)
        {
            AddToCartBtnById(id).Click();
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

        public string GetCategoriesText()
        {
            return CategoriesBarProductDetails.Text;
        }

    }
}

