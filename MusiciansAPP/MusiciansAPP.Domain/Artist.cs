using System;

namespace MusiciansAPP.Domain
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}
