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
    public class AddressTable
    {
        public const string tableName = "Address"; 
        public int Id { get; set; }
        public int? CountryId { get; set; }
        public int? StateProvinceId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? City { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? ZipPostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedOnUtc { get; set; }


    }
}
