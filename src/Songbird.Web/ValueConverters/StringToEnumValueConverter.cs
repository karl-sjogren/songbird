using System;
using FastEnumUtility;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Songbird.Web.ValueConverters;

public class StringToEnumValueConverter<T> : ValueConverter<T, string> where T : struct, Enum {
    public StringToEnumValueConverter() : base(
        value => value.ToString(),
        value => value == null ? default : FastEnum.Parse<T>(value), null) {
    }
}
