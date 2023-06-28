using ActivityLog.Model;

namespace ActivityLog.Repository.SoldOut
{
    public interface ISoldOutRepository
    {
        Task<bool> New(SoldOutModel data);
    }
}
