using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//builder.Services.AddScoped(typeof(IRepository<Employee>), (x) =>
//    new InMemoryRepository<Employee>(FakeDataFactory.Employees));
//builder.Services.AddScoped(typeof(IRepository<Role>), (x) =>
//    new InMemoryRepository<Role>(FakeDataFactory.Roles));
//builder.Services.AddScoped(typeof(IRepository<Customer>), (x) =>
//    new InMemoryRepository<Customer>(FakeDataFactory.Customers));
//builder.Services.AddScoped(typeof(IRepository<Preference>), (x) =>
//    new InMemoryRepository<Preference>(FakeDataFactory.Preferences));

//builder.Services.AddDbContext<PromoCodeFactoryDataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<PromoCodeFactoryDataContext>(options =>
           options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IRepository<Employee>, EfRepository<Employee>>();
builder.Services.AddScoped<IRepository<Role>, EfRepository<Role>>();
builder.Services.AddScoped<IRepository<EmployeeRole>, EfRepository<EmployeeRole>>();
builder.Services.AddScoped<IRepository<Customer>, EfRepository<Customer>>();
builder.Services.AddScoped<IRepository<Preference>, EfRepository<Preference>>();
builder.Services.AddScoped<IRepository<PromoCode>, EfRepository<PromoCode>>();
builder.Services.AddScoped<IRepository<CustomerPreference>, EfRepository<CustomerPreference>>();
builder.Services.AddScoped<IRepository<Partner>, EfRepository<Partner>>();
builder.Services.AddScoped<IRepository<PartnerPromoCodeLimit>, EfRepository<PartnerPromoCodeLimit>>();


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


app.Run();
