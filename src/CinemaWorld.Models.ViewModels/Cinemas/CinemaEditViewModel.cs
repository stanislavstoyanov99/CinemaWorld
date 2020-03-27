namespace CinemaWorld.Models.ViewModels.Cinemas
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Cinema;

    using Cinema = CinemaWorld.Data.Models.Cinema;

    public class CinemaEditViewModel : IMapFrom<Cinema>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = AddressLengthError)]
        public string Address { get; set; }
    }
}
