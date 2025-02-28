using Microsoft.EntityFrameworkCore;
using PedidoApi.Models;

namespace PedidoApi.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemPedido>()
                .HasOne<Pedido>()
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId);
        }
    }
}
