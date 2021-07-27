using AutoMapper;
using Xunit;

namespace Songbird.Web.Tests {
    public class AutoMapperConfigurationTests {
        [Fact]
        public void AssertConfigurationIsValid() {
            var config = new MapperConfiguration(configuration => {
                var assembly = typeof(Startup).Assembly;
                configuration.AddMaps(assembly);
            });
            config.AssertConfigurationIsValid();
        }
    }
}
