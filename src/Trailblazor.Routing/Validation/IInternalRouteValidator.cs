using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Validation;

internal interface IInternalRouteValidator
{
    internal void ValidateRoutes(List<Route> routes);
}
