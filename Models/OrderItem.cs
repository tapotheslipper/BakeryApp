namespace BakeryApp.Models
{
    public class OrderItem
    {
        // table
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        // tie
        public Product Product { get; set; } = null!;
    }
}