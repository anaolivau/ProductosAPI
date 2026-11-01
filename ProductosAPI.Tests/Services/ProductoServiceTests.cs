using Moq;
using ProductosAPI.Models;
using ProductosAPI.Repositories;
using ProductosAPI.Services;

namespace ProductosAPI.Tests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _repositoryMock;
    private readonly ProductoService _service;

    public ProductoServiceTests()
    {
        _repositoryMock = new Mock<IProductoRepository>();
        _service = new ProductoService(_repositoryMock.Object);
    }

    private static Producto CrearProducto(int id) => new()
    {
        Id = id,
        Nombre = "Mouse inalambrico",
        Descripcion = "Mouse optico",
        Categoria = "Accesorios",
        Precio = 39.90m,
        Stock = 80
    };

    [Fact]
    public async Task ObtenerTodosAsync_DevuelveTodosLosProductos()
    {
        // Arrange
        var productos = new List<Producto> { CrearProducto(1), CrearProducto(2) };
        _repositoryMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(productos);

        // Act
        var resultado = await _service.ObtenerTodosAsync();

        // Assert
        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ProductoExiste_DevuelveProducto()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(CrearProducto(1));

        // Act
        var resultado = await _service.ObtenerPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ProductoNoExiste_DevuelveNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Producto?)null);

        // Act
        var resultado = await _service.ObtenerPorIdAsync(99);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task CrearAsync_ReiniciaIdYQuitaEspacios()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 99,
            Nombre = "  Teclado mecanico  ",
            Categoria = " Accesorios ",
            Precio = 159m,
            Stock = 40
        };
        _repositoryMock.Setup(r => r.CrearAsync(It.IsAny<Producto>()))
            .ReturnsAsync((Producto p) => p);

        // Act
        var resultado = await _service.CrearAsync(producto);

        // Assert
        Assert.Equal(0, resultado.Id);
        Assert.Equal("Teclado mecanico", resultado.Nombre);
        Assert.Equal("Accesorios", resultado.Categoria);
        _repositoryMock.Verify(r => r.CrearAsync(It.IsAny<Producto>()), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsync_ProductoExiste_ActualizaYDevuelveTrue()
    {
        // Arrange
        var existente = CrearProducto(1);
        _repositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(existente);
        var cambios = new Producto
        {
            Id = 1,
            Nombre = " Mouse gamer ",
            Descripcion = "Mouse con luces RGB",
            Categoria = "Gaming",
            Precio = 89.90m,
            Stock = 10
        };

        // Act
        var resultado = await _service.ActualizarAsync(1, cambios);

        // Assert
        Assert.True(resultado);
        Assert.Equal("Mouse gamer", existente.Nombre);
        Assert.Equal(89.90m, existente.Precio);
        Assert.Equal(10, existente.Stock);
        _repositoryMock.Verify(r => r.ActualizarAsync(existente), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsync_ProductoNoExiste_DevuelveFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Producto?)null);

        // Act
        var resultado = await _service.ActualizarAsync(99, CrearProducto(99));

        // Assert
        Assert.False(resultado);
        _repositoryMock.Verify(r => r.ActualizarAsync(It.IsAny<Producto>()), Times.Never);
    }

    [Fact]
    public async Task EliminarAsync_ProductoExiste_DevuelveTrue()
    {
        // Arrange
        var existente = CrearProducto(1);
        _repositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(existente);

        // Act
        var resultado = await _service.EliminarAsync(1);

        // Assert
        Assert.True(resultado);
        _repositoryMock.Verify(r => r.EliminarAsync(existente), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_ProductoNoExiste_DevuelveFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Producto?)null);

        // Act
        var resultado = await _service.EliminarAsync(99);

        // Assert
        Assert.False(resultado);
        _repositoryMock.Verify(r => r.EliminarAsync(It.IsAny<Producto>()), Times.Never);
    }
}
