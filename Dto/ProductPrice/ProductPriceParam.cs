namespace ActivityLog.Dto.ProductPrice
{
    public class ProductPriceParam
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SiteCode { get; set; }
        public string Sku { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
