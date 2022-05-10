using System;

namespace Songbird.Web.Contracts;

public interface IRandomNumberGenerator {
    Int32 Next(Int32 maxValue);
}
