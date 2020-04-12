namespace CinemaWorld.Models.ViewModels.Halls
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Services.Mapping;

    public class HallDetailsViewModel : IMapFrom<Hall>
    {
        public int Id { get; set; }

        public HallCategory Category { get; set; }

        public int Capacity { get; set; }
    }
}
