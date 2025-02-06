using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(
        "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Joakim\\Downloads\\C#Projects\\DataStorage_Assignment\\Data\\Databases\\local_db.mdf\";Integrated Security=True;Connect Timeout=30"
    );
});
serviceCollection.AddScoped<IUserService, UserService>();

var serviceProvider = serviceCollection.BuildServiceProvider();
