using System;

namespace DatingApp.api.DTOS
{
    public class PhotoDetailedDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool ismain { get; set; }
    }
}