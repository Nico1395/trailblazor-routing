using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Descriptors;

/// <summary>
/// Base class for navigation descriptors.
/// </summary>
public abstract record NavigationDescriptorBase
{
    internal NavigationDescriptorBase() { }

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

    /// <summary>
    /// Determines whether the route is registered using a '<c>@page</c>' directive.
    /// </summary>
    public bool RouteRegisteredUsingPageDirective { get; set; }

    /// <summary>
    /// Method adds a query parameter to the descriptors <see cref="Uri"/>.
    /// </summary>
    /// <param name="queryParameterName">Name of the query parameter.</param>
    /// <param name="queryParameterValue">Value of the query parameter.</param>
    /// <returns>Navigation descriptor for further configurations.</returns>
    public NavigationDescriptorBase AddParameter(string queryParameterName, [DisallowNull] object queryParameterValue)
    {
        ArgumentNullException.ThrowIfNull(queryParameterValue, nameof(queryParameterValue));

        QueryParameters.Add(queryParameterName, queryParameterValue.ToString()!);
        return this;
    }

    public NavigationDescriptorBase AddDirectiveParameter(uint order, [DisallowNull] object queryParameterValue)
    {
        ArgumentNullException.ThrowIfNull(queryParameterValue, nameof(queryParameterValue));

        QueryParameters.Add(order.ToString(), queryParameterValue.ToString()!);
        return this;
    }
}
