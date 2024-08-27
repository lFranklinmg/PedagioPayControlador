using Microsoft.EntityFrameworkCore;
using PedagioPayApiControlador.Data.Interfaces;
using PedagioPayApiControlador.Data.Repositories;
using PedagioPayApiControlador.Models;
using PedagioPayApiControlador.Service;
using PedagioPayApiControlador.Service.Interface;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PEDAGIOPAYControladorContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevelopmentDB"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddControllers().AddNewtonsoftJson();
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ICartaoUsuarioRepository, CartaoUsuarioRepository>();
builder.Services.AddScoped<IDebitoRepository, DebitoRepository>();
builder.Services.AddScoped<ICartaoUsuarioService, CartaoUsuarioService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<ICriptografiaService, CriptograService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", c =>
{
    c.AllowAnyOrigin()
       .AllowAnyHeader()
       .AllowAnyMethod();
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, lc) => lc
.WriteTo.Console()
.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    builder.WebHost.UseUrls("http://localhost:7008");
#if !DEBUG
    builder.WebHost.UseEnvironment("Production");
    builder.WebHost.UseIISIntegration();
#endif
}

app.UsePathBase("/pedagiopay-cabine-controlador-api");

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
