using ActivityLog.Dto.ProductPrice;
using ActivityLog.Dto.SoldOut;
using ActivityLog.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActivityLog.Repository.ProductPrice
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly IMongoCollection<ProductPriceModel> _collection;

        public ProductPriceRepository(IMongoClient mongoClient)
        {
            MongoDatabaseSettings options = new MongoDatabaseSettings();
            options.ReadConcern = ReadConcern.Majority;

            var db = mongoClient.GetDatabase("activity_log", options);

            _collection = db.GetCollection<ProductPriceModel>("product_price", new MongoCollectionSettings());
        }

        public async Task<bool> NewAsync(ProductPriceModel data)
        {
            data.ID = ObjectId.GenerateNewId();
            data.CreatedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(data);

            return true;
        }

        public async Task<List<ProductPriceModel>> GetListAsync(ProductPriceParam param)
        {
            var filter = GenerateFilter(param);

            var result = await _collection.Find(filter)
                .Limit(param.Size)
                .Skip((param.Page - 1) * param.Size)
                .ToListAsync();

            return result;
        }

        public async Task<long> GetTotalDataAsync(ProductPriceParam? param)
        {
            var filter = Builders<ProductPriceModel>.Filter.Empty;

            if (param != null)
            {
                filter = GenerateFilter(param);
            }

            return await _collection.CountDocumentsAsync(filter);
        }

        private FilterDefinition<ProductPriceModel> GenerateFilter(ProductPriceParam param)
        {
            var filterSiteCode = param.SiteCode == "" ? Builders<ProductPriceModel>.Filter.Empty : Builders<ProductPriceModel>.Filter.Eq(q => q.SiteCode, param.SiteCode);
            var filterSku = param.Sku == "" ? Builders<ProductPriceModel>.Filter.Empty : Builders<ProductPriceModel>.Filter.Eq(q => q.SKU, param.Sku);
            var filterStartAt = Builders<ProductPriceModel>.Filter.Gte(q => q.CreatedAt, param.StartDate);
            var filterEndAt = Builders<ProductPriceModel>.Filter.Lte(q => q.CreatedAt, param.EndDate);

            return Builders<ProductPriceModel>.Filter.And(filterSiteCode, filterSku, filterStartAt, filterEndAt);
        }
    }
}
