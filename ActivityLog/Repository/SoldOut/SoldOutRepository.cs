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

        public async Task<bool> New(SoldOutModel data)
        {
            data.ID = ObjectId.GenerateNewId();
            data.CreatedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(data);

            return true;
        }
    }
}
