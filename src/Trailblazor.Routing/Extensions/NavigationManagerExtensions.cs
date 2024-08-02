using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Extensions;

internal static class NavigationManagerExtensions
{
    internal static string GetRelativeUri(this NavigationManager navigationManager)
    {
        return navigationManager.ToBaseRelativePath(navigationManager.Uri);
    }
}
