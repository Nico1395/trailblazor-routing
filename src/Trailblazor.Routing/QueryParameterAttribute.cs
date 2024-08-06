using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing;

/// <summary>
/// Attribute flags a property to be linked to a query parameter from a URI.
/// </summary>
/// <remarks>
/// In order for component query parameters to receive their value a <see cref="ParameterAttribute"/> has to be
/// placed above the property as well.
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class QueryParameterAttribute : Attribute
{
    /// <summary>
    /// Instantiates an attribute. Since no <see cref="Name"/> is specified, the name of the property
    /// will be used as a query parameter name.
    /// </summary>
    public QueryParameterAttribute()
    {
    }

    /// <summary>
    /// Instantiates an attribute.
    /// </summary>
    /// <param name="name">Name of the query parameter from the URI linked to the property decorated by the attribute.</param>
    public QueryParameterAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Name of the query parameter of the URI.
    /// </summary>
    /// <remarks>
    /// This name is optional and can be used to link a property to a given query parameter of a URI. If not specified, the name of the
    /// property decorated by the attribute will be used. The name is always case-insesitive.
    /// </remarks>
    public string? Name { get; set; }
}
