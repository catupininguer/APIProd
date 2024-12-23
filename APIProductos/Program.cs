using Microsoft.EntityFrameworkCore;
using APIProductos.Data;

var builder = WebApplication.CreateBuilder(args);
var misReglasCors = "ReglasCors";

builder.Services.AddCors(option =>
option.AddPolicy(name: misReglasCors,
builder =>
{
    builder.AllowAnyOrigin() // allowanyorigins()
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

// Registrar MyDbContext y la conexión a la base de datos
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(misReglasCors);
}


app.UseAuthorization();

app.MapControllers();

app.Run();
