using System;

namespace MusiciansAPP.API.Resources
{
    public class ArtistDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}
