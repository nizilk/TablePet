using System;
using System.Collections.Generic;
using System.Linq;

namespace TablePet.Services.Models
{
    public class Alarm
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public bool Status { get; set; }
        public string RepeatMode { get; set; }  // "仅一次", "每天", "自定义"
        public List<DayOfWeek> CustomDays { get; set; }  // 对于"自定义"重复模式，存储选择的星期几
        public string Description { get; set; }

        public Alarm()
        {
            CustomDays = new List<DayOfWeek>();  // 防止 CustomDays 为 null
        }

        public void SetDescription()
        {
            // 更新 Description 字段
            switch (RepeatMode)
            {
                case "仅一次":
                    Description = "仅一次";
                    break;
                case "每天":
                    Description = "每天";
                    break;
                case "自定义":
                    Description = string.Join(" ", CustomDays.Select(day => GetDayName(day))); // 如果是自定义，显示选择的星期几
                    break;
                default:
                    Description = "未定义";
                    break;
            }
        }

        private string GetDayName(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return "周一";
                case DayOfWeek.Tuesday: return "周二";
                case DayOfWeek.Wednesday: return "周三";
                case DayOfWeek.Thursday: return "周四";
                case DayOfWeek.Friday: return "周五";
                case DayOfWeek.Saturday: return "周六";
                case DayOfWeek.Sunday: return "周日";
                default: return string.Empty;
            }
        }
    }
}