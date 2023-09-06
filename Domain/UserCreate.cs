namespace Domain
{
    public class UserCreate
    {
        public Guid ObjectId { get; set; }
        public string DisplayName { get; set; }
        public bool NewUser { get; set; }
    }
}
