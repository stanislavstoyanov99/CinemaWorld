namespace CinemaWorld.Models.InputModels.AdministratorInputModels.MovieProjections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Models.ViewModels.Movies;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Cinema;
    using static CinemaWorld.Models.Common.ModelValidation.Hall;
    using static CinemaWorld.Models.Common.ModelValidation.Movie;

    using Cinema = CinemaWorld.Data.Models.Cinema;
    using Hall = CinemaWorld.Data.Models.Hall;
    using Movie = CinemaWorld.Data.Models.Movie;

    public class MovieProjectionCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = nameof(Movie))]
        [Range(1, int.MaxValue, ErrorMessage = MovieIdError)]
        public int MovieId { get; set; }

        public IEnumerable<MovieDetailsViewModel> Movies { get; set; }

        [Display(Name = nameof(Hall))]
        [Range(1, int.MaxValue, ErrorMessage = HallIdError)]
        public int HallId { get; set; }

        public IEnumerable<HallDetailsViewModel> Halls { get; set; }

        [Display(Name = nameof(Cinema))]
        [Range(1, int.MaxValue, ErrorMessage = CinemaIdError)]
        public int CinemaId { get; set; }

        public IEnumerable<CinemaDetailsViewModel> Cinemas { get; set; }
    }
}
