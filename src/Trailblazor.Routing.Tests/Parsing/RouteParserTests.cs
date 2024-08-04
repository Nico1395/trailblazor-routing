using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing;

namespace Trailblazor.Routing.Tests.Parsing;

public class RouteParserTests
{
    private IRouteParser RouteParser => ParsingDependencyInjection.ServiceProvider.GetRequiredService<IRouteParser>();

    [Fact]
    public void RouteParser_RoutesMatch_Succeeds()
    {
        var leftSegments = new string[] { "these", "segments", "match" };
        var rightSegments = new string[] { "these", "segments", "match" };

        Assert.True(RouteParser.RoutesMatch(leftSegments, rightSegments));
    }

    [Fact]
    public void RouteParser_RoutesMatch_Fails()
    {
        var leftSegments = new string[] { "these", "segments", "match" };
        var rightSegments = new string[] { "these", "segments", "dont", "match" };

        Assert.False(RouteParser.RoutesMatch(leftSegments, rightSegments));
    }

    [Fact]
    public void RouteParser_RoutesMatch_DifferentOrder()
    {
        var leftSegments = new string[] { "these", "segments", "match" };
        var rightSegments = new string[] { "segments", "match", "these" };

        Assert.False(RouteParser.RoutesMatch(leftSegments, rightSegments));
    }

    [Fact]
    public void RouteParser_CombineSegments()
    {
        var uriSegments = new[]
        {
            "this",
            "is",
            "a",
            "test-uri",
        };

        var uri = RouteParser.CombineSegments(uriSegments);
        Assert.Equal("this/is/a/test-uri", uri);
    }

    [Fact]
    public void RouteParser_CombineSegmentsWithQueryParameters()
    {
        var uriSegments = new[]
        {
            "this",
            "is",
            "a",
            "test-uri",
        };
        var queryParameters = new Dictionary<string, string>()
        {
            { "coolParameter", 44.ToString() },
            { "test", "hello-world" },
        };

        var uri = RouteParser.CombineSegments(uriSegments, queryParameters);
        Assert.Equal("this/is/a/test-uri?coolParameter=44&test=hello-world", uri);
    }

    [Fact]
    public void RouteParser_ParseSegments_Uri()
    {
        var uri = "this/is/a/test-uri";
        var uriSegments = RouteParser.ParseSegments(uri);

        var targetUriSegments = new[]
        {
            "this",
            "is",
            "a",
            "test-uri",
        };
        Assert.True(RouteParser.RoutesMatch(targetUriSegments, uriSegments));
    }

    [Fact]
    public void RouteParser_ParseSegments_UriWithQueryParameters()
    {
        var uri = "this/is/a/test-uri?coolParameter=44&test=hello-world";
        var uriSegments = RouteParser.ParseSegments(uri);

        var targetUriSegments = new[]
        {
            "this",
            "is",
            "a",
            "test-uri",
        };
        Assert.True(RouteParser.RoutesMatch(targetUriSegments, uriSegments));
    }

    [Fact]
    public void RouteParser_RemoveQueryParameters()
    {
        var uri = "this/is/a/test-uri?coolParameter=44&test=hello-world";
        var uriWithoutQueryParameters = RouteParser.RemoveQueryParameters(uri);

        Assert.Equal("this/is/a/test-uri", uriWithoutQueryParameters);
    }

    [Fact]
    private void RouteParser_ExtractQueryParameters()
    {
        var uri = "this/is/a/test-uri?coolParameter=44&test=hello-world";
        var queryParameters = RouteParser.ExtractQueryParameters(uri);

        var targetQueryParameters = new Dictionary<string, object?>()
        {
            { "coolParameter", "44" },
            { "test", "hello-world" },
        };

        Assert.Equal(targetQueryParameters.Count, queryParameters.Count);

        foreach (var kvp in targetQueryParameters)
        {
            Assert.True(queryParameters.ContainsKey(kvp.Key), $"The key '{kvp.Key}' was not found in the query parameters.");
            Assert.Equal(kvp.Value, queryParameters[kvp.Key]);
        }
    }
}
