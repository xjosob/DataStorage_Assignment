using Business.Factories;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ConsoleApp.Dialogs;

var serviceCollection = new ServiceCollection();

serviceCollection.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(
        "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Joakim\\Downloads\\C#Projects\\DataStorage_Assignment\\Data\\Databases\\local_db.mdf;Integrated Security=True;Connect Timeout=30"
    );
});
serviceCollection.AddScoped<IUserService, UserService>();
serviceCollection.AddScoped<ProjectFactory>();
serviceCollection.AddScoped<UserFactory>();
serviceCollection.AddScoped<MenuDialog>();
serviceCollection.AddScoped<ICustomerService, CustomerService>();
serviceCollection.AddScoped<IProjectService, ProjectService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var dialogs = serviceProvider.GetRequiredService<MenuDialog>();
await dialogs.MenuOptions();
