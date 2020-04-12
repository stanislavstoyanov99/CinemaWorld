namespace CinemaWorld.Models.ViewModels.MovieProjections
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Mapping;

    public class MovieProjectionDetailsViewModel : IMapFrom<MovieProjection>
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = nameof(Movie))]
        public MovieDetailsViewModel Movie { get; set; }

        [Display(Name = nameof(Hall))]
        public HallDetailsViewModel Hall { get; set; }

        [Display(Name = nameof(Cinema))]
        public CinemaDetailsViewModel Cinema { get; set; }
    }
}
