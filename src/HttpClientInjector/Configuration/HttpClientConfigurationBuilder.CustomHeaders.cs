using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HttpClientInjector.Configuration;

public partial class HttpClientConfigurationBuilder
{
    private readonly List<KeyValuePair<string, string>> _customHeaders = [];
    
    public IHttpClientConfigurationBuilder WithCustomHeader(string key, string value)
    {
        _customHeaders.Add(new KeyValuePair<string, string>(key, value));
        return this;
    }
    
    public IHttpClientConfigurationBuilder WithCustomHeader(KeyValuePair<string, string> pair)
    {
        _customHeaders.Add(pair);
        return this;
    }
    
    public IHttpClientConfigurationBuilder WithCustomHeader(string headerName, Func<IServiceProvider, string> builder)
    {
        var value = builder(provider);
        return WithCustomHeader(headerName, value);
    }
    
    public IHttpClientConfigurationBuilder WithCustomHeader(Func<IServiceProvider, KeyValuePair<string, string>> builder)
    {
        var pair = builder(provider);
        return WithCustomHeader(pair);
    }
    
    private void ApplyCustomHeaders(HttpClient client)
    {
        _customHeaders.ForEach(c => client.DefaultRequestHeaders.Add(c.Key, c.Value));
    }
}