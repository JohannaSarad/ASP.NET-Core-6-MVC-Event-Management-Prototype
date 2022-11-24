using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSarad_C868_Capstone.Data;


namespace JSarad_C868_Capstone_NUnitTests
{
    [TestFixture]
    public class ShedulingServiceNUnitTests
    {
        private SchedulingService schedulingService;

       [SetUp]
        public void Setup()
        {
            //Arrange test data for all schedulingService methods
            schedulingService = new SchedulingService();
        }

        [Test]
        //testing start time is at exactly open (6am) ane end time is at exactly close (11pm), expected result = true
        [TestCase("2022-11-10 06:00:00", "2022-11-10 23:00:00", ExpectedResult = true)]
        //testing start time 7am and end time 9pm, expected result = true
        [TestCase("2022-11-10 07:00:00", "2022-11-10 21:00:00", ExpectedResult = true)]
        //testing start time 5am (before open) and end time 9pm (before close), expected result = false
        [TestCase("2022-11-10 05:00:00", "2022-11-10 21:00:00", ExpectedResult = false)]
        //testing start time 7am (after open) and end time 11:30pm (after close), expected result = false
        [TestCase("2022-11-10 07:00:00", "2022-11-10 23:30:00", ExpectedResult = false)]
        //testing start time 5am (before open) and end time 11:30pm (after close), expected result = false
        [TestCase("2022-11-10 06:00:00", "2022-11-10 23:30:00", ExpectedResult = false)]
        public bool IsDuringBusinessHours_InputStartAndEndDates_ReturnTrueIfStartAndEndWithinBusinessHours(DateTime start, DateTime end)
        {
            //Assert All test cases return true or false
            return schedulingService.IsDuringBusinessHours(start, end);
        }
        

        [Test]
        //testing start time 7am is before end time 9pm, expected result true
        [TestCase("2022-11-10 07:00:00", "2022-11-10 21:00:00", ExpectedResult = true)]
        //testing start time 9pm isn't before end time 7am, expected result false
        [TestCase("2022-11-10 21:00:00", "2022-11-10 07:00:00", ExpectedResult = false)]
        public bool IsStartBeforeEnd_InputStartAndEndDates_ReturnTrueIfStartTimeIsBeforeEndTime(DateTime start, DateTime end)
        {
            //Assert All test cases return true or false
            return schedulingService.IsStartBeforeEnd(start, end);
        }

        [Test]
        //testing available schedule days Monday "M", Wednesday "W", Friday "F" is avaialable on Monday, expected result true 
        [TestCase("2022-11-21 12:00:00", "MWF", ExpectedResult = true)]
        //testing available schedule day Tuesday "T", is avaialable on Tuesday, expected result true 
        [TestCase("2022-11-22 12:00:00", "T", ExpectedResult = true)]
        //testing available schedule days Thursday "R", Saturday "S", Sunday "U" is available on Sunday, expected result true
        [TestCase("2022-11-27 12:00:00", "RSU", ExpectedResult = true)]
        //testing available schedule days All "MTWRFSU" is available Saturday, expected result true
        [TestCase("2022-11-26 12:00:00", "MTRFSU", ExpectedResult = true)]
        //testing available schedule days Monday "M", Wednesday "W", Friday "F" is avaialable on Thursday, expected result false 
        [TestCase("2022-11-24 12:00:00", "MWF", ExpectedResult = false)]
        //testing available schedule day Tuesday "T", is avaialable on Wednesday, expected result false 
        [TestCase("2022-11-23 12:00:00", "T", ExpectedResult = false)]
        //testing available schedule days Thursday "R", Saturday "S", Sunday "U" is available on Friday, expected result false
        [TestCase("2022-11-25 12:00:00", "RSU", ExpectedResult = false)]
        public bool IsAvailable_InputDateOfEventAndStringOfAvailableDays_ReturnsTrueOrFalse(DateTime dayOfWeek, string availability)
        {
            //Assert
            return schedulingService.IsAvailable(dayOfWeek, availability);
        }
    }
}
               

                
              

               

                
      
