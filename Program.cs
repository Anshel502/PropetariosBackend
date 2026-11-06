using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
// program define inicio

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddSingleton<IPeopleService, PeopleService>();
//otro servicio
builder.Services.AddKeyedSingleton<IPeopleService, PeopleService>("peopleService");
builder.Services.AddKeyedSingleton<IPeopleService, People2Service>("people2Service");

builder.Services.AddKeyedSingleton<IRandomService, RandomService>("randomSingleton");
builder.Services.AddKeyedScoped<IRandomService, RandomService>("randomScoped");
builder.Services.AddKeyedTransient<IRandomService, RandomService>("randomTransient");

builder.Services.AddHttpClient<IPostService, PostService>(client =>
{
    var baseUrl = builder.Configuration["BaseUrlPost"]
        ?? throw new InvalidOperationException("The BaseUrlPost configuration value is required.");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreConnection"));
});

// Registros de Beer
builder.Services.AddScoped<IValidator<BeerInsertDto>, BeerInsertValidator>();
builder.Services.AddScoped<IValidator<BeerUpdateDto>, BeerUpdateValidator>();
builder.Services.AddKeyedScoped<ICommonService<BeerDto, BeerInsertDto, BeerUpdateDto>, BeerService>("beerService");
builder.Services.AddScoped<IRepository<Beer>, BeerRepository>();

builder.Services.AddScoped<IValidator<BrandInsertDto>, BrandInsertValidator>();
builder.Services.AddScoped<IValidator<BrandUpdateDto>, BrandUpdateValidator>();
builder.Services.AddKeyedScoped<ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>, BrandService>("brandService");
builder.Services.AddScoped<IRepository<Brand>, BrandRepository>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();