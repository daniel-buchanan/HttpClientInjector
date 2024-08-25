using System;
using HttpClientInjector.Authentication;

namespace HttpClientInjector.Configuration;

public partial class HttpClientConfigurationBuilder
{
    public IHttpClientConfigurationBuilder WithBasicAuthentication(string username, string password)
        => WithAuthentication(BasicAuthentication.Create(username, password));
    
    public IHttpClientConfigurationBuilder WithBasicAuthentication(IBasicAuthentication authentication)
        => WithAuthentication(authentication);

    public IHttpClientConfigurationBuilder WithBasicAuthentication(Func<IBasicAuthentication> builder)
        => WithAuthentication(builder);

    public IHttpClientConfigurationBuilder WithBasicAuthentication(Func<IServiceProvider, IBasicAuthentication> builder)
        => WithAuthentication(builder);
}