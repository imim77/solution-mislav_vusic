using Proizvodi.Api.Models;

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

    public static User ToUser(this LoginResponse login)
    {
        return new User
        {
            Id = login.Id,
            Username = login.Username,
            FirstName = login.FirstName,
            LastName = login.LastName,
            Email = login.Email,
            AccessToken = login.AccessToken,
            RefreshToken = login.RefreshToken
        };
    }

    public static void UpdateTokens(this User user, LoginResponse login)
    {
        user.AccessToken = login.AccessToken;
        user.RefreshToken = login.RefreshToken;
    }
}
