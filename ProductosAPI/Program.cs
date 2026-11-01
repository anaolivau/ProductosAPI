using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductosAPI.Data;
using ProductosAPI.Repositories;
using ProductosAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Productos API",
        Version = "v1",
        Description = "API CRUD de productos con pruebas unitarias, cobertura y CI/CD."
    });

    var archivoXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opciones.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, archivoXml));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage(Justification = "Solo contiene configuración de arranque, sin lógica de negocio.")]
public partial class Program { }
