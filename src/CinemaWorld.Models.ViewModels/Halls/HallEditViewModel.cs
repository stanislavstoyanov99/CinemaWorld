namespace CinemaWorld.Models.ViewModels.Halls
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation;

    public class HallEditViewModel : IMapFrom<Hall>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Category { get; set; }
    }
}
