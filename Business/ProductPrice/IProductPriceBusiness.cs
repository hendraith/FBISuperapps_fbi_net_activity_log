using ActivityLog.Dto.ProductPrice;
using ActivityLog.Dto.SoldOut;
using ActivityLog.Model;

namespace ActivityLog.Business.ProductPrice
{
    public interface IProductPriceBusiness
    {
        Task New(ProductPriceModel data);
        Task<ProductPriceResponse> GetListAsync(ProductPriceParam param);
    }
}
