using System;
using HttpClientInjector.Authentication;

namespace HttpClientInjector.Configuration;

public partial class HttpClientConfigurationBuilder
{
    public IHttpClientConfigurationBuilder WithBearerAuthentication(string token)
        => WithAuthentication(BearerAuthentication.Create(token));
    
    public IHttpClientConfigurationBuilder WithBearerAuthentication(IBearerAuthentication authentication)
        => WithAuthentication(authentication);

    public IHttpClientConfigurationBuilder WithBearerAuthentication(Func<IBearerAuthentication> builder)
        => WithAuthentication(builder);

    public IHttpClientConfigurationBuilder WithBearerAuthentication(Func<IServiceProvider, IBearerAuthentication> builder)
        => WithAuthentication(builder);
}