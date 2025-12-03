using HeroManager.Api.Domain.Repositories;
using HeroManager.Api.Domain.Services;
using HeroManager.Api.Infrastructure.Data;
using HeroManager.Api.Infrastructure.Repositories;
using HeroManager.Api.Infrastructure.Services;
using HeroManager.Api.Mapping;
using Microsoft.EntityFrameworkCore;              

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//InMemory pra facilitar os testes
builder.Services.AddDbContext<HeroContext>(options =>
    options.UseInMemoryDatabase("HeroDb"));

//var connectionString = builder.Configuration.GetConnectionString("HeroDb");

//builder.Services.AddDbContext<HeroContext>(options =>
//    options.UseSqlServer(connectionString));


builder.Services.AddScoped<IHeroRepository, HeroRepository>();
builder.Services.AddScoped<ISuperpowerRepository, SuperpowerRepository>();
builder.Services.AddScoped<IHeroService, HeroService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});


builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<HeroProfile>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { message = "Erro interno inesperado." });
    });
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
