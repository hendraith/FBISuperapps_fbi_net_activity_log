namespace ActivityLog.Dto.ProductPrice
{
    public class ProductPriceParam
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Search { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
