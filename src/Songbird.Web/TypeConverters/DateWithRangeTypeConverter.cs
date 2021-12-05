using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Songbird.Web.Queries;

namespace Songbird.Web.TypeConverters {
    public class DateWithRangeTypeConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            var casted = value as string;
            return casted != null
                ? ParseString(casted)
                : base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            var casted = value as DateWithRange;
            return destinationType == typeof(string) && casted != null
                ? SerializeToString(casted)
                : base.ConvertTo(context, culture, value, destinationType);
        }

        public string SerializeToString(DateWithRange dateWithRange) {
#pragma warning disable RCS1146
            if(dateWithRange == null || dateWithRange.IsEmpty) {
                return null;
            }
#pragma warning restore RCS1146

            if(dateWithRange.IsSingleDate) {
                return dateWithRange.MinDate.ToString("yyyy-MM-dd");
            }

            return dateWithRange.MinDate.ToString("yyyy-MM-dd") + " - " + dateWithRange.MaxDate.ToString("yyyy-MM-dd");
        }

        public DateWithRange ParseString(string value) {
            if(string.IsNullOrWhiteSpace(value)) {
                return null;
            }

            var dateWithRange = ParseSingleDate(value) ?? ParseDateRange(value);

            return dateWithRange;
        }

        private static DateWithRange ParseSingleDate(string value) {
            if(DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date)) {
                return new DateWithRange {
                    MinDate = date,
                    MaxDate = date.AddHours(23).AddMinutes(59).AddSeconds(59)
                };
            }

            return null;
        }

        private static DateWithRange ParseDateRange(string value) {
            if(!value.Contains(" - ")) {
                return null;
            }

            var parts = value.Split(" - ", StringSplitOptions.RemoveEmptyEntries).Select(part => part?.Trim()).ToArray();
            if(parts.Length != 2) {
                return null;
            }

            var dateWithRange = new DateWithRange();

            if(DateTime.TryParseExact(parts[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date)) {
                dateWithRange.MinDate = date;
            } else {
                return null;
            }

            if(DateTime.TryParseExact(parts[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out date)) {
                dateWithRange.MaxDate = date.AddHours(23).AddMinutes(59).AddSeconds(59);
            } else {
                return null;
            }

            return dateWithRange;
        }
    }
}
