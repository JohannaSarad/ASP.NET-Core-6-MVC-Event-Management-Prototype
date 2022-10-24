using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.ViewModels
{
    public class ClientListViewModel
    {
        public int SelectedId { get; set; }
        public string SelectedName { get; set; }
        public IEnumerable<Client>? ClientList { get; set; }
    }
}
