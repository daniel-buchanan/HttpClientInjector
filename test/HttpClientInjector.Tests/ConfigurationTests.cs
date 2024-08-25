using FluentAssertions;
using HttpClientInjector.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HttpClientInjector.Tests;

public partial class ConfigurationTests
{
    private readonly IHttpClientConfigurationBuilder _builder;
    
    public ConfigurationTests()
    {
        IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
        _builder = new HttpClientConfigurationBuilder(provider);
    }

    [Theory]
    [MemberData(nameof(AcceptEncodingTests))]
    public void AcceptEncodingSetCorrectly(Func<IHttpClientConfigurationBuilder, IHttpClientConfigurationBuilder> method, string expected)
    {
        // Arrange
        var client = new HttpClient();
        
        // Act
        method(_builder);
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders
            .AcceptEncoding
            .Should()
            .Satisfy(h => h.Value == expected);
    }

    public static IEnumerable<object[]> AcceptEncodingTests
    {
        get
        {
            yield return [(IHttpClientConfigurationBuilder b) => b.WithCompression(), "br"];
        }
    }
    
    [Theory]
    [MemberData(nameof(AcceptTests))]
    public void AcceptSetCorrectly(Func<IHttpClientConfigurationBuilder, IHttpClientConfigurationBuilder> method, string expected)
    {
        // Arrange
        var client = new HttpClient();
        
        // Act
        method(_builder);
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders
            .Accept
            .Should()
            .Satisfy(h => h.MediaType == expected);
    }
    
    public static IEnumerable<object[]> AcceptTests
    {
        get
        {
            yield return [(IHttpClientConfigurationBuilder b) => b.WithJsonEncoding(), "application/json"];
            yield return [(IHttpClientConfigurationBuilder b) => b.WithXmlEncoding(), "text/xml"];
        }
    }
}