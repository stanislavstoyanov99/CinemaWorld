namespace CinemaWorld.Models.ViewModels.Genres
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation.Genre;

    public class GenreDetailsViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        [Display(Name = NameDisplayName)]
        public string Name { get; set; }
    }
}
