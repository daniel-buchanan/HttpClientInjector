using FluentAssertions;
using HttpClientInjector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HttpClientInjector.Tests;

public class FullUpTests
{
    private readonly IServiceCollection _services = new ServiceCollection();

    [Fact]
    public void ConfiguredClientReturned()
    {
        // Arrange
        _services.AddHttpClient();
        _services.InjectHttpClientFor<IMockService>(b 
            => b.WithBaseUrl("http://test.com").WithoutAuthentication());
        var provider = _services.BuildServiceProvider();

        // Act
        var client = provider.GetHttpFor<IMockService>();

        // Assert
        client.Should().NotBeNull();
    }
    
    [Fact]
    public void ClientInjectedSuccessfullyWithoutAddHttpClient()
    {
        // Arrange
        _services.InjectHttpClientFor<IMockService>(b => b.WithoutAuthentication());
        _services.AddScoped<IMockService, MockService>();
        var provider = _services.BuildServiceProvider();

        // Act
        var service = provider.GetService<IMockService>();
        var factory = provider.GetService<IHttpClientFactory>();

        // Assert
        factory.Should().NotBeNull();
        service.Should().NotBeNull();
        service?.Client.Should().NotBeNull();
    }
    
    [Fact]
    public void ClientInjectedSuccessfullyWithConcreteImplementation()
    {
        // Arrange
        _services.InjectHttpClientFor<MockService2>(b => b.WithoutAuthentication());
        _services.AddScoped<MockService2>();
        var provider = _services.BuildServiceProvider();

        // Act
        var service = provider.GetService<MockService2>();
        var factory = provider.GetService<IHttpClientFactory>();

        // Assert
        factory.Should().NotBeNull();
        service.Should().NotBeNull();
        service?.Client.Should().NotBeNull();
    }

    [Fact]
    public void ClientInjectedSuccessfully()
    {
        // Arrange
        _services.AddHttpClient();
        _services.InjectHttpClientFor<IMockService, MockService>(b => b.WithoutAuthentication());
        _services.AddScoped<IMockService, MockService>();
        var provider = _services.BuildServiceProvider();

        // Act
        var service = provider.GetService<IMockService>();

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
        _services.AddHttpClient();
        _services.InjectHttpClientFor<IMockService>(b 
            => b.WithBasicAuthentication(username, password));
        _services.AddScoped<IMockService, MockService>();
        var provider = _services.BuildServiceProvider();
        
        // Act
        var service = provider.GetRequiredService<IMockService>();
        
        // Assert
        AssertHeaders(service.Client, "Basic", computed);
    }

    [Fact]
    public void BearerAuthenticatedClientInjectedSuccessfully()
    {
        const string token = "abc123";
        
        // Arrange
        _services.AddHttpClient();
        _services.InjectHttpClientFor<IMockService>(b 
            => b.WithBearerAuthentication(token));
        _services.AddScoped<IMockService, MockService>();
        var provider = _services.BuildServiceProvider();
        
        // Act
        var service = provider.GetRequiredService<IMockService>();
        
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