using AutoMapper;
using Garagem75.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔥 BANCO DE DADOS
builder.Services.AddDbContext<Garagem75DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔥 CONTROLLERS
builder.Services.AddControllers();

// 🔥 SWAGGER (ESSENCIAL PRA TESTAR)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("liberado",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var key = "GARAGEM75_CHAVE_ULTRA_SECRETA_COM_64_CARACTERES_1234567890"; // depois colocamos no appsettings

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddAuthorization();

// 🔥 AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program));

// 🔥 AUTH (por causa do [Authorize])
builder.Services.AddAuthorization();

var app = builder.Build();

// 🔥 SWAGGER UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔥 PIPELINE
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("liberado");
    
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();