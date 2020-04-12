namespace CinemaWorld.Models.ViewModels.Halls
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Hall;

    using Hall = CinemaWorld.Data.Models.Hall;

    public class HallEditViewModel : IMapFrom<Hall>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Category { get; set; }

        [Range(CapacityMinLength, CapacityMaxLength)]
        public int Capacity { get; set; }
    }
}
