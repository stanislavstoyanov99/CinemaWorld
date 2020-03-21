namespace CinemaWorld.Models.ViewModels.Countries
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Country;

    using Country = CinemaWorld.Data.Models.Country;

    public class CountryEditViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }
    }
}
