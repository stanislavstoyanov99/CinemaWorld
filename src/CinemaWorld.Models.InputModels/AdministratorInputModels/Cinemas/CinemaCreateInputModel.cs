namespace CinemaWorld.Models.InputModels.AdministratorInputModels.Cinemas
{
    using System.ComponentModel.DataAnnotations;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Cinema;

    public class CinemaCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = AddressLengthError)]
        public string Address { get; set; }
    }
}
