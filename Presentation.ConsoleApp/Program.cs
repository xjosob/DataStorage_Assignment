using Business.Factories;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ConsoleApp.Dialogs;

var serviceCollection = new ServiceCollection();

serviceCollection.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer("");
});
serviceCollection.AddScoped<IUserService, UserService>();
serviceCollection.AddScoped<ICustomerService, CustomerService>();
serviceCollection.AddScoped<IProjectService, ProjectService>();
serviceCollection.AddScoped<IStatusService, StatusService>();
serviceCollection.AddScoped<IProductService, ProductService>();
serviceCollection.AddScoped<ProjectFactory>();
serviceCollection.AddScoped<UserFactory>();
serviceCollection.AddScoped<ProjectRepository>();
serviceCollection.AddScoped<UserRepository>();
serviceCollection.AddScoped<CustomerRepository>();
serviceCollection.AddScoped<UserRepository>();
serviceCollection.AddScoped<StatusTypeRepository>();
serviceCollection.AddScoped<ProductRepository>();
serviceCollection.AddScoped<MenuDialog>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var dialogs = serviceProvider.GetRequiredService<MenuDialog>();
await dialogs.MenuOptions();
