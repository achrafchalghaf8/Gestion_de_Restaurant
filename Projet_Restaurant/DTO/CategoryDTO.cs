namespace Projet_Restaurant.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }


        public string? Name { get; set; }


    }

    public class ArticleDTO
    {
        public int ArticleID { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }// ID de la catégorie pour lier l'article à une catégorie
        public string? CategoryName  {get; set; }
    }

    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Ajouter une liste des détails de commande avec un DTO (OrderDetailDTO)
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }


    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }




}
