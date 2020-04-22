namespace CinemaWorld.Models.ViewModels.Privacy
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Privacy;

    using Privacy = CinemaWorld.Data.Models.Privacy;

    public class PrivacyEditViewModel : IMapFrom<Privacy>
    {
        public int Id { get; set; }

        [Display(Name = PageContentDisplayName)]
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(PageContentMaxLength, MinimumLength = PageContentMinLength, ErrorMessage = PageContentLengthError)]
        public string PageContent { get; set; }
    }
}
