﻿using System;
using System.Collections.Generic;

namespace MusiciansAPP.Domain;

public class Artist
{
    public Artist()
    {
        Tracks = new List<Track>();
        Albums = new List<Album>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Biography { get; set; }
    public IEnumerable<Track> Tracks { get; set; }
    public IEnumerable<Album> Albums { get; set; }
}