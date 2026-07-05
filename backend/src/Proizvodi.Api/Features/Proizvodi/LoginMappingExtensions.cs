namespace Proizvodi.Api.Features.Proizvodi;

public static class LoginMappingExtensions
{
    public static LoginResult ToLoginResult(this LoginResponse login)
    {
        return new LoginResult(
            login.Id,
            login.Username,
            login.FirstName,
            login.LastName,
            login.AccessToken);
    }
}
