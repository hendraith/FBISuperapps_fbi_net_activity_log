using ActivityLog.Dto.SoldOut;
using ActivityLog.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActivityLog.Repository.SoldOut
{
    public class SoldOutRepository : ISoldOutRepository
    {
        private readonly IMongoCollection<SoldOutModel> _collection;

        public SoldOutRepository(IMongoClient mongoClient)
        {
            MongoDatabaseSettings options = new MongoDatabaseSettings();
            options.ReadConcern = ReadConcern.Majority;

            var db = mongoClient.GetDatabase("activity_log", options);

            _collection = db.GetCollection<SoldOutModel>("sold_out", new MongoCollectionSettings());
        }

        public async Task<bool> NewAsync(SoldOutModel data)
        {
            data.ID = ObjectId.GenerateNewId();
            data.CreatedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(data);

            return true;
        }

        public async Task<List<SoldOutModel>> GetListAsync(SoldOutParam param)
        {
            var filter = GenerateFilter(param);

            var result = await _collection.Find(filter)
                .SortByDescending(q => q.CreatedAt)
                .Limit(param.Size)
                .Skip((param.Page - 1) * param.Size)
                .ToListAsync();

            return result;
        }

        public async Task<long> GetTotalDataAsync(SoldOutParam? param)
        {
            var filter = Builders<SoldOutModel>.Filter.Empty;

            if (param != null)
            {
                filter = GenerateFilter(param);
            }

            return await _collection.CountDocumentsAsync(filter);
        }

        private FilterDefinition<SoldOutModel> GenerateFilter(SoldOutParam param)
        {
            var filterSiteCode = param.Search == "" ? Builders<SoldOutModel>.Filter.Empty : Builders<SoldOutModel>.Filter.Eq(q => q.SiteCode, param.Search);
            var filterSku = param.Search == "" ? Builders<SoldOutModel>.Filter.Empty : Builders<SoldOutModel>.Filter.Eq(q => q.SKU, param.Search);
            var filterSearch = Builders<SoldOutModel>.Filter.Or(filterSiteCode, filterSku);

            var filterStartAt = Builders<SoldOutModel>.Filter.Gte(q => q.CreatedAt, param.StartDate);
            var filterEndAt = Builders<SoldOutModel>.Filter.Lte(q => q.CreatedAt, param.EndDate);
            var filterGlobal = param.IsGlobal == null ? Builders<SoldOutModel>.Filter.Empty : Builders<SoldOutModel>.Filter.Eq(q => q.IsGlobal, param.IsGlobal);

            return Builders<SoldOutModel>.Filter.And(filterSearch, filterStartAt, filterEndAt, filterGlobal);
        }
    }
}
