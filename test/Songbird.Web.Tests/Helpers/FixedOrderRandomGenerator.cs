using Songbird.Web.Contracts;

namespace Songbird.Web.Tests.Helpers;

internal class FixedOrderRandomGenerator : IRandomNumberGenerator {
    private readonly IEnumerator<double> _generator;

    public FixedOrderRandomGenerator(IEnumerable<double> generator) {
        _generator = generator.GetEnumerator();
    }

    public int Next(int maxValue) {
        if(!_generator.MoveNext())
            throw new InvalidOperationException("FixedOrderRandomGenerator was called more times then the generator allowed.");

        var result = (Int32)Math.Ceiling((double)_generator.Current * maxValue) - 1;
        return (Int32)Math.Max(result, 0.0);
    }
}

public class FixedOrderRandomGeneratorTests {
    [Fact]
    public void ShouldGenerateSoundValues() {
        var input = new[] { 0d, 1d, 0d, 1d, 0.5d, 0.3333333d, 0.9d };
        var generator = new FixedOrderRandomGenerator(input);

        for(var i = 1; i <= input.Length; i++) {
            generator.Next(i).ShouldBeInRange(0, i - 1);
        }
    }
}
