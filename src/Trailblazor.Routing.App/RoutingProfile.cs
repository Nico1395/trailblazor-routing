using Trailblazor.Routing.App.Pages;
using Trailblazor.Routing.Profiles;

namespace Trailblazor.Routing.App;

internal sealed class RoutingProfile : RoutingProfileBase
{
    protected sealed override void Configure(RoutingProfileConfiguration configuration)
    {
        configuration.AddRoute<Home>(r => r.WithUri("/"));
        configuration.AddRoute<Counter>(r => r.WithUri("/counter"));
        configuration.AddRoute<Weather>(r => r.WithUri("/weather"));
    }
}
