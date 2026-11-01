using Microsoft.EntityFrameworkCore;
using ProductosAPI.Data;
using ProductosAPI.Models;
using ProductosAPI.Repositories;

namespace ProductosAPI.Tests.Repositories;

public class ProductoRepositoryTests
{
    private static AppDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(opciones);
    }

    private static Producto NuevoProducto(string nombre) => new()
    {
        Nombre = nombre,
        Categoria = "Computo",
        Precio = 100m,
        Stock = 5
    };

    [Fact]
    public async Task CrearAsync_GuardaElProductoYAsignaId()
    {
        // Arrange
        using var contexto = CrearContexto();
        var repositorio = new ProductoRepository(contexto);

        // Act
        var creado = await repositorio.CrearAsync(NuevoProducto("Laptop"));

        // Assert
        Assert.True(creado.Id > 0);
        Assert.Equal(1, await contexto.Productos.CountAsync());
    }

    [Fact]
    public async Task ObtenerTodosAsync_DevuelveTodosLosProductos()
    {
        // Arrange
        using var contexto = CrearContexto();
        contexto.Productos.AddRange(NuevoProducto("Laptop"), NuevoProducto("Monitor"));
        await contexto.SaveChangesAsync();
        var repositorio = new ProductoRepository(contexto);

        // Act
        var resultado = await repositorio.ObtenerTodosAsync();

        // Assert
        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ProductoExiste_DevuelveProducto()
    {
        // Arrange
        using var contexto = CrearContexto();
        var producto = NuevoProducto("Laptop");
        contexto.Productos.Add(producto);
        await contexto.SaveChangesAsync();
        var repositorio = new ProductoRepository(contexto);

        // Act
        var resultado = await repositorio.ObtenerPorIdAsync(producto.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Laptop", resultado.Nombre);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ProductoNoExiste_DevuelveNull()
    {
        // Arrange
        using var contexto = CrearContexto();
        var repositorio = new ProductoRepository(contexto);

        // Act
        var resultado = await repositorio.ObtenerPorIdAsync(999);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task ActualizarAsync_GuardaLosCambios()
    {
        // Arrange
        using var contexto = CrearContexto();
        var producto = NuevoProducto("Laptop");
        contexto.Productos.Add(producto);
        await contexto.SaveChangesAsync();
        var repositorio = new ProductoRepository(contexto);
        producto.Precio = 250m;

        // Act
        await repositorio.ActualizarAsync(producto);

        // Assert
        var guardado = await contexto.Productos.FindAsync(producto.Id);
        Assert.Equal(250m, guardado!.Precio);
    }

    [Fact]
    public async Task EliminarAsync_QuitaElProducto()
    {
        // Arrange
        using var contexto = CrearContexto();
        var producto = NuevoProducto("Laptop");
        contexto.Productos.Add(producto);
        await contexto.SaveChangesAsync();
        var repositorio = new ProductoRepository(contexto);

        // Act
        await repositorio.EliminarAsync(producto);

        // Assert
        Assert.Equal(0, await contexto.Productos.CountAsync());
    }
}
