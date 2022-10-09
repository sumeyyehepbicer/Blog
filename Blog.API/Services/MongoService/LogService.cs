using Blog.API.Models;
using Blog.Shared.MongoModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blog.API.Services.MongoService
{
    public class LogService
    {
        private readonly IMongoCollection<LogModel> _logCollection;

        public LogService(IOptions<MongoDatabaseConnection> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.DefaultConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _logCollection = mongoDatabase.GetCollection<LogModel>(
                bookStoreDatabaseSettings.Value.LogCollectionName);
        }

        public async Task CreateAsync(LogModel log) =>
        await _logCollection.InsertOneAsync(log);
    }
}
