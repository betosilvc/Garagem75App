using AutoMapper;
using Garagem75.Api.Data;
using Microsoft.EntityFrameworkCore;

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
    options.AddPolicy("blazor",
        policy =>
        {
            policy.WithOrigins("http://localhost:5135")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

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

app.UseCors("blazor");
    
app.UseAuthorization();

app.MapControllers();

app.Run();