namespace CinemaWorld.Models.ViewModels.Directors
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class DirectorViewModel : IMapFrom<Director>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
