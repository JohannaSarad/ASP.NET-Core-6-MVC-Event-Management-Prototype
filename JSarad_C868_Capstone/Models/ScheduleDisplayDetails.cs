namespace JSarad_C868_Capstone.Models
{
    public class ScheduleDisplayDetails
    {
        public int? ScheduleId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        //public Schedule schedule { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
