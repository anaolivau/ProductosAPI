using System.ComponentModel.DataAnnotations;

namespace ProductosAPI.Models;

/// <summary>
/// Representa un producto del inventario.
/// </summary>
public class Producto
{
    /// <summary>Identificador único, generado por la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Nombre del producto.</summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción opcional del producto.</summary>
    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
    public string? Descripcion { get; set; }

    /// <summary>Categoría a la que pertenece el producto.</summary>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "La categoría debe tener entre 3 y 50 caracteres.")]
    public string Categoria { get; set; } = string.Empty;

    /// <summary>Precio unitario en soles.</summary>
    [Range(0.01, 100000, ErrorMessage = "El precio debe estar entre 0.01 y 100000.")]
    public decimal Precio { get; set; }

    /// <summary>Cantidad disponible en inventario.</summary>
    [Range(0, 10000, ErrorMessage = "El stock debe estar entre 0 y 10000.")]
    public int Stock { get; set; }
}
