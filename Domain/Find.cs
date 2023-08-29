namespace Domain
{
    public class Find
    {
        public Guid FindId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Description { get; set; }
        public Guid AuthorObjectId { get; set; }
        public bool IsApproved { get; set; }
        public bool Rejected { get; set; }
    }
}
