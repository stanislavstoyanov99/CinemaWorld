namespace CinemaWorld.Models.ViewModels.Seats
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation.Seat;

    public class SeatDetailsViewModel : IMapFrom<Seat>
    {
        public int Id { get; set; }

        [Display(Name = NumberDisplayName)]
        public int Number { get; set; }

        [Display(Name = RowNumberDisplayName)]
        public int RowNumber { get; set; }

        [Display(Name = HallIdDisplayName)]
        public HallDetailsViewModel Hall { get; set; }

        public SeatCategory Category { get; set; }

        [Display(Name = IsAvailableDisplayName)]
        public bool IsAvailable { get; set; }

        [Display(Name = IsSoldDisplayName)]
        public bool IsSold { get; set; }
    }
}
