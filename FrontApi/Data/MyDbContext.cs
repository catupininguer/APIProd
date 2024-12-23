using Microsoft.EntityFrameworkCore;
using FrontApi.Models;

namespace FrontApi.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Productos> Productos { get; set; }
    }
}
