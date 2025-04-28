namespace Projet_Restaurant.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Relation avec les détails de commande
        public ICollection<OrderDetail> OrderDetails { get; set; }
       // public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
