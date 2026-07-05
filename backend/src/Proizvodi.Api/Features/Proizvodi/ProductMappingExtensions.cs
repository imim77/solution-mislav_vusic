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

    public static ProizvodiDto ToDto(this Product product)
    {
        return new ProizvodiDto(
            product.Id,
            product.Title,
            product.Price,
            product.Description,
            product.Thumbnail);
    }

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
