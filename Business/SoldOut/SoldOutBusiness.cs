using ActivityLog.Dto.SoldOut;
using ActivityLog.Model;
using ActivityLog.Repository.SoldOut;

namespace ActivityLog.Business.SoldOut
{
    public class SoldOutBusiness : ISoldOutBusiness
    {
        private readonly ISoldOutRepository _soldOutRepository;

        public SoldOutBusiness(ISoldOutRepository soldOutRepository)
        {
            _soldOutRepository = soldOutRepository;
        }

        public async Task New(SoldOutModel data)
        {
            if (string.IsNullOrEmpty(data.SiteCode))
            {
                throw new Exception("invalid parameter site code");
            };

            if (string.IsNullOrEmpty(data.SKU))
            {
                throw new Exception("invalid parameter sku");
            };

            if (string.IsNullOrEmpty(data.CreatedBy))
            {
                throw new Exception("invalid parameter created by");
            };

            await _soldOutRepository.NewAsync(data);
        }

        public async Task<SoldOutResponse> GetListAsync(SoldOutParam param)
        {
            var data = await _soldOutRepository.GetListAsync(param);
            var totalData = await _soldOutRepository.GetTotalDataAsync(null);
            var totalFilteredData = await _soldOutRepository.GetTotalDataAsync(param);

            return new SoldOutResponse
            {
                Data = data,
                TotalData = totalData,
                TotalFilteredData = totalFilteredData,
                CurrentPage = param.Page,
                LastPage = (int)Math.Ceiling((float)totalData / (float)param.Size)
            };
        }
    }
}
