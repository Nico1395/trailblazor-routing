using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Descriptors;

/// <summary>
/// Base class for navigation descriptors.
/// </summary>
public record NavigationDescriptor
{
    /// <summary>
    /// Target URI to navigate to.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Uri)]
    public string? Uri { get; set; }

    /// <summary>
    /// Options further configuring how to navigate.
    /// </summary>
    public NavigationOptions Options { get; set; } = new NavigationOptions();

    /// <summary>
    /// Query parameters to be added to the <see cref="Uri"/>.
    /// </summary>
    public Dictionary<string, string> QueryParameters { get; } = [];
}
