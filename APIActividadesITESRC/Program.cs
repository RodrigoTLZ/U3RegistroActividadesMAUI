using APIActividadesITESRC.Helper;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ItesrcneActividadesContext>(x => x.UseMySql("server=204.93.216.11;database=itesrcne_actividades;user=itesrcne_deptos;password=sistemaregistrotec24", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb")));
builder.Services.AddTransient<DepartamentosRepository>();
builder.Services.AddTransient<ActividadesRepository>();
var connectionString = builder.Configuration.GetConnectionString("ActividadesConnectionString");
builder.Services.AddSingleton<GeneradorToken>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x=>
{
    var issuer = builder.Configuration.GetSection("Jwt").GetValue<string>("Issuer");
    var audience = builder.Configuration.GetSection("Jwt").GetValue<string>("Audience");
    var secret = builder.Configuration.GetSection("Jwt").GetValue<string>("Secret");

    x.TokenValidationParameters = new()
    {
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true
    };
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
