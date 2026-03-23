using L4GA.Backend.Data;
using L4GA.Backend.Interfaces;
using L4GA.Backend.Repositories;
using L4GA.Backend.Services;
using L4GA.Interfaces;
using L4GA.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de la Base de Datos (MySQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// 2. Configuración de CORS para Angular (Local + Producción)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
    policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",                     // Tu entorno de VS Code
                "https://l4ga-frontend.azurestaticapps.net"  // URL asignada por Azure
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 3. Configuración de Controladores con manejo de Ciclos
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Vital para el reporte de Nóminas y Transportes
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 4. Inyección de Dependencias
// Usuarios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Transporte
builder.Services.AddScoped<ITransporteRepository, TransporteRepository>();
builder.Services.AddScoped<ITransporteService, TransporteService>();

// Nóminas
builder.Services.AddScoped<INominaRepository, NominaRepository>();
builder.Services.AddScoped<INominaService, NominaService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "L4GA API V1");
        c.RoutePrefix = string.Empty; // Esto hace que Swagger cargue en la raíz!
    });
}

app.UseHttpsRedirection();

// IMPORTANTE: UseCors debe ir después de HttpsRedirection y antes de Authorization
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();