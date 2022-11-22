namespace JSarad_C868_Capstone.Data
{
    public class SchedulingService
    {
        //validates event dates and Employee schedule dates start times are before end times
        public bool IsStartBeforeEnd(DateTime start, DateTime end)
        {
            if ((start > end) || (start == end))
            {
                return false;
            }
            return true;
        }

        //validates event and employee schedules are within business hours
        public bool IsDuringBusinessHours(DateTime start, DateTime end)
        {
            TimeSpan open = new TimeSpan(06, 00, 00);
            TimeSpan close = new TimeSpan(23, 00, 00);

            if ((end.TimeOfDay > close) || (start.TimeOfDay < open) || (end.TimeOfDay < open) || (start.TimeOfDay > close))
            {
                return false;
            }
            return true;
        }

        //validate employee availability for event
        public bool IsAvailable(DateTime start, string availability)
        {
            if ((start.DayOfWeek == DayOfWeek.Monday && !availability.Contains("M"))
            || (start.DayOfWeek == DayOfWeek.Tuesday && !availability.Contains("T"))
            || (start.DayOfWeek == DayOfWeek.Wednesday && !availability.Contains("W"))
            || (start.DayOfWeek == DayOfWeek.Thursday && !availability.Contains("R"))
            || (start.DayOfWeek == DayOfWeek.Friday && !availability.Contains("F"))
            || (start.DayOfWeek == DayOfWeek.Saturday && !availability.Contains("S"))
            || (start.DayOfWeek == DayOfWeek.Sunday && !availability.Contains("U")))
            {
                return false;
            }
            return true;
        }
    }
}
