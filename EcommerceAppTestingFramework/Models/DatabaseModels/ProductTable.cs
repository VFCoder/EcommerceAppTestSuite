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
    public class ProductTable
    {
        public const string tableName = "Product";
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Sku { get; set; }
        public int ProductTypeId { get; set; }
        public int ParentGroupedProductId { get; set; }
        public Bitmap VisibleIndividually { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public int ProductTemplateId { get; set; }
        public int VendorId { get; set; }
        public Bitmap ShowOnHomepage { get; set; }
        public Bitmap AllowCustomerReviews { get; set; }
        public int ApprovedRatingSum { get; set; }
        public int NotApprovedRatingSum { get; set; }
        public int ApprovedTotalReviews { get; set; }
        public int NotApprovedTotalReviews { get; set; }
        public Bitmap SubjectToAcl { get; set; }
        public Bitmap LimitedToStores { get; set; }
        public Bitmap IsGiftCard { get; set; }
        public int GiftCardTypeId { get; set; }
        public Bitmap RequireOtherProducts { get; set; }
        public Bitmap AutomaticallyAddRequiredProducts { get; set; }
        public Bitmap IsDownload { get; set; }
        public int DownloadId { get; set; }
        public Bitmap UnlimitedDownloads { get; set; }
        public int MaxNumberOfDownloads { get; set; }
        public int DownloadActivationTypeId { get; set; }
        public Bitmap HasSampleDownload { get; set; }
        public int SampleDownloadId { get; set; }
        public Bitmap HasUserAgreement { get; set; }
        public Bitmap IsRecurring { get; set; }
        public int RecurringCycleLength { get; set; }
        public int RecurringCyclePeriodId { get; set; }
        public int RecurringTotalCycles { get; set; }
        public Bitmap IsRental { get; set; }
        public int RentalPriceLength { get; set; }
        public int RentalPricePeriodId { get; set; }
        public Bitmap IsShipEnabled { get; set; }
        public Bitmap IsFreeShipping { get; set; }
        public Bitmap ShipSeparately { get; set; }
        public decimal AdditionalShippingCharge { get; set; }
        public int DeliveryDateId { get; set; }
        public Bitmap IsTaxExempt { get; set; }
        public int TaxCategoryId { get; set; }
        public Bitmap IsTelecommunicationsOrBroadcastingOrElectronicServices { get; set; }
        public int ManageInventoryMethodId { get; set; }
        public int ProductAvailabilityRangeId { get; set; }
        public Bitmap UseMultipleWarehouses { get; set; }
        public int WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public Bitmap DisplayStockAvailability { get; set; }
        public Bitmap DisplayStockQuantity { get; set; }
        public int MinStockQuantity { get; set; }
        public int LowStockActivityId { get; set; }
        public int NotifyAdminForQuantityBelow { get; set; }
        public int BackorderModeId { get; set; }
        public int AllowBackInStockSubscriptions { get; set; }
        public int OrderMinimumQuantity { get; set; }
        public int OrderMaximumQuantity { get; set; }
        public Bitmap AllowAddingOnlyExistingAttributeCombinations { get; set; }
        public Bitmap NotReturnable { get; set; }
        public Bitmap DisableBuyButton { get; set; }
        public Bitmap DisableWishlistButton { get; set; }
        public Bitmap AvailableForPreOrder { get; set; }
        public Bitmap CallForPrice { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public decimal ProductCost { get; set; }
        public Bitmap CustomerEntersPrice { get; set; }
        public decimal MinimumCustomerEnteredPrice { get; set; }
        public decimal MaximumCustomerEnteredPrice { get; set; }
        public int BasepriceEnabled { get; set; }
        public decimal BasepriceAmount { get; set; }
        public int BasepriceUnitId { get; set; }
        public decimal BasepriceBaseAmount { get; set; }
        public int BasepriceBaseUnitId { get; set; }
        public Bitmap MarkAsNew { get; set; }
        public Bitmap HasTierPrices { get; set; }
        public Bitmap HasDiscountsApplied { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public int DisplayOrder { get; set; }
        public int Published { get; set; }
        public int Deleted { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }

    }
}
