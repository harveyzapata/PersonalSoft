
using PruebaHarveyNet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace PruebaHarveyNet.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<Poliza> _polizaCollection;

        public MongoDBService()
        {
  
            // Constructor sin parámetros para Moq
        }

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _polizaCollection = database.GetCollection<Poliza>(mongoDBSettings.Value.CollectionName);
        }

        public virtual async Task CreateAsync(Poliza poliza)
        {
            await _polizaCollection.InsertOneAsync(poliza);
            return;
        }

        public virtual async Task<List<Poliza>> GetAsync()
        {
            return await _polizaCollection.Find(new BsonDocument()).ToListAsync();
        }

        public virtual Poliza BuscarPolizaPorPlacaYNumero(string placaVehiculo, string numeroPoliza)
        {
            return _polizaCollection.Find(p =>
                p.LicensePlate == placaVehiculo || p.NumberPoliza == numeroPoliza)
                .FirstOrDefault();
        }

        public async Task DeleteAsync(string id)
        {
            FilterDefinition<Poliza> filter = Builders<Poliza>.Filter.Eq("Id", id);
            await _polizaCollection.DeleteOneAsync(filter);
            return;
        }
        
    }
}
