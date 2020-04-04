namespace CinemaWorld.Models.ViewModels.Genres
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class NavbarGenreViewModel : IMapFrom<Genre>
    {
        public string Name { get; set; }
    }
}
