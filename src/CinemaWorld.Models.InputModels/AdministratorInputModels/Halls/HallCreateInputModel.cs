namespace CinemaWorld.Models.InputModels.AdministratorInputModels.Halls
{
    using System.ComponentModel.DataAnnotations;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Hall;

    public class HallCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Category { get; set; }

        [Range(CapacityMinLength, CapacityMaxLength)]
        public int Capacity { get; set; }
    }
}
