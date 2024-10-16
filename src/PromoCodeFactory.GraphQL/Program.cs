using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.GraphQL;
using PromoCodeFactory.GraphQL.Domain;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Execution.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();
builder.Services.AddScoped<ICustomer, CustomerService>();

builder.Services.AddDbContext<PromoCodeFactoryDataContext>(x =>
{
	x.UseSqlite("Filename=PromoCodeFactoryDb.sqlite");
});

// Настраиваем HotChocolate GraphQL
builder.Services
	.AddGraphQLServer()
	.AddQueryType<CustomerQueries>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGraphQL("/graphql");

app.UsePlayground(new PlaygroundOptions
{
	QueryPath = "/graphql",
	Path = "/playground"
});

// Инициализация базы данных
void SeedDatabase()
{
	using (var scope = app.Services.CreateScope())
	{
		var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
		dbInitializer.InitializeDb();
	}
}

SeedDatabase();
app.Run();
