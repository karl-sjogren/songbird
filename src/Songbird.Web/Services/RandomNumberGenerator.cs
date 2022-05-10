using System;
using Songbird.Web.Contracts;

namespace Songbird.Web.Services;

public class RandomNumberGenerator : IRandomNumberGenerator {
    private readonly Random _random = new();
    private readonly object _lock = new();

    public Int32 Next(Int32 maxValue) {
        lock(_lock) {
            return _random.Next(maxValue);
        }
    }
}
