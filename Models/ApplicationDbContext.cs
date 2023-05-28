using Microsoft.EntityFrameworkCore;

namespace api_ejemplar.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public ApplicationDbContext(/*string connectionString*/) : base(GetDbContextOptions(/*connectionString*/))
        {
        }

        private static DbContextOptions<ApplicationDbContext> GetDbContextOptions(/*string connectionString*/)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server = tcp:bdpedidos.database.windows.net, 1433; Initial Catalog = pedidos; Persist Security Info = False; User ID = mauro; Password = Belgrano1000; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;")
                .Options;
        }

        public DbSet<USUARIO> USUARIO { get; set; }

        public DbSet<ERRORES> ERRORES { get; set; }
        public DbSet<LOG_TOKEN> LOG_TOKEN { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura tus entidades y relaciones aquí
            //modelBuilder.Entity<Usuario>().HasMany(u => u.Productos).WithOne(p => p.Usuario);

            base.OnModelCreating(modelBuilder);
        }
    }
}
