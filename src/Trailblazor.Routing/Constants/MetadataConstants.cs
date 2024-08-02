using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Constants;

/// <summary>
/// Class contains metadata related constants. These constants represent keys for common metadata values.
/// </summary>
public class MetadataConstants
{
    /// <summary>
    /// Key of the metadata value that indicates that the route has been added to the router by the '@page' directive or <see cref="RouteAttribute"/>.
    /// </summary>
    public const string FromPageDirective = "from-page-directive";
}
