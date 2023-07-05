using ActivityLog.Dto.SoldOut;
using ActivityLog.Model;

namespace ActivityLog.Repository.SoldOut
{
    public interface ISoldOutRepository
    {
        Task<bool> NewAsync(SoldOutModel data);
        Task<List<SoldOutModel>> GetListAsync(SoldOutParam param);
        Task<long> GetTotalDataAsync(SoldOutParam? param);
    }
}
