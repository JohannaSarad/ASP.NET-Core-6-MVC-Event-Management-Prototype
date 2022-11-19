
using JSarad_C868_Capstone.Models;
namespace JSarad_C868_Capstone.Data
{
    public class DataService
    {
        public static User currentUser { get; set; }
        
        private readonly AppDbContext _db;

        public DataService(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Client> GetClients()
        {
            IEnumerable<Client> clients = _db.Clients;
            return clients;
        }

        public Client GetClientById(int id)
        {
            var client = _db.Clients.Find(id);
            return client;
        }
    }
}
