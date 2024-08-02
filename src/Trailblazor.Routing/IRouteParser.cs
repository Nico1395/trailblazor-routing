namespace Trailblazor.Routing;

public interface IRouteParser
{
    public string CombineSegments(string[] uriSegments);
    public string CombineSegments(string[] uriSegments, Dictionary<string, string> queryParameters);
    public string[] ParseSegments(string uri);
    public string RemoveQueryParameters(string uri);
    public Dictionary<string, object?> ParseQueryParameters(string uri);
    public bool RoutesMatch(string[] leftUriSegments, string[] rightUriSegments);
}
