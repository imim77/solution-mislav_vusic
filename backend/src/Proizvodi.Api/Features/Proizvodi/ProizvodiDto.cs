using System.ComponentModel.DataAnnotations;

namespace Proizvodi.Api.Features.Proizvodi;

public record ProizvodiResponse(
    List<ProizvodiDto> Products,
    int Total,
    int Skip,
    int Limit
);

public record ProizvodiDto(
    [Required] int Id,
    [Required] string Title,
    [Required] decimal Price,
    [Required][StringLength(100)] string Description,
    [Required] string Thumbnail
);

public record ProductDetailsDto(
    [Required] int Id,
    [Required] string Title,
    [Required] decimal Price,
    [Required] string Description,
    [Required] string Category,
    string? Brand,
    [Required] double Rating,
    [Required] int Stock,
    [Required] string AvailabilityStatus, 
    [Required] List<string> Images,
    [Required] List<string> Tags,
    List<ProductReviewDto> Reviews
);

public record ProductReviewDto(
    [Required] int Rating,
    [Required][StringLength(500)] string Comment,
    [Required][StringLength(100)] string ReviewerName,
    [Required] DateTime Date
);

public record LoginRequest(
    string Username,
    string Password,
    int? ExpiresInMins
);

public record LoginResponse(
    int Id,
    string Username, 
    string FirstName,
    string LastName,
    string Email,
    string Image,
    string AccessToken,
    string RefreshToken
);
