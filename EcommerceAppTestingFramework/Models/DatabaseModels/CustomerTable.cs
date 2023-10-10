using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EcommerceAppTestingFramework.Models.DatabaseModels
{
    public class CustomerTable
    {
        public const string tableName = "Customer";
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int? BillDeletedingAddress_Id { get; set; }
        public int? ShippingAddress_Id { get; set; }
        public UniqueId CustomerGuid { get; set; }
        public string? AdminComment { get; set; }    
        public Bitmap IsTaxExempt { get; set; }
        public int AffiliateId { get; set; }
        public int VendorId { get; set; }
        public Bitmap HasShoppingCartItems { get; set; }
        public Bitmap RequireReLogin { get; set; }
        public Bitmap FailedLoginAttempts { get; set; }
        public Bitmap Active { get; set; }
        public Bitmap Deleted { get; set; }
        public Bitmap IsSystemAccount { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime LastActivityDate { get; set; }
        public int RegisteredInStoreId { get; set; }

    }
}
