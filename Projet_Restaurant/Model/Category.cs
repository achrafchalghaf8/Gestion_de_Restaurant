namespace Projet_Restaurant.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // Relation avec les Articles
        public ICollection<Article> Articles { get; set; }
    }
}
