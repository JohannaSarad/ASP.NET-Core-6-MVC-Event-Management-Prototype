using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.ViewModels
{
    public class EventListViewModel
    {
        //public int EventId { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public string Type { get; set; }
        //public int ClientId { get; set; }
        //public string ContactName { get; set; }
        //public string Phone { get; set; }
        public int SelectedId { get; set; }
        public string SelectedName { get; set; }
        public IEnumerable<EventListDetails>? EventList { get; set; }
    }
}
