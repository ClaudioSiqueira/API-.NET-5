using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data
{
    public class DataContext : DbContext  // Essa classe é a representação do meu DB
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        // Representação das tabelas
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }


    }
}