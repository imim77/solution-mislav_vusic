using System.ComponentModel.DataAnnotations;

namespace Proizvodi.Api.Features.Proizvodi;

public record ProizvodiResponse(
    List<ProizvodiDto> Products,
    int Total,
    int Skip,
    int Limit
);

public record ProizvodiDto(
    [Required] string Title,
    [Required] decimal Price,
    [Required][StringLength(100)] string Description,
    [Required] string Thumbnail
);

public record LoginRequest(
    string username,
    string password,
    int? ExpiresInMins
);

public record LoginResponse(
    int Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Gender,
    string Image,
    string AccessToken,
    string RefreshToken
);
