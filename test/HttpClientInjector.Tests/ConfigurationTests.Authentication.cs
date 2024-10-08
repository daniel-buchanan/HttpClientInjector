using FluentAssertions;
using HttpClientInjector.Authentication;
using HttpClientInjector.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HttpClientInjector.Tests;

public partial class ConfigurationTests
{
    [Fact]
    public void NoAuthenticationSetDoesNotThrow()
    {
        // Arrange
        var client = new HttpClient();
        
        // Act
        Action method = () => _builder.Apply(client);

        // Assert
        method.Should().NotThrow();
    }
    
    [Fact]
    public void WithoutAuthenticationDoesNotSet()
    {
        // Arrange
        var client = new HttpClient();
        
        // Act
        _builder.WithoutAuthentication();
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders.Authorization.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(ProvidedAuthenticationTests))]
    public void WithAuthenticationProvidedSucceeds(IClientAuthentication auth, string scheme, string value)
    {
        // Arrange
        var client = new HttpClient();

        // Act
        _builder.WithAuthentication(auth);
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders
            .Authorization!
            .Scheme
            .Should()
            .Be(scheme);
        client.DefaultRequestHeaders
            .Authorization
            .Parameter
            .Should()
            .Be(value);
    }

    public static TheoryData<IClientAuthentication, string, string> ProvidedAuthenticationTests =>
        new()
        {
            {new BasicAuthentication().SetCredential("user", "name"), "Basic", "dXNlcjpuYW1l"},
            {new BearerAuthentication().SetCredential("token"), "Bearer", "token"}
        };

    [Theory]
    [MemberData(nameof(ProvidedAuthenticationFunctionTests))]
    public void WithAuthenticationFunctionProvidedSucceeds(Func<IClientAuthentication> auth, string scheme, string value)
    {
        // Arrange
        var client = new HttpClient();

        // Act
        _builder.WithAuthentication(auth);
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders
            .Authorization!
            .Scheme
            .Should()
            .Be(scheme);
        client.DefaultRequestHeaders
            .Authorization
            .Parameter
            .Should()
            .Be(value);
    }
    
    public static TheoryData<Func<IClientAuthentication>, string, string> ProvidedAuthenticationFunctionTests =>
        new()
        {
            {() => new BasicAuthentication().SetCredential("user", "name"), "Basic", "dXNlcjpuYW1l"},
            {() => new BearerAuthentication().SetCredential("token"), "Bearer", "token"}
        };

    [Theory]
    [MemberData(nameof(ProvidedAuthenticationBuilderTests))]
    public void WithAuthenticationBuilderProvidedSucceeds(Func<IServiceProvider, IClientAuthentication> auth, string scheme, string value)
    {
        // Arrange
        var client = new HttpClient();

        // Act
        _builder.WithAuthentication(auth);
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders
            .Authorization!
            .Scheme
            .Should()
            .Be(scheme);
        client.DefaultRequestHeaders
            .Authorization
            .Parameter
            .Should()
            .Be(value);
    }
    
    public static TheoryData<Func<IServiceProvider, IClientAuthentication>, string, string> ProvidedAuthenticationBuilderTests =>
        new()
        {
            {p => new BasicAuthentication().SetCredential("user", "name"), "Basic", "dXNlcjpuYW1l"},
            {p => new BearerAuthentication().SetCredential("token"), "Bearer", "token"}
        };
}