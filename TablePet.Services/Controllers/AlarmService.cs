using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using TablePet.Services.Models;

namespace TablePet.Services.Controllers
{
    public class AlarmService
    {
        private string connectionString;

        public AlarmService(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        // Helper method: Convert DayOfWeek list to comma-separated string
        private string CustomDaysToString(List<DayOfWeek> days)
        {
            return days != null ? string.Join(",", days.Select(d => d.ToString())) : null;
        }

        // Helper method: Convert comma-separated string to DayOfWeek list
        private List<DayOfWeek> StringToCustomDays(string days)
        {
            if (string.IsNullOrEmpty(days))
                return new List<DayOfWeek>();

            return days.Split(',')
                       .Select(day => Enum.TryParse(day, out DayOfWeek result) ? result : (DayOfWeek?)null)
                       .Where(d => d.HasValue)
                       .Select(d => d.Value)
                       .ToList();
        }

        // 1. 获取所有闹钟
        public List<Alarm> GetAllAlarms()
        {
            var alarms = new List<Alarm>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Alarms";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var alarm = new Alarm
                            {
                                Id = reader.GetInt32("id"),
                                Time = reader.GetTimeSpan("time"),
                                Status = reader.GetBoolean("status"),
                                RepeatMode = reader.GetString("repeat_mode"),
                                CustomDays = reader["custom_days"] != DBNull.Value 
                                    ? StringToCustomDays(reader.GetString("custom_days")) 
                                    : new List<DayOfWeek>()
                            };
                            alarms.Add(alarm);
                        }
                    }
                }
            }
            return alarms;
        }

        // 2. 添加新闹钟
        public void AddAlarm(Alarm alarm)
        {
            var customDaysStr = CustomDaysToString(alarm.CustomDays);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Alarms (time, status, repeat_mode, custom_days) VALUES (@time, @is_active, @repeat_mode, @custom_days)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@time", alarm.Time);
                    cmd.Parameters.AddWithValue("@is_active", alarm.Status);
                    cmd.Parameters.AddWithValue("@repeat_mode", alarm.RepeatMode);
                    cmd.Parameters.AddWithValue("@custom_days", customDaysStr);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 3. 更新现有闹钟
        public void UpdateAlarm(Alarm alarm)
        {
            var customDaysStr = CustomDaysToString(alarm.CustomDays);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "UPDATE Alarms SET time = @time, status = @is_active, repeat_mode = @repeat_mode, custom_days = @custom_days WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", alarm.Id);
                    cmd.Parameters.AddWithValue("@time", alarm.Time);
                    cmd.Parameters.AddWithValue("@is_active", alarm.Status);
                    cmd.Parameters.AddWithValue("@repeat_mode", alarm.RepeatMode);
                    cmd.Parameters.AddWithValue("@custom_days", customDaysStr);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 4. 删除指定闹钟
        public void DeleteAlarm(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "DELETE FROM Alarms WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 5. 启用/禁用闹钟
        public void ToggleAlarmStatus(int id, bool isActive)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "UPDATE Alarms SET status = @is_active WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@is_active", isActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}