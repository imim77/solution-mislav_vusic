namespace Proizvodi.Api.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }

    public List<UserFavorite> Favorites { get; set; } = [];
}
