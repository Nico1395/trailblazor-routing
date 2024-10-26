using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.App.Samples.AttributeSamples;

[Route("attribute-sample-parent")]
[RouteChildren(typeof(AttributeSample))]
public partial class AttributeSampleParent : ComponentBase
{
    [Inject]
    private INavigator Navigator { get; set; } = null!;

    [CascadingParameter]
    public required RouterContext RouterContext { get; set; }

    private string? GetTitle()
    {
        return RouterContext.Route?.GetMetadataValue<string?>("title");
    }

    private void NavigateToChild()
    {
        Navigator.NavigateTo<AttributeSample>();
    }
}
