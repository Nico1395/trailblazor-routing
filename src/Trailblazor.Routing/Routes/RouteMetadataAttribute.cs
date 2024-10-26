namespace Trailblazor.Routing;

/// <summary>
/// Attribute configures a components metadata.
/// </summary>
/// <remarks>
/// This attribute configures all routes (i.e. for every URI) associated with the component.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RouteMetadataAttribute : Attribute
{
    /// <summary>
    /// Constructor configures the metadata of all routes of a component.
    /// </summary>
    /// <param name="metadataKey">Key of the metadata.</param>
    /// <param name="metadataValue">Value of the metadata.</param>
    public RouteMetadataAttribute(string metadataKey, object? metadataValue)
    {
        MetadataKey = metadataKey;
        MetadataValue = metadataValue;
    }

    /// <summary>
    /// Key of the metadata.
    /// </summary>
    public string MetadataKey { get; }

    /// <summary>
    /// Value of the metadata.
    /// </summary>
    public object? MetadataValue { get; }
}
