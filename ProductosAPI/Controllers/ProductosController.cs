using Microsoft.AspNetCore.Mvc;
using ProductosAPI.Models;
using ProductosAPI.Services;

namespace ProductosAPI.Controllers;

/// <summary>
/// Endpoints para gestionar productos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductosController(IProductoService service)
    {
        _service = service;
    }

    /// <summary>Lista todos los productos.</summary>
    /// <response code="200">Lista de productos.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Producto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Producto>>> ObtenerTodos()
    {
        var productos = await _service.ObtenerTodosAsync();
        return Ok(productos);
    }

    /// <summary>Obtiene un producto por su id.</summary>
    /// <param name="id">Id del producto.</param>
    /// <response code="200">Producto encontrado.</response>
    /// <response code="404">No existe un producto con ese id.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Producto>> ObtenerPorId(int id)
    {
        var producto = await _service.ObtenerPorIdAsync(id);
        if (producto is null)
        {
            return NotFound(new { mensaje = $"No existe un producto con id {id}." });
        }

        return Ok(producto);
    }

    /// <summary>Crea un producto nuevo.</summary>
    /// <response code="201">Producto creado.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Producto>> Crear(Producto producto)
    {
        var creado = await _service.CrearAsync(producto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>Actualiza un producto existente.</summary>
    /// <param name="id">Id del producto. Debe coincidir con el id del cuerpo.</param>
    /// <param name="producto">Datos actualizados del producto.</param>
    /// <response code="204">Producto actualizado.</response>
    /// <response code="400">Datos inválidos o ids que no coinciden.</response>
    /// <response code="404">No existe un producto con ese id.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, Producto producto)
    {
        if (id != producto.Id)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del producto." });
        }

        var actualizado = await _service.ActualizarAsync(id, producto);
        if (!actualizado)
        {
            return NotFound(new { mensaje = $"No existe un producto con id {id}." });
        }

        return NoContent();
    }

    /// <summary>Elimina un producto.</summary>
    /// <param name="id">Id del producto.</param>
    /// <response code="204">Producto eliminado.</response>
    /// <response code="404">No existe un producto con ese id.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _service.EliminarAsync(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No existe un producto con id {id}." });
        }

        return NoContent();
    }
}
