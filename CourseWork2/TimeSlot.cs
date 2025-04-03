using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork2
{
    internal class TimeSlot
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public int ScheduleId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
