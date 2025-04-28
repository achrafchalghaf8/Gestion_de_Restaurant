namespace Projet_Restaurant.Model
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // Relation avec la catégorie
        public Category? Category { get; set; }
    }
}
