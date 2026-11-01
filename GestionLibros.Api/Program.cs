using GestionLibros.Data.Context;
using GestionLibros.Services.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var constr = builder.Configuration.GetConnectionString("Conexion");
builder.Services.AddDbContextFactory<Contexto>(options =>
    options.UseSqlite(constr));

builder.Services.AddScoped<EstudiantesService>();
builder.Services.AddScoped<LibrosService>();
builder.Services.AddScoped<PrestamosService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();             

var app = builder.Build();

app.UseSwagger();       
app.UseSwaggerUI();    

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();