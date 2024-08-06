using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.App.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private INavigator Navigator { get; set; } = null!;

    private void NavigateToCounter()
    {
        Navigator.NavigateTo<Counter>(d => d.AddParameter(c => c.InitialCounter, 50));
    }
}
