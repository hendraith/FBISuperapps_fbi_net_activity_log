using ActivityLog.Dto.ProductPrice;
using ActivityLog.Dto.SoldOut;
using ActivityLog.Model;

namespace ActivityLog.Repository.ProductPrice
{
    public interface IProductPriceRepository
    {
        Task<bool> NewAsync(ProductPriceModel data);
        Task<List<ProductPriceModel>> GetListAsync(ProductPriceParam param);
        Task<long> GetTotalDataAsync(ProductPriceParam? param);
    }
}
