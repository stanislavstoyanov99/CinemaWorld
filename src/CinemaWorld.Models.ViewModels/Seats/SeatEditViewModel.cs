namespace CinemaWorld.Models.ViewModels.Seats
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Hall;
    using static CinemaWorld.Models.Common.ModelValidation.Seat;

    using Seat = CinemaWorld.Data.Models.Seat;

    public class SeatEditViewModel : IMapFrom<Seat>
    {
        public int Id { get; set; }

        [Display(Name = NumberDisplayName)]
        [Range(1, int.MaxValue, ErrorMessage = SeatNumberIdError)]
        public int Number { get; set; }

        [Display(Name = RowNumberDisplayName)]
        [Range(1, int.MaxValue, ErrorMessage = RowNumberIdError)]
        public int RowNumber { get; set; }

        [Display(Name = nameof(Hall))]
        [Range(1, int.MaxValue, ErrorMessage = HallIdError)]
        public int HallId { get; set; }

        public IEnumerable<HallDetailsViewModel> Halls { get; set; }

        public string Category { get; set; }
    }
}
