using System.Net.Http;

namespace HttpClientInjector.Authentication;

public interface IClientAuthentication
{
    AuthenticationType Kind { get; }

    void Apply(HttpClient client);
}

public interface INoAuthentication : IClientAuthentication { }

