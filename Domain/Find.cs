﻿namespace Domain
{
    public class Find
    {
        public Guid FindId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public Image Image { get; set; }
        public User User { get; set; }
    }
}
