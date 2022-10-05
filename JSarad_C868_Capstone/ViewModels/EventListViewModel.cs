using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EventListViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
    }
}
