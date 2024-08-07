using Microsoft.Extensions.DependencyInjection;

namespace Trailblazor.Routing.Tests.Parsing;

public class UriParserTests
{
    private IUriParser UriParser => ParsingDependencyInjection.ServiceProvider.GetRequiredService<IUriParser>();

    [Fact]
    public void UriParser_RoutesMatch_Succeeds()
    {
        var leftSegments = new string[] { "these", "segments", "match" };
        var rightSegments = new string[] { "these", "segments", "match" };

        Assert.True(UriParser.RoutesMatch(leftSegments, rightSegments));
    }

    [Fact]
    public void UriParser_RoutesMatch_Fails()
    {
        var leftSegments = new string[] { "these", "segments", "match" };
        var rightSegments = new string[] { "these", "segments", "dont", "match" };

        Assert.False(UriParser.RoutesMatch(leftSegments, rightSegments));
    }

    [Fact]
    public void UriParser_RoutesMatch_DifferentOrder()
    {
        var leftSegments = new string[] { "these", "segments", "match" };
        var rightSegments = new string[] { "segments", "match", "these" };

        Assert.False(UriParser.RoutesMatch(leftSegments, rightSegments));
    }

    [Fact]
    public void UriParser_CombineSegments()
    {
        var uriSegments = new[]
        {
            "this",
            "is",
            "a",
            "test-uri",
        };

        var uri = UriParser.CombineSegments(uriSegments);
        Assert.Equal("this/is/a/test-uri", uri);
    }

    [Fact]
    public void UriParser_CombineSegmentsWithQueryParameters()
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

        var uri = UriParser.CombineSegments(uriSegments, queryParameters);
        Assert.Equal("this/is/a/test-uri?coolParameter=44&test=hello-world", uri);
    }

    [Fact]
    public void UriParser_ParseSegments_Uri()
    {
        var uri = "this/is/a/test-uri";
        var uriSegments = UriParser.ParseSegments(uri);

        var targetUriSegments = new[]
        {
            "this",
            "is",
            "a",
            "test-uri",
        };
        Assert.True(UriParser.RoutesMatch(targetUriSegments, uriSegments));
    }

    [Fact]
    public void UriParser_ParseSegments_UriWithQueryParameters()
    {
        var uri = "this/is/a/test-uri?coolParameter=44&test=hello-world";
        var uriSegments = UriParser.ParseSegments(uri);

        var targetUriSegments = new[]
        {
            "this",
            "is",
            "a",
            "test-uri",
        };
        Assert.True(UriParser.RoutesMatch(targetUriSegments, uriSegments));
    }

    [Fact]
    public void UriParser_RemoveQueryParameters()
    {
        var uri = "this/is/a/test-uri?coolParameter=44&test=hello-world";
        var uriWithoutQueryParameters = UriParser.RemoveQueryParameters(uri);

        Assert.Equal("this/is/a/test-uri", uriWithoutQueryParameters);
    }

    [Fact]
    private void UriParser_ExtractQueryParameters()
    {
        var uri = "this/is/a/test-uri?coolParameter=44&test=hello-world";
        var queryParameters = UriParser.ExtractQueryParameters(uri);

        var targetQueryParameters = new Dictionary<string, object?>()
        {
            { "coolParameter", 44.ToString() },
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
