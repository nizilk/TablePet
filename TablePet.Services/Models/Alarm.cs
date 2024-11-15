using System;
using System.Collections.Generic;

namespace TablePet.Services.Models
{
    public class Alarm
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public bool Status { get; set; }
        public string RepeatMode { get; set; }  // "一次", "每天", "自定义"
        public List<DayOfWeek> CustomDays { get; set; }  // 对于"自定义"重复模式，存储选择的星期几
    }
}