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
