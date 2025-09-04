using C2INIS_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public class AppDbContext : DbContext
    {
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=petanimaldb;Integrated Security=True;MultipleActiveResultSets=true";

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Food> Keepers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public bool wasCreated { get; private set; }

        public AppDbContext()
        {
            wasCreated = Database.EnsureCreated();
        }

        public void Reset()
        {
            Animals.RemoveRange(Animals);
            Keepers.RemoveRange(Keepers);
            Activities.RemoveRange(Activities);
            SaveChanges();
            Database.EnsureDeleted();
            Database.EnsureCreated();
            /*Animals.RemoveRange();
            Keepers.RemoveRange();
            Activities.RemoveRange();*/
            SaveChanges();
            wasCreated = false;
        }

        public void Delete()
        {
            Database.EnsureDeleted();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
