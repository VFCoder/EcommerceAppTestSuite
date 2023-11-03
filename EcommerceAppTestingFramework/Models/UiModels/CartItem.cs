using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Models.UiModels
{
    public class CartItem
    {
        public string ProductSKU { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string TotalPrice { get; set; }
    }
}
