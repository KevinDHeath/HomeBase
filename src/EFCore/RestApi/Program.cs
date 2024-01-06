global using Microsoft.EntityFrameworkCore;
global using Common.Core.Classes;
global using Common.Core.Models;
global using EFCore.RestApi.Services;

using Microsoft.OpenApi.Models;
using System.Reflection;
using EFCore.Data;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<SuperHeroService>();
builder.Services.AddScoped<ProvinceService>();
builder.Services.AddScoped<ISOCountryService>();
builder.Services.AddScoped<PostcodeService>();

builder.Services.AddDbContext<EFCoreDbContext>(
	options => options.UseSqlServer( builder.Configuration["ConnectionStrings:CommonData"] ) );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
	options.SwaggerDoc( "v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "EF Core API",
		Description = "An ASP.NET Core Web API for managing Entity Framework resources.",
	} );

	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments( Path.Combine( AppContext.BaseDirectory, xmlFilename ) );
} );

var app = builder.Build();

// Configure the HTTP request pipeline.
//if( app.Environment.IsDevelopment() )
//{
	app.UseSwagger();
	app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();