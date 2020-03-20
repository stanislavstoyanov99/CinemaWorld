namespace CinemaWorld.Models.InputModels.AdministratorInputModels.Directors
{
    using System.ComponentModel.DataAnnotations;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Director;

    public class DirectorCreateInputModel
    {
        [Display(Name = FirstNameDisplayName)]
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string FirstName { get; set; }

        [Display(Name = LastNameDisplayName)]
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string LastName { get; set; }
    }
}
