using ApiUpClass.DataContexts;
using ApiUpClass.Profiles;
using ApiUpClass.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("mysql");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)))
);

builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<CursoService>();
builder.Services.AddScoped<ModuloService>();
builder.Services.AddScoped<AulaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<MatriculaService>();
builder.Services.AddScoped<PagamentoService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CategoriaProfile>();
    config.AddProfile<CursoProfile>();
    config.AddProfile<ModuloProfile>();
    config.AddProfile<AulaProfile>();
    config.AddProfile<UsuarioProfile>();
    config.AddProfile<MatriculaProfile>();
    config.AddProfile<PagamentoProfile>();
});

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
