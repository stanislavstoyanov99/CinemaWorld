namespace CinemaWorld.Models.ViewModels.Cinemas
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation.Cinema;

    public class CinemaDetailsViewModel : IMapFrom<Cinema>
    {
        public int Id { get; set; }

        [Display(Name = NameDisplayName)]
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
