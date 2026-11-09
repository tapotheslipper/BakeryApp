namespace BakeryApp.Models
{
    public class Review
    {
        // table
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        public bool IsPublished { get; set; } = false;
    }
}