namespace CinemaWorld.Models.ViewModels.Countries
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class CountryDetailsViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        [Display(Name = nameof(Country))]
        public string Name { get; set; }
    }
}
