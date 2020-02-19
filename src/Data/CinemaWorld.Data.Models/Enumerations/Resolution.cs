namespace CinemaWorld.Data.Models.Enumerations
{
    using System.ComponentModel.DataAnnotations;

    public enum Resolution
    {
        Unknown = 0,
        HD = 1,
        SD = 2,
        [Display(Name = "4K")]
        FourK = 3,
    }
}
