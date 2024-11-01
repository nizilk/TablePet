using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablePet.Services.Models;

namespace TablePet.Services.Controllers
{
    public class CalendarService
    {
        private Dictionary<DateTime, List<CalendarEvent>> events;

        public CalendarService()
        {
            events = new Dictionary<DateTime, List<CalendarEvent>>();
        }

        public string GetMonthYearLabel(DateTime currentMonth)
        {
            return currentMonth.ToString("MMMM yyyy");
        }

        public int GetDaysInMonth(DateTime currentMonth)
        {
            return DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
        }

        //获取当月开始的日期
        public int GetStartDayOfMonth(DateTime currentMonth)
        {
            DateTime firstOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            return (int)firstOfMonth.DayOfWeek;
        }

        //根据日期获取事件
        public List<string> GetEventsForDate(DateTime date)
        {
            if (events.TryGetValue(date, out var dateEvents))
            {
                List<string> eventDescriptions = new List<string>();
                foreach (var calendarEvent in dateEvents)
                {
                    eventDescriptions.Add($"{calendarEvent.StartTime.ToShortTimeString()} - {calendarEvent.Title}");
                }
                return eventDescriptions;
            }
            else
            {
                return new List<string> { "当前日期没有事件。" };
            }
        }

        //添加事件
        public void AddEvent(DateTime date, CalendarEvent calendarEvent)
        {
            if (!events.ContainsKey(date))
            {
                events[date] = new List<CalendarEvent>();
            }
            events[date].Add(calendarEvent);
        }
    }
}
