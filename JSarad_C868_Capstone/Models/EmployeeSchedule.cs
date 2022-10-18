using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace JSarad_C868_Capstone.Models
{
    public class EmployeeSchedule
    {
        public int? EmployeeId { get; set; }
        public string? Name { get; set; }
        [BindProperty, DataType(DataType.Time), DisplayFormat(DataFormatString = "{HH:mm tt}")]
        public DateTime StartTime{ get; set; }
        [BindProperty, DataType(DataType.Time), DisplayFormat(DataFormatString = "{HH:mm tt}")]
        public DateTime EndTime { get; set; }

    }
}
