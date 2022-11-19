using JSarad_C868_Capstone.Data.Interfaces;
using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.Data.Repositories
{
    public class ClientRepository : IClientRepository

    {
        private readonly AppDbContext _db;

        public ClientRepository(AppDbContext db)
        {
            _db = db;

        }
        public void Add(Client client)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAll()
        {
            throw new NotImplementedException();
        }

        public Client GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Client Update(int id, Client client)
        {
            throw new NotImplementedException();
        }
    }
}
