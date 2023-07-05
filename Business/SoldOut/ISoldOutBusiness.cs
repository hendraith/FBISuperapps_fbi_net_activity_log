using ActivityLog.Dto.SoldOut;
using ActivityLog.Model;

namespace ActivityLog.Business.SoldOut
{
    public interface ISoldOutBusiness
    {
        Task New(SoldOutModel data);
        Task<SoldOutResponse> GetListAsync(SoldOutParam param);
    }
}
