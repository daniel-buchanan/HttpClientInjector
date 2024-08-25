using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HttpClientInjector.Authentication;

public sealed class BasicAuthentication() : 
    ClientAuthentication(AuthenticationType.Basic), 
    IBasicAuthentication
{
    protected override void ApplyInternal(HttpClient client)
    {
        var base64 = EncodeHeader(Username, Password);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Kind.ToString(), base64);
    }

    public string Username { get; private set; }
    public string Password { get; private set; }
    
    public IBasicAuthentication SetCredential(string username, string password)
    {
        Username = username;
        Password = password;
        return this;
    }

    public static IBasicAuthentication Create(string username, string password)
        => new BasicAuthentication().SetCredential(username, password);
    
    public static string EncodeHeader(string username, string password)
    {
        var combined = $"{username}:{password}";
        var bytes = Encoding.UTF8.GetBytes(combined);
        return Convert.ToBase64String(bytes);
    }
}