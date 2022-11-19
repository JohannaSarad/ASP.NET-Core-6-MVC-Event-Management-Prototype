using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.Data.Interfaces
{
    public interface IClientRepository : IDisposable
    {
        IEnumerable<Client> GetAll();
        Client GetById(int id);
        void Add(Client client);
        Client Update(int id, Client client);
        void Delete(int id);
    }
}
        
       
