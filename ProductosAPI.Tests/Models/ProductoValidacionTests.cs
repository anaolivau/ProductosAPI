using System.ComponentModel.DataAnnotations;
using ProductosAPI.Models;

namespace ProductosAPI.Tests.Models;

public class ProductoValidacionTests
{
    private static Producto CrearProductoValido() => new()
    {
        Nombre = "Mouse inalambrico",
        Descripcion = "Mouse optico",
        Categoria = "Accesorios",
        Precio = 39.90m,
        Stock = 80
    };

    private static List<ValidationResult> Validar(Producto producto)
    {
        var resultados = new List<ValidationResult>();
        Validator.TryValidateObject(producto, new ValidationContext(producto), resultados, validateAllProperties: true);
        return resultados;
    }

    [Fact]
    public void ProductoValido_NoTieneErrores()
    {
        // Arrange
        var producto = CrearProductoValido();

        // Act
        var errores = Validar(producto);

        // Assert
        Assert.Empty(errores);
    }

    [Fact]
    public void NombreVacio_TieneError()
    {
        // Arrange
        var producto = CrearProductoValido();
        producto.Nombre = "";

        // Act
        var errores = Validar(producto);

        // Assert
        Assert.Contains(errores, e => e.MemberNames.Contains(nameof(Producto.Nombre)));
    }

    [Fact]
    public void NombreMuyCorto_TieneError()
    {
        // Arrange
        var producto = CrearProductoValido();
        producto.Nombre = "TV";

        // Act
        var errores = Validar(producto);

        // Assert
        Assert.Contains(errores, e => e.MemberNames.Contains(nameof(Producto.Nombre)));
    }

    [Fact]
    public void CategoriaVacia_TieneError()
    {
        // Arrange
        var producto = CrearProductoValido();
        producto.Categoria = "";

        // Act
        var errores = Validar(producto);

        // Assert
        Assert.Contains(errores, e => e.MemberNames.Contains(nameof(Producto.Categoria)));
    }

    [Fact]
    public void PrecioCero_TieneError()
    {
        // Arrange
        var producto = CrearProductoValido();
        producto.Precio = 0;

        // Act
        var errores = Validar(producto);

        // Assert
        Assert.Contains(errores, e => e.MemberNames.Contains(nameof(Producto.Precio)));
    }

    [Fact]
    public void StockNegativo_TieneError()
    {
        // Arrange
        var producto = CrearProductoValido();
        producto.Stock = -1;

        // Act
        var errores = Validar(producto);

        // Assert
        Assert.Contains(errores, e => e.MemberNames.Contains(nameof(Producto.Stock)));
    }
}
