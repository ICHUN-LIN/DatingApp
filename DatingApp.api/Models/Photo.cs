using System;

namespace DatingApp.api.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsMain { get; set; }

        public int UserId { get; set; }

        //Nevigate property of Photo
        public User User { get; set; }
    }
}