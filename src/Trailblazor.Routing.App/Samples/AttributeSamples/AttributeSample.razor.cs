using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.App.Samples.AttributeSamples;

[Route("attribute-sample")]
[RouteParent(typeof(AttributeSampleParent))]
[RouteChildren(typeof(AttributeSampleChild))]
[RouteMetadata("title", "Attribute Sample")]
public partial class AttributeSample : ComponentBase
{
    [Inject]
    private INavigator Navigator { get; set; } = null!;

    [CascadingParameter]
    public required RouterContext RouterContext { get; set; }

    private string? GetTitle()
    {
        return RouterContext.Route?.GetMetadataValue<string?>("title");
    }

    private void NavigateToParent()
    {
        Navigator.NavigateTo<AttributeSampleParent>();
    }

    private void NavigateToChild()
    {
        Navigator.NavigateTo<AttributeSampleChild>();
    }
}
