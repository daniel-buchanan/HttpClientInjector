using FluentAssertions;
using HttpClientInjector.Authentication;
using HttpClientInjector.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HttpClientInjector.Tests;

public partial class ConfigurationTests
{
    [Theory]
    [MemberData(nameof(BasicAuthenticationTests))]
    public void WithBasicAuthenticationProvidedSucceeds(IBasicAuthentication auth, string scheme, string value)
    {
        // Arrange
        var client = new HttpClient();

        // Act
        _builder.WithBasicAuthentication(auth);
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

    public static TheoryData<IBasicAuthentication, string, string> BasicAuthenticationTests =>
        new()
        {
            {new BasicAuthentication().SetCredential("user", "name"), "Basic", "dXNlcjpuYW1l"},
            {new BasicAuthentication().SetCredential(null, null), "Basic", BasicAuthentication.EncodeHeader(null,null)}
        };

    [Theory]
    [MemberData(nameof(BasicAuthenticationBuilderTests))]
    public void WithBasicAuthenticationBuilderSucceeds(string username, string password, string expected)
    {
        // Arrange
        var client = new HttpClient();

        // Act
        _builder.WithBasicAuthentication(() => new BasicAuthentication().SetCredential(username, password));
        _builder.Apply(client);

        // Assert
        client.DefaultRequestHeaders
            .Authorization!
            .Scheme
            .Should()
            .Be("Basic");
        client.DefaultRequestHeaders
            .Authorization
            .Parameter
            .Should()
            .Be(expected);
    }
    
    public static TheoryData<string, string, string> BasicAuthenticationBuilderTests =>
        new()
        {
            {"bob", "smith", BasicAuthentication.EncodeHeader("bob", "smith")}
        };
}