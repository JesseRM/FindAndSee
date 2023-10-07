using Microsoft.AspNetCore.Http;

namespace Application.Finds
{
    public class FindCreateDto
    {
        public Guid FindId { get; set; }
        public string Title { get; set; }
        public IFormFile ImageFile { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Description { get; set; }
        public Guid AuthorObjectId { get; set; }
    }
}
