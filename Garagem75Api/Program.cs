using AutoMapper;
using Garagem75.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenApi = Microsoft.OpenApi; // Criamos um "apelido" para o Swaggerusing System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔥 BANCO DE DADOS
builder.Services.AddDbContext<Garagem75DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔥 CONTROLLERS
builder.Services.AddControllers();

// 🔥 SWAGGER (ESSENCIAL PRA TESTAR)
builder.Services.AddEndpointsApiExplorer();
// Configuração do Swagger para suportar JWT
builder.Services.AddSwaggerGen(c =>
{
    // Usando o nome completo para Info
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Garagem 75 API", Version = "v1" });

    // Definindo a segurança sem usar o namespace .Models no meio
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header. Digite 'Bearer' [espaço] e o token."
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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

// --- INÍCIO DO TRECHO DE AUTENTICAÇÃO ---

// 1. Busca a chave do appsettings.json
var chaveJwt = builder.Configuration["Jwt:ChaveSecreta"];

// 2. Verifica se a chave foi encontrada para não dar erro de nulo
if (string.IsNullOrEmpty(chaveJwt))
{
    throw new Exception("Chave JWT não encontrada no appsettings.json!");
}

var keyBytes = Encoding.UTF8.GetBytes(chaveJwt);

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
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.Zero // Garante que o token expire na hora exata
    };

    // Eventos para ajudar você a debugar no terminal da API
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("ERRO JWT: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("TOKEN VALIDADO COM SUCESSO!");
            return Task.CompletedTask;
        }
    };
});

// --- FIM DO TRECHO DE AUTENTICAÇÃO ---

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

app.UseHttpsRedirection();
app.UseStaticFiles();

// 🔥 A ORDEM EXATA É ESSA:
app.UseRouting(); // 1. Roteamento

app.UseCors("liberado"); // 2. Cors (Sempre antes da Auth)

app.UseAuthentication(); // 3. Autenticação (Quem é você?)
app.UseAuthorization();  // 4. Autorização (O que você pode fazer?)

app.MapControllers();

app.Run();