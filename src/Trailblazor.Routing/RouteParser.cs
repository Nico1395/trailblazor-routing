using Microsoft.AspNetCore.WebUtilities;

namespace Trailblazor.Routing;

/// <summary>
/// Service provides parsing functionalities for routes.
/// </summary>
internal sealed class RouteParser : IRouteParser
{
    /// <summary>
    /// Method combines <paramref name="uriSegments"/> to a URI string.
    /// </summary>
    /// <param name="uriSegments">Segments to be combined.</param>
    /// <returns>URI string.</returns>
    public string CombineSegments(string[] uriSegments)
    {
        return string.Join('/', uriSegments);
    }

    /// <summary>
    /// Method combines <paramref name="uriSegments"/> and <paramref name="queryParameters"/> to a URI string.
    /// </summary>
    /// <param name="uriSegments">Segments to be combined.</param>
    /// <param name="queryParameters">Query parameters to of the URI.</param>
    /// <returns>URI string.</returns>
    public string CombineSegments(string[] uriSegments, Dictionary<string, string> queryParameters)
    {
        var uri = CombineSegments(uriSegments);
        return AddQueryParameters(uri, queryParameters);
    }

    /// <summary>
    /// Method adds <paramref name="queryParameters"/> to the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI the <paramref name="queryParameters"/> are to be added to.</param>
    /// <param name="queryParameters">Query paramaters to be added.</param>
    /// <returns>URI with <paramref name="queryParameters"/> added to it.</returns>
    public string AddQueryParameters(string uri, Dictionary<string, string> queryParameters)
    {
        foreach (var queryParameter in queryParameters)
            uri = QueryHelpers.AddQueryString(uri, queryParameter.Key, queryParameter.Value);

        return uri;
    }

    /// <summary>
    /// Method parses a <paramref name="uri"/> into its segments.
    /// </summary>
    /// <param name="uri">URI to be parsed.</param>
    /// <returns>Segments of the specified <paramref name="uri"/>.</returns>
    public string[] ParseSegments(string uri)
    {
        return RemoveQueryParameters(uri).Split('/', StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Method removes the query parameters of the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI the query parameters are to be removed from.</param>
    /// <returns>URI without query parameters.</returns>
    public string RemoveQueryParameters(string uri)
    {
        if (!uri.Contains('?'))
            return uri;

        return uri.Substring(0, uri.IndexOf('?'));
    }

    /// <summary>
    /// Method parses the query parameters of the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI whose query parameters are to be parsed.</param>
    /// <returns>Query parameters of the URI.</returns>
    public Dictionary<string, string> ExtractQueryParameters(string uri)
    {
        var queryParametersString = uri.Substring(uri.IndexOf("?") + 1);
        var queryParameterPairs = queryParametersString.Split('&', StringSplitOptions.RemoveEmptyEntries);
        var queryParameters = new Dictionary<string, string>();

        foreach (var queryParameterPair in queryParameterPairs)
        {
            if (queryParameterPair != string.Empty && queryParameterPair.Contains('='))
            {
                var pair = queryParameterPair.Split('=');
                if (pair.Length == 2)
                    queryParameters.Add(pair[0], pair[1]);
            }
        }

        return queryParameters;
    }

    /// <summary>
    /// Method determines whether two specified collections of URI segments match each other.
    /// </summary>
    /// <param name="leftUriSegments">Left URI segments.</param>
    /// <param name="rightUriSegments">Right URI segments.</param>
    /// <returns><see langword="true"/> if the segments match.</returns>
    public bool RoutesMatch(string[] leftUriSegments, string[] rightUriSegments)
    {
        if (leftUriSegments.Length != rightUriSegments.Length)
            return false;

        for (var i = 0; i < rightUriSegments.Length; i++)
        {
            if (string.Compare(leftUriSegments[i], rightUriSegments[i], StringComparison.OrdinalIgnoreCase) != 0)
                return false;
        }

        return true;
    }
}
