namespace JSarad_C868_Capstone.Models
{
    public class EventListDetails
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Type { get; set; }
        public int ClientId { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
    }
}
