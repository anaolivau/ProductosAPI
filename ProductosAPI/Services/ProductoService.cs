using ProductosAPI.Models;
using ProductosAPI.Repositories;

namespace ProductosAPI.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _repository.ObtenerPorIdAsync(id);
    }

    public async Task<Producto> CrearAsync(Producto producto)
    {
        producto.Id = 0;
        producto.Nombre = producto.Nombre.Trim();
        producto.Categoria = producto.Categoria.Trim();
        return await _repository.CrearAsync(producto);
    }

    public async Task<bool> ActualizarAsync(int id, Producto producto)
    {
        var existente = await _repository.ObtenerPorIdAsync(id);
        if (existente is null)
        {
            return false;
        }

        existente.Nombre = producto.Nombre.Trim();
        existente.Descripcion = producto.Descripcion;
        existente.Categoria = producto.Categoria.Trim();
        existente.Precio = producto.Precio;
        existente.Stock = producto.Stock;

        await _repository.ActualizarAsync(existente);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var existente = await _repository.ObtenerPorIdAsync(id);
        if (existente is null)
        {
            return false;
        }

        await _repository.EliminarAsync(existente);
        return true;
    }
}
