using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Finds
{
    public class FindUpdateDto
    {
        public Guid FindId { get; set; }
        public string Title { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Description { get; set; }
        public Guid AuthorObjectId { get; set; }
    }
}
