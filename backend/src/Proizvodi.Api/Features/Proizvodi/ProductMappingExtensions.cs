using System.Linq.Expressions;
using Proizvodi.Api.Models;

namespace Proizvodi.Api.Features.Proizvodi;

public static class ProductMappingExtensions
{
    public static readonly Expression<Func<Product, ProizvodiDto>> ToDtoProjection = product =>
        new ProizvodiDto(
            product.Id,
            product.Title,
            product.Price,
            product.Description,
            product.Thumbnail);

    private static readonly Func<Product, ProizvodiDto> CompiledProjection = ToDtoProjection.Compile();

    public static ProizvodiDto ToDto(this Product product) => CompiledProjection(product);

    public static Product ToProduct(this AddFavoriteRequest request)
    {
        return new Product
        {
            Id = request.ProductId,
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Thumbnail = request.Thumbnail
        };
    }

}
