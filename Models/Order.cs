namespace BakeryApp.Models
{
    public class Order
    {
        // table
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Новый";
        public decimal TotalSum { get; set; }

        // for order delivery
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = "Самовывоз";
        public string Address { get; set; } = string.Empty;

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}