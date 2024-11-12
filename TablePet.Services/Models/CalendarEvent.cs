using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePet.Services.Models
{
    public class CalendarEvent
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime startTime { get; set; }

        public override string ToString()
        {
            // 控制在 ListBox 中显示的格式
            return $"{startTime.ToShortTimeString()} - {description}";
        }

    }
}
