using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Contexts
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=local_db;AttachDbFilename=C:\\Users\\Joakim\\Downloads\\C#Projects\\DataStorage_Assignment\\Data\\Databases\\local_db.mdf;Integrated Security=True;Connect Timeout=30"
            );

            return new DataContext(optionsBuilder.Options);
        }
    }
}
