namespace HttpClientInjector.Authentication;

public interface IBasicAuthentication : IClientAuthentication
{
    string Username { get; }
    string Password { get; }

    IBasicAuthentication SetCredential(string username, string password);
}