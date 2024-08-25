using System;
using HttpClientInjector.Authentication;

namespace HttpClientInjector.Configuration;

public partial class HttpClientConfigurationBuilder
{
    private IClientAuthentication _authentication;

    public IHttpClientConfigurationBuilder WithoutAuthentication()
        => WithAuthentication(new NoAuthentication());

    public IHttpClientConfigurationBuilder WithAuthentication(IClientAuthentication authentication)
    {
        _authentication = authentication;
        return this;
    }

    public IHttpClientConfigurationBuilder WithAuthentication(Func<IClientAuthentication> builder)
    {
        _authentication = builder();
        return this;
    }

    public IHttpClientConfigurationBuilder WithAuthentication(Func<IServiceProvider, IClientAuthentication> builder)
    {
        _authentication = builder(provider);
        return this;
    }
}