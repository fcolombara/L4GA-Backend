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

// 2. Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
    policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://salmon-sky-0c876580f.1.azurestaticapps.net" // <--- SIN la barra "/" al final
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 3. Configuración de Controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 4. Inyección de Dependencias
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITransporteRepository, TransporteRepository>();
builder.Services.AddScoped<ITransporteService, TransporteService>();
builder.Services.AddScoped<INominaRepository, NominaRepository>();
builder.Services.AddScoped<INominaService, NominaService>();
builder.Services.AddScoped<IOperacionRepository, OperacionRepository>();
builder.Services.AddScoped<IOperacionService, OperacionService>();
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- CONFIGURACIÓN DEL PIPELINE (EL ORDEN IMPORTA) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "L4GA API V1");
        c.RoutePrefix = string.Empty;
    });
}

// 1. Ruteo (Primero)
app.UseRouting();

// 2. CORS (Después de Routing, antes de Auth)
app.UseCors("AllowAngularApp");

// 3. Autorización
app.UseAuthorization();

// 4. Mapeo de Controladores
app.MapControllers();

app.Run();