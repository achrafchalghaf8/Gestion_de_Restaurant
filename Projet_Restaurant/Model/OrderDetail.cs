using Projet_Restaurant.Model;

public class OrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int ArticleId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // Marquez ces relations comme facultatives
    public Article? Article { get; set; }
    public Order? Order { get; set; }
}
