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
        if (uri.IndexOf('?') > -1)
            uri = uri.Substring(0, uri.IndexOf('?'));

        var segments = uri.Trim().Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments;
    }

    public Dictionary<string, object?> ParseQueryParameters(string uri)
    {
        var queryParameters = new Dictionary<string, object?>();

        foreach (var queryParameterPair in uri.Substring(uri.IndexOf("?") + 1).Split('&', StringSplitOptions.RemoveEmptyEntries))
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
