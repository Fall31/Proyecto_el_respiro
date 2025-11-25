using ConsultorioAPI.Data;
using ConsultorioAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Necesario para JWT
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // Necesario para las llaves criptográficas
using System.Text; // Necesario para Encoding

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------
// 1. CONFIGURACIÓN DE SERVICIOS (LO QUE TU APP PUEDE HACER)
// -------------------------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// A. Conexión a Base de Datos (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("CadenaSQL");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IAuthService, AuthService>(); 

// B. Configuración de CORS (El permiso para que Vue se conecte)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirVue", policy =>
    {
        policy.AllowAnyOrigin()  // En producción deberías poner la URL real de Vue
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// C. Configuración de JWT (El Guardia de Seguridad) 👮‍♂️
// Leemos la clave secreta del appsettings.json
var key = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)), // ¡El secreto!
        ValidateIssuer = false, // Lo ponemos en false para facilitar desarrollo local
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// -------------------------------------------------------------------------
// 2. PIPELINE DE PETICIONES (EL ORDEN AQUÍ ES SAGRADO)
// -------------------------------------------------------------------------

// 1. Swagger (Solo en desarrollo para probar endpoints)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. CORS (Debe ir antes de seguridad)
app.UseCors("PermitirVue");

app.UseHttpsRedirection();

// 3. SEGURIDAD (Primero revisa quién eres, luego qué permiso tienes)
app.UseAuthentication(); // <--- ¡Esto faltaba!
app.UseAuthorization();

app.MapControllers();

app.Run();
