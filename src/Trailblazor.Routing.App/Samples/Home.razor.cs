﻿using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.App.Samples;

public partial class Home : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private INavigator Navigator { get; set; } = null!;

    private void NavigateToCounter()
    {
        Navigator.NavigateTo<Counter>(d => d.WithParameter(c => c.InitialCounter, 50));
    }

    private void NavigateToDirectiveComponent()
    {
        Navigator.NavigateTo<DirectiveComponent>(d => d
            .WithParameter(c => c.QueryParameter, DateTime.Now)
            .WithUri("/alternative-directive-route"));
    }
}
