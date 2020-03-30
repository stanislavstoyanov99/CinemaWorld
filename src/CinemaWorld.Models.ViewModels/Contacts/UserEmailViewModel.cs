namespace CinemaWorld.Models.ViewModels.Contacts
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class UserEmailViewModel : IMapFrom<ContactFormEntry>
    {
        public string Email { get; set; }
    }
}
