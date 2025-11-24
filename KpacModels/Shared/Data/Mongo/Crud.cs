// using KpacModels.Shared.Config;
// using MongoDB.Bson;
// using MongoDB.Driver;
//
// namespace KpacModels.Shared.Data.Mongo;
//
// public class Crud<T>
// {
//     protected readonly IMongoCollection<T> Collection;
//
//     protected Crud(
//         MongoOptions options)
//     {
//         var mongoClient = new MongoClient(
//             options.ConnectionUrl);
//
//         var mongoDatabase = mongoClient.GetDatabase(
//             options.DatabaseName);
//         
//         Collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
//     }
//     
//     public async Task<List<T>> GetAllAsync() =>
//         await Collection.Find(_ => true).ToListAsync();
//     
//     
//     public async Task<T?> GetByIdAsync(string id)
//     {
//         var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
//         return await Collection.Find(filter).FirstOrDefaultAsync();
//     }
//     
//     public async Task CreateAsync(T newObject) =>
//         await Collection.InsertOneAsync(newObject);
//
//
//     public async Task<ReplaceOneResult> ReplaceAsync(string id, T document)
//     {
//         var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
//         return await Collection.ReplaceOneAsync(filter, document);
//     }
//     
//     public async Task<DeleteResult> RemoveAsync(string id)
//     {
//         var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
//         return await Collection.DeleteOneAsync(filter);
//     }
//     
//     public async Task InsertManyByListAsync(List<T> newList) =>
//         await Collection.InsertManyAsync(newList);
// }