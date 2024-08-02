using Microsoft.AspNetCore.WebUtilities;

namespace Trailblazor.Routing;

internal sealed class RouteParser : IRouteParser
{
    public string CombineSegments(string[] uriSegments)
    {
        return string.Join('/', uriSegments);
    }

    public string CombineSegments(string[] uriSegments, Dictionary<string, string> queryParameters)
    {
        var uri = CombineSegments(uriSegments);
        foreach (var queryParameter in queryParameters)
            uri = QueryHelpers.AddQueryString(uri, queryParameter.Key, queryParameter.Value);

        return uri;
    }

    public string[] ParseSegments(string uri)
    {
        return RemoveQueryParameters(uri).Split('/', StringSplitOptions.RemoveEmptyEntries);
    }

    public string RemoveQueryParameters(string uri)
    {
        if (!uri.Contains('?'))
            return uri;

        return uri.Substring(0, uri.IndexOf('?'));
    }

    public Dictionary<string, object?> ParseQueryParameters(string uri)
    {
        var queryParametersString = uri.Substring(uri.IndexOf("?") + 1);
        var queryParameterPairs = queryParametersString.Split('&', StringSplitOptions.RemoveEmptyEntries);
        var queryParameters = new Dictionary<string, object?>();

        foreach (var queryParameterPair in queryParameterPairs)
        {
            if (queryParameterPair != string.Empty && queryParameterPair.Contains('='))
            {
                var pair = queryParameterPair.Split('=');
                queryParameters.Add(pair[0], pair[1]);
            }
        }

        return queryParameters;
    }

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
