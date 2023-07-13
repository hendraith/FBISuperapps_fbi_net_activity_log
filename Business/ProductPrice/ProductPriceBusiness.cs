using ActivityLog.Dto.ProductPrice;
using ActivityLog.Model;
using ActivityLog.Repository.ProductPrice;

namespace ActivityLog.Business.ProductPrice
{
    public class ProductPriceBusiness : IProductPriceBusiness
    {
        private readonly IProductPriceRepository _productPriceRepository;

        public ProductPriceBusiness(IProductPriceRepository productPriceRepository)
        {
            _productPriceRepository = productPriceRepository;
        }

        public async Task New(ProductPriceModel data)
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

            if (data.DistributionChannel == 0)
            {
                throw new Exception("invalid parameter distribution channel");
            };

            await _productPriceRepository.NewAsync(data);
        }

        public async Task<ProductPriceResponse> GetListAsync(ProductPriceParam param)
        {
            var data = await _productPriceRepository.GetListAsync(param);
            var totalData = await _productPriceRepository.GetTotalDataAsync(null);
            var totalFilteredData = await _productPriceRepository.GetTotalDataAsync(param);

            return new ProductPriceResponse
            {
                Data = data,
                TotalData = totalData,
                TotalFilteredData = totalFilteredData,
                CurrentPage = param.Page,
                LastPage = (int)Math.Ceiling((float)totalFilteredData / (float)param.Size)
            };
        }
    }
}
