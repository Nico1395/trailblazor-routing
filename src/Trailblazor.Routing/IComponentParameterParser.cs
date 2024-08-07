namespace Trailblazor.Routing;

/// <summary>
/// Service parses query parameter values from strings into their respective type.
/// </summary>
internal interface IComponentParameterParser
{
    /// <summary>
    /// Method parses <paramref name="rawQueryParameters"/> for the query parameter properties for components of type <paramref name="componentType"/>.
    /// </summary>
    /// <param name="rawQueryParameters">Raw unparsed query parameters from the URI.</param>
    /// <param name="componentType">Type of component the <paramref name="rawQueryParameters"/> are to be parsed for.</param>
    /// <returns>Parsed component parameters.</returns>
    internal Dictionary<string, object?> ParseFromQueryParameters(Dictionary<string, string> rawQueryParameters, Type componentType);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal Dictionary<string, object?> ParseFromDirectiveQueryParameters(string relativeUri, Type componentType, string routeUri);
}
