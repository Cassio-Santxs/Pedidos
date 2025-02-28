using System.ComponentModel.DataAnnotations;

namespace PedidoApi.Models {
    public class Pedido {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Cliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<ItemPedido> Itens { get; set; } = new();
        public decimal Total { get; set; }
        public string Status { get; set; } = "PENDENTE";
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    }
}
