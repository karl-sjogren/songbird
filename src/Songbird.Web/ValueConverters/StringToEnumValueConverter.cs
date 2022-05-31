using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Songbird.Web.ValueConverters;

public class StringToEnumValueConverter<T> : ValueConverter<T, string> where T : struct, IConvertible {
    public StringToEnumValueConverter() : base(
        value => value.ToString(),
        value => value == null ? default : Enum.Parse<T>(value), null) {
    }
}
