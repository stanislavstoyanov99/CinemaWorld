namespace CinemaWorld.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.Privacy;

    public class Privacy : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(ContentPageMaxLength)]
        public string PageContent { get; set; }
    }
}
