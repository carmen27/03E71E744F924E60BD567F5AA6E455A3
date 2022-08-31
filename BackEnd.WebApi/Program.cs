using BackEnd.Business;
using BackEnd.Common;
using BackEnd.Data;
using BackEnd.Interface.Business;
using BackEnd.Interface.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(options => builder.Configuration.Bind(options));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
builder.Services.AddScoped<IBoUsuario, BoUsuario>();
builder.Services.AddScoped<IDoUsuario, DoUsuario>();
builder.Services.AddScoped<IBoProducto, BoProducto>();
builder.Services.AddScoped<IDoProducto, DoProducto>();
builder.Services.AddScoped<IBoCompra, BoCompra>();
builder.Services.AddScoped<IDoCompra, DoCompra>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();