
using System.Collections.Generic;
using System.Threading.Tasks;
using PruebaHarveyNet.Models;

namespace PruebaHarveyNet.Services
{
    public interface IMongoDBService
    {
        Task CreateAsync(Poliza poliza);
        Task<List<Poliza>> GetAsync();
        Poliza BuscarPolizaPorPlacaYNumero(string placaVehiculo, string numeroPoliza);
        Task DeleteAsync(string id);
       
    }
}


