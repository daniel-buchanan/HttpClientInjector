using FluentAssertions;
using HttpClientInjector.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HttpClientInjector.Tests;

public partial class ConfigurationTests
{
    private readonly HttpClientConfigurationBuilder _builder;
    
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

    public static TheoryData<Func<IHttpClientConfigurationBuilder, IHttpClientConfigurationBuilder>, string> AcceptEncodingTests =>
        new()
        {
            {b => b.WithCompression(), "br"}
        };

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
    
    public static TheoryData<Func<IHttpClientConfigurationBuilder, IHttpClientConfigurationBuilder>, string> AcceptTests =>
        new()
        {
            { b => b.WithJsonEncoding(), "application/json" },
            { b => b.WithXmlEncoding(), "text/xml" }
        };

    [Theory]
    [MemberData(nameof(CustomHeaderTests))]
    public void CustomHeaderSetCorrectly(Func<IHttpClientConfigurationBuilder, IHttpClientConfigurationBuilder> method, string key, string value)
    {
        // Arrange
        var client = new HttpClient();
        
        // Act
        method(_builder);
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders.Should().ContainKey(key);
        client.DefaultRequestHeaders.GetValues(key).Should().Contain(value);
    }
    
    public static TheoryData<Func<IHttpClientConfigurationBuilder, IHttpClientConfigurationBuilder>, string, string> CustomHeaderTests()
    {
        const string key1 = "Foo";
        const string value1 = "Bar";
        
        const string key2 = "Fizz";
        const string value2 = "Buzz";
        
        return new()
        {
            { b => b.WithCustomHeader(key1, value1), key1, value1 },
            { b => b.WithCustomHeader(key2, value2), key2, value2 },
            { b => b.WithCustomHeader(new KeyValuePair<string, string>(key1, value1)), key1, value1 },
            { b => b.WithCustomHeader(new KeyValuePair<string, string>(key2, value2)), key2, value2 },
            { b => b.WithCustomHeader(key1, _ => value1), key1, value1 },
            { b => b.WithCustomHeader(key2, _ => value2), key2, value2 },
            { b => b.WithCustomHeader(_ => new KeyValuePair<string, string>(key1, value1)), key1, value1 },
            { b => b.WithCustomHeader(_ => new KeyValuePair<string, string>(key2, value2)), key2, value2 }
        };
    }
}