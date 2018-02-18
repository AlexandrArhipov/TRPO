using System;
using System.Globalization;

namespace DateRegExp
{
    public class DateConverter
    {        
        private DateTime _dateTime;

        public DateConverter(string date)
        {
            _dateTime = DateTime.Parse(date);
        }

        public void PrintTask()
        {
            Console.WriteLine("Converting date: " + _dateTime);
            Console.WriteLine("Next day beginning: " + GetNextDay());
            Console.WriteLine("This week beginning: " + GetBeginningOfWeek());
            Console.WriteLine("This week ending: " + GetEndingOfWeek());
            Console.WriteLine("Next month beggining: " + GetBeginningOfNextMonth());
        }

        private DateTime GetNextDay()
        {
            return SetTimeOfDayToZero(_dateTime).AddDays(1);
        }

        private DateTime GetBeginningOfWeek()
        {
            // Прибавляем 1, т.к. DayOfWeek.Sunday = 0, а в контексте задачи, неделя начинается с понедельника
            // По той же причине отдельно обрабатываем день Воскресенье
            return _dateTime.DayOfWeek == DayOfWeek.Sunday 
                ? SetTimeOfDayToZero(_dateTime).AddDays(-6)
                : SetTimeOfDayToZero(_dateTime).AddDays(-(int)_dateTime.DayOfWeek + 1);
        }

        private DateTime GetEndingOfWeek()
        {
            // Прибавляем 1, т.к. DayOfWeek.Sunday = 0, а в контексте задачи, неделя начинается с понедельника
            // По той же причине отдельно обрабатываем день Воскресенье
            return _dateTime.DayOfWeek == DayOfWeek.Sunday
            ? SetTimeOfDayToZero(_dateTime).AddDays(1).AddMilliseconds(-1)
            : SetTimeOfDayToZero(_dateTime).AddDays(7 - (int)_dateTime.DayOfWeek + 1).AddMilliseconds(-1);
        }

        private DateTime GetBeginningOfNextMonth()
        {
            return SetTimeOfDayToZero(_dateTime).AddDays(-_dateTime.Day + 1).AddMonths(1);
        }

        private static DateTime SetTimeOfDayToZero(DateTime dateTime)
        {
            return dateTime.AddHours(-dateTime.Hour).AddMinutes(-dateTime.Minute)
                .AddSeconds(-dateTime.Second).AddMilliseconds(-dateTime.Millisecond);
        } 
    }
}