namespace CinemaWorld.Models.ViewModels.Movies
{
    using System;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Services.Mapping;

    public class MovieViewModel : IMapFrom<Movie>
    {
        public string Name { get; set; }

        public DateTime DateOfRelease { get; set; }

        public string Resolution { get; set; }

        public decimal Rating { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public CinemaCategory CinemaCategory { get; set; }

        public string TrailerPath { get; set; }

        public string CoverPath { get; set; }

        public string IMDBLink { get; set; }

        public int Length { get; set; }
    }
}
