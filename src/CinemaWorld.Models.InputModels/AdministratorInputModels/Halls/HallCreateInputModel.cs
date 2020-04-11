namespace CinemaWorld.Models.InputModels.AdministratorInputModels.Halls
{
    using System.ComponentModel.DataAnnotations;

    using static CinemaWorld.Models.Common.ModelValidation;

    public class HallCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Category { get; set; }
    }
}
