using FluentAssertions;
using HttpClientInjector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HttpClientInjector.Tests;

public class FullUpTests
{
    private readonly IServiceCollection _services;
    
    public FullUpTests()
    {
        _services = new ServiceCollection();
        _services.AddHttpClient();
    }

    [Fact]
    public void ConfiguredClientReturned()
    {
        // Arrange
        _services.InjectHttpClientFor<MockService>(b 
            => b.WithBaseUrl("http://test.com").WithoutAuthentication());
        var provider = _services.BuildServiceProvider();

        // Act
        var client = provider.GetHttpClientFor<MockService>();

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void ClientInjectedSuccessfully()
    {
        // Arrange
        _services.InjectHttpClientFor<MockService>(b => b.WithoutAuthentication());
        _services.AddScoped<MockService>();
        var provider = _services.BuildServiceProvider();

        // Act
        var service = provider.GetService<MockService>();

        // Assert
        service.Should().NotBeNull();
        service?.Client.Should().NotBeNull();
    }

    [Fact]
    public void BasicAuthenticatedClientInjectedSuccessfully()
    {
        const string username = "bob@internet.com";
        const string password = "password";
        var computed = BasicAuthentication.EncodeHeader(username, password);
        
        // Arrange
        _services.InjectHttpClientFor<MockService>(b 
            => b.WithBasicAuthentication(username, password));
        _services.AddScoped<MockService>();
        var provider = _services.BuildServiceProvider();
        
        // Act
        var service = provider.GetRequiredService<MockService>();
        
        // Assert
        AssertHeaders(service.Client, "Basic", computed);
    }

    [Fact]
    public void BearerAuthenticatedClientInjectedSuccessfully()
    {
        const string token = "abc123";
        
        // Arrange
        _services.InjectHttpClientFor<MockService>(b 
            => b.WithBearerAuthentication(token));
        _services.AddScoped<MockService>();
        var provider = _services.BuildServiceProvider();
        
        // Act
        var service = provider.GetRequiredService<MockService>();
        
        // Assert
        AssertHeaders(service.Client, "Bearer", token);
    }

    private static void AssertHeaders(HttpClient client, string scheme, string? value = null)
    {
        client.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        client.DefaultRequestHeaders.Authorization?.Scheme.Should().Be(scheme);
        client.DefaultRequestHeaders.Authorization?.Parameter.Should().Be(value);
    }
}