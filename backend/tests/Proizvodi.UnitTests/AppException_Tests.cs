using System.Net;
using Proizvodi.Api;

namespace Proizvodi.UnitTests;

public class AppException_Tests
{
    [Fact]
    public void Constructor_WithResourceNameAndKey_SetsNotFoundMessage()
    {
        var exception = new AppException.NotFoundException("Product", 123);

        Assert.Equal("Product with identifier '123' was not found.", exception.Message);
    }

    [Fact]
    public void Constructor_WithResourceNameAndKey_SetsNotFoundStatusCode()
    {
        var exception = new AppException.NotFoundException("Product", 123);

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public void Constructor_WithStringKey_SetsNotFoundMessage()
    {
        var exception = new AppException.NotFoundException("Category", "electronics");

        Assert.Equal("Category with identifier 'electronics' was not found.", exception.Message);
    }
}
