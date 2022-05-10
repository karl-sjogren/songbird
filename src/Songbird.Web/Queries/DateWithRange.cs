using System;
using System.ComponentModel;
using Songbird.Web.TypeConverters;

namespace Songbird.Web.Queries;

[TypeConverter(typeof(DateWithRangeTypeConverter))]
public class DateWithRange {
    public DateTime MinDate { get; set; }

    private DateTime _maxDate;

    public DateTime MaxDate {
        get {
            return _maxDate;
        }
        set {
            if(value == DateTime.MinValue) {
                _maxDate = value;
            } else {
                _maxDate = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59, DateTimeKind.Local);
            }
        }
    }

    public bool IsEmpty => MinDate == DateTime.MinValue && MaxDate == DateTime.MinValue;
    public bool IsSingleDate => MinDate != DateTime.MinValue && MinDate.Date == MaxDate.Date;
}
