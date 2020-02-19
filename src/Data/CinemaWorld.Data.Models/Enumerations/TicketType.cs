namespace CinemaWorld.Data.Models.Enumerations
{
    using System.ComponentModel.DataAnnotations;

    public enum TicketType
    {
        Regular = 1,
        [Display(Name = "For Children")]
        ForChildren = 2,
        Student = 3,
        Retired = 4,
    }
}
