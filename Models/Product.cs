namespace BakeryApp.Models
{
    public class Product
    {
        // table
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "/images/default.svg";
        public string Category { get; set; } = string.Empty;
        public bool IsRecommended { get; set; }

        // extra info
        public string Calories { get; set; } = string.Empty;
        public string Composition { get; set; } = string.Empty;
    }
}