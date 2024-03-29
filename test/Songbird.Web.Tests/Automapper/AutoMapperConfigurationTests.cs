using AutoMapper;

namespace Songbird.Web.Tests.Automapper;

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
