using System.ComponentModel.DataAnnotations;

namespace Proizvodi.Api.Features.Categories;

public record CategoriesDto(
    [Required] string Name 
);