using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HttpClientInjector.Configuration;

public partial class HttpClientConfigurationBuilder(IServiceProvider provider) :
    IHttpClientConfigurationBuilder
{
    private const string ApplicationJson = "application/json";
    private const string TextXml = "text/xml";
    private const string All = "*";
    private const string Brotli = "br";
    
    private string _url;
    private string _compression = All;
    private string _encoding = ApplicationJson;

    public IHttpClientConfigurationBuilder WithBaseUrl(string url)
    {
        _url = url;
        return this;
    }

    public IHttpClientConfigurationBuilder WithBaseUri(Uri uri)
    {
        _url = uri.ToString();
        return this;
    }

    public IHttpClientConfigurationBuilder WithJsonEncoding()
    {
        _encoding = ApplicationJson;
        return this;
    }

    public IHttpClientConfigurationBuilder WithXmlEncoding()
    {
        _encoding = TextXml;
        return this;
    }

    public IHttpClientConfigurationBuilder WithCompression()
    {
        _compression = Brotli;
        return this;
    }

    public void Apply(HttpClient client)
    {
        if(!string.IsNullOrWhiteSpace(_url)) client.BaseAddress = new Uri(_url);
        _authentication?.Apply(client);
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(_compression));
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_encoding));
    }
}