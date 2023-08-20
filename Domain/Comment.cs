namespace Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid FindID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
