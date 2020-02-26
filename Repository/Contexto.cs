using Microsoft.EntityFrameworkCore;
using ygo_geo_api.Models;

namespace ygo_geo_api.Repository
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        public DbSet<User> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(x => x.Id);
            base.OnModelCreating(builder);
        }
    }
}