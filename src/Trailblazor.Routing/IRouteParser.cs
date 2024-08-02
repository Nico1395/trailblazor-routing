namespace Trailblazor.Routing;

/// <summary>
/// Service provides parsing functionalities for routes.
/// </summary>
public interface IRouteParser
{
    /// <summary>
    /// Method combines <paramref name="uriSegments"/> to a URI string.
    /// </summary>
    /// <param name="uriSegments">Segments to be combined.</param>
    /// <returns>URI string.</returns>
    public string CombineSegments(string[] uriSegments);

    /// <summary>
    /// Method combines <paramref name="uriSegments"/> and <paramref name="queryParameters"/> to a URI string.
    /// </summary>
    /// <param name="uriSegments">Segments to be combined.</param>
    /// <returns>URI string.</returns>
    public string CombineSegments(string[] uriSegments, Dictionary<string, string> queryParameters);

    /// <summary>
    /// Method parses a <paramref name="uri"/> into its segments.
    /// </summary>
    /// <param name="uri">URI to be parsed.</param>
    /// <returns>Segments of the specified <paramref name="uri"/>.</returns>
    public string[] ParseSegments(string uri);

    /// <summary>
    /// Method removes the query parameters of the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI the query parameters are to be removed from.</param>
    /// <returns>URI without query parameters.</returns>
    public string RemoveQueryParameters(string uri);

    /// <summary>
    /// Method parses the query parameters of the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI whose query parameters are to be parsed.</param>
    /// <returns>Query parameters of the URI.</returns>
    public Dictionary<string, object?> ParseQueryParameters(string uri);

    /// <summary>
    /// Method determines whether two specified collections of URI segments match each other.
    /// </summary>
    /// <param name="leftUriSegments">Left URI segments.</param>
    /// <param name="rightUriSegments">Right URI segments.</param>
    /// <returns><see langword="true"/> if the segments match.</returns>
    public bool RoutesMatch(string[] leftUriSegments, string[] rightUriSegments);
}
