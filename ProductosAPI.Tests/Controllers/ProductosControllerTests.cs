using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductosAPI.Controllers;
using ProductosAPI.Models;
using ProductosAPI.Services;

namespace ProductosAPI.Tests.Controllers;

public class ProductosControllerTests
{
    private readonly Mock<IProductoService> _serviceMock;
    private readonly ProductosController _controller;

    public ProductosControllerTests()
    {
        _serviceMock = new Mock<IProductoService>();
        _controller = new ProductosController(_serviceMock.Object);
    }

    private static Producto CrearProducto(int id) => new()
    {
        Id = id,
        Nombre = "Monitor Samsung",
        Categoria = "Computo",
        Precio = 549m,
        Stock = 20
    };

    [Fact]
    public async Task ObtenerTodos_DevuelveOkConProductos()
    {
        // Arrange
        var productos = new List<Producto> { CrearProducto(1), CrearProducto(2) };
        _serviceMock.Setup(s => s.ObtenerTodosAsync()).ReturnsAsync(productos);

        // Act
        var resultado = await _controller.ObtenerTodos();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(resultado.Result);
        var lista = Assert.IsAssignableFrom<IEnumerable<Producto>>(ok.Value);
        Assert.Equal(2, lista.Count());
    }

    [Fact]
    public async Task ObtenerPorId_ProductoExiste_DevuelveOk()
    {
        // Arrange
        var producto = CrearProducto(1);
        _serviceMock.Setup(s => s.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        // Act
        var resultado = await _controller.ObtenerPorId(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(resultado.Result);
        Assert.Equal(producto, ok.Value);
    }

    [Fact]
    public async Task ObtenerPorId_ProductoNoExiste_DevuelveNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.ObtenerPorIdAsync(99)).ReturnsAsync((Producto?)null);

        // Act
        var resultado = await _controller.ObtenerPorId(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Crear_DevuelveCreatedAtAction()
    {
        // Arrange
        var creado = CrearProducto(6);
        _serviceMock.Setup(s => s.CrearAsync(It.IsAny<Producto>())).ReturnsAsync(creado);

        // Act
        var resultado = await _controller.Crear(CrearProducto(0));

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(resultado.Result);
        Assert.Equal(nameof(ProductosController.ObtenerPorId), created.ActionName);
        Assert.Equal(6, (int)created.RouteValues!["id"]!);
    }

    [Fact]
    public async Task Actualizar_IdsNoCoinciden_DevuelveBadRequest()
    {
        // Arrange
        var producto = CrearProducto(2);

        // Act
        var resultado = await _controller.Actualizar(1, producto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        _serviceMock.Verify(s => s.ActualizarAsync(It.IsAny<int>(), It.IsAny<Producto>()), Times.Never);
    }

    [Fact]
    public async Task Actualizar_ProductoNoExiste_DevuelveNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.ActualizarAsync(99, It.IsAny<Producto>())).ReturnsAsync(false);

        // Act
        var resultado = await _controller.Actualizar(99, CrearProducto(99));

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado);
    }

    [Fact]
    public async Task Actualizar_ProductoExiste_DevuelveNoContent()
    {
        // Arrange
        _serviceMock.Setup(s => s.ActualizarAsync(1, It.IsAny<Producto>())).ReturnsAsync(true);

        // Act
        var resultado = await _controller.Actualizar(1, CrearProducto(1));

        // Assert
        Assert.IsType<NoContentResult>(resultado);
    }

    [Fact]
    public async Task Eliminar_ProductoExiste_DevuelveNoContent()
    {
        // Arrange
        _serviceMock.Setup(s => s.EliminarAsync(1)).ReturnsAsync(true);

        // Act
        var resultado = await _controller.Eliminar(1);

        // Assert
        Assert.IsType<NoContentResult>(resultado);
    }

    [Fact]
    public async Task Eliminar_ProductoNoExiste_DevuelveNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.EliminarAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _controller.Eliminar(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado);
    }
}
