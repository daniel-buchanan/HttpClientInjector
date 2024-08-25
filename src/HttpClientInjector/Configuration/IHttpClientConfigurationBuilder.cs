using System;
using System.Net.Http;
using HttpClientInjector.Authentication;

namespace HttpClientInjector.Configuration;

public interface IHttpClientConfigurationBuilder
{
    void Apply(HttpClient client);
    
    IHttpClientConfigurationBuilder WithBaseUrl(string url);
    IHttpClientConfigurationBuilder WithBaseUri(Uri uri);
    
    IHttpClientConfigurationBuilder WithoutAuthentication();
    
    IHttpClientConfigurationBuilder WithAuthentication(IClientAuthentication authentication);
    IHttpClientConfigurationBuilder WithAuthentication(Func<IClientAuthentication> builder);
    IHttpClientConfigurationBuilder WithAuthentication(Func<IServiceProvider, IClientAuthentication> builder);
    
    IHttpClientConfigurationBuilder WithBasicAuthentication(string username, string password);
    IHttpClientConfigurationBuilder WithBasicAuthentication(IBasicAuthentication authentication);
    IHttpClientConfigurationBuilder WithBasicAuthentication(Func<IBasicAuthentication> builder);
    IHttpClientConfigurationBuilder WithBasicAuthentication(Func<IServiceProvider, IBasicAuthentication> builder);
    
    IHttpClientConfigurationBuilder WithBearerAuthentication(string token);
    IHttpClientConfigurationBuilder WithBearerAuthentication(IBearerAuthentication authentication);
    IHttpClientConfigurationBuilder WithBearerAuthentication(Func<IBearerAuthentication> builder);
    IHttpClientConfigurationBuilder WithBearerAuthentication(Func<IServiceProvider, IBearerAuthentication> builder);

    IHttpClientConfigurationBuilder WithJsonEncoding();
    IHttpClientConfigurationBuilder WithXmlEncoding();
    IHttpClientConfigurationBuilder WithCompression();
}