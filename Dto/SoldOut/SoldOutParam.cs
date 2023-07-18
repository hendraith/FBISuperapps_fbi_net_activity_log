namespace ActivityLog.Dto.SoldOut
{
    public class SoldOutParam
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Search { get; set; }
        public bool? IsGlobal { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
