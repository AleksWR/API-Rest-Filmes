using API_Filmes.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Filmes.Data
{
    public class EFContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<Filme> Filmes { get; set; }

        public EFContext(IConfiguration configuration, DbContextOptions options): base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("BancoAPI"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
