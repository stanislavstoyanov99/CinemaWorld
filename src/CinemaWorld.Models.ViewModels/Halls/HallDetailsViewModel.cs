namespace CinemaWorld.Models.ViewModels.Halls
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class HallDetailsViewModel : IMapFrom<Hall>
    {
        public int Id { get; set; }

        public string Category { get; set; }
    }
}
