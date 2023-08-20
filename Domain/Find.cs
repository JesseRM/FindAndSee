namespace Domain
{
    public class Find
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Double Longitude { get; set; }
        public Double Latitude { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool Rejected { get; set; } = false;
    }
}
