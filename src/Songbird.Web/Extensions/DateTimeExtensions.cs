using System;
using System.Globalization;

namespace Songbird.Web.Extensions {
    public static class DateTimeExtensions {
        private class Iso8601Calendar : GregorianCalendar {
            private static Iso8601Calendar _instance;
            public static Iso8601Calendar Instance { get { return _instance ??= new Iso8601Calendar(); } }

            public override Int32 GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek) {
                var weekDay = GetDayOfWeek(time);
                if(weekDay >= DayOfWeek.Monday && weekDay <= DayOfWeek.Wednesday) {
                    time = time.AddDays(3);
                }

                return base.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
        }

        public static Int32 GetWeekNumber(this DateTime date) {
            // There might be cases to not use Iso8601 in the future
            var calendar = Iso8601Calendar.Instance;
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime StartOfWeek(this DateTime date, DayOfWeek? firstDayOfWeek = null) {
            firstDayOfWeek ??= CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            var diff = date.DayOfWeek - firstDayOfWeek.Value;
            if(diff < 0) {
                diff += 7;
            }

            return date.AddDays(-1 * diff).Date;
        }

        // While not strictly an extension, I'm not sure where to put this otherwise..
        public static DateTime GetStartOfWeek(Int32 year, Int32 weekNumber) {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var firstThursday = jan1.AddDays(daysOffset);

            var calendar = Iso8601Calendar.Instance;
            var firstWeek = calendar.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            if(firstWeek <= 1)
                weekNumber--;

            var result = firstThursday.AddDays(weekNumber * 7);
            var weekMonday = result.AddDays(-3); // thursday -> monday

            return weekMonday.StartOfWeek();
        }
    }
}

