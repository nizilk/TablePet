using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablePet.Services.Models;
using MySql.Data.MySqlClient;

namespace TablePet.Services.Controllers
{
    public class CalendarService
    {
        private Dictionary<DateTime, List<CalendarEvent>> events;
        private string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

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
            List<string> eventDescriptions = new List<string>();

            // 获取指定日期的事件，按时间排序
            string query = "SELECT start_time, description FROM CalendarEvents WHERE DATE(start_time) = @Date ORDER BY start_time";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", date.Date);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime startTime = reader.GetDateTime("start_time");
                            string description = reader.GetString("description");
                            eventDescriptions.Add($"{startTime.ToShortTimeString()} - {description}");
                        }
                    }
                }
            }
            
            if (eventDescriptions.Count == 0)
            {
                eventDescriptions.Add("当前日期没有事件。");
            }

            return eventDescriptions;
        }


        //添加事件
        public void AddEvent(CalendarEvent calendarEvent)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO CalendarEvents (start_time, description) VALUES (@StartTime, @Description)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StartTime", calendarEvent.startTime);
                    cmd.Parameters.AddWithValue("@Description", calendarEvent.description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string CheckTodaysEvents()
        {
            string todayEvent = GetEventForToday();

            return string.IsNullOrEmpty(todayEvent) ? string.Empty : todayEvent;
        }

        private string GetEventForToday()
        {
            DateTime now = DateTime.Now;
            string eventDescription = string.Empty;

            // 获取今天的所有事件
            var eventsForToday = GetEventListForToday();

            // 遍历事件并检查是否有匹配的事件
            foreach (var calendarEvent in eventsForToday)
            {
                // 比较事件的开始时间与当前时间，确保精确到分钟
                if (calendarEvent.startTime <= now && calendarEvent.startTime.AddMinutes(1) > now)
                {
                    // 如果找到与当前时间匹配的事件，返回事件内容
                    eventDescription = $"现在的时间是{calendarEvent.startTime:HH:mm}, 该做{calendarEvent.description}啦";
                    break;
                }
            }
            
            return eventDescription;
        }

        private List<CalendarEvent> GetEventListForToday()
        {
            List<CalendarEvent> eventList = new List<CalendarEvent>();

            string query = "SELECT * FROM CalendarEvents WHERE DATE(start_time) = CURDATE()";  // 获取今天的所有事件
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eventList.Add(new CalendarEvent
                            {
                                startTime = reader.GetDateTime("start_time"),
                                description = reader.GetString("description")
                            });
                        }
                    }
                }
            }

            return eventList;
        }

    }
}
