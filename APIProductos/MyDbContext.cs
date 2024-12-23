using Microsoft.EntityFrameworkCore;
using APIProductos.Models;

namespace APIProductos.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        public DbSet<APIProductos.Models.Productos> Productos { get; set; }
    }
}
