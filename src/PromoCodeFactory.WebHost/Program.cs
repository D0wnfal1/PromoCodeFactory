using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();

builder.Services.AddDbContext<PromoCodeFactoryDataContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddDbContext<PromoCodeFactoryDataContext>(options =>
//options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registration of repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));



builder.Services.AddOpenApiDocument(options => 
{
    options.Title = "PromoCode Factory API Doc";
    options.Version = "1.0";
});

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<PromoCodeFactoryDataContext>();

//    context.Database.EnsureDeleted();
//    context.Database.EnsureCreated();

//    context.Employees.AddRange(FakeDataFactory.Employees);
//    context.Roles.AddRange(FakeDataFactory.Roles);
//    context.Customers.AddRange(FakeDataFactory.Customers);
//    context.Preferences.AddRange(FakeDataFactory.Preferences);
//    context.EmployeeRoles.AddRange(FakeDataFactory.EmployeeRoles);
//    context.Partners.AddRange(FakeDataFactory.Partners);

//    context.SaveChanges();
//}

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