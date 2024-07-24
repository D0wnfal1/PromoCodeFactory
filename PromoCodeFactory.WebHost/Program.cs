using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<Employee>), (x) =>
    new InMemoryRepository<Employee>(FakeDataFactory.Employees));
builder.Services.AddScoped(typeof(IRepository<Role>), (x) =>
    new InMemoryRepository<Role>(FakeDataFactory.Roles));

builder.Services.AddOpenApiDocument(options => 
{
    options.Title = "PromoCode Factory API Doc";
    options.Version = "1.0";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}
else
{
    app.UseHsts();
}

app.UseOpenApi();
app.UseSwaggerUi(x =>
{
    x.DocExpansion = "list";
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();


app.MapControllers();


app.Run();
