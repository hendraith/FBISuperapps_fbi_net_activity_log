using ActivityLog.Model;

namespace ActivityLog.Business.SoldOut
{
    public interface ISoldOutBusiness
    {
        Task New(SoldOutModel data);
    }
}
