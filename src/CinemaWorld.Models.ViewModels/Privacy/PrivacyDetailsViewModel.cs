namespace CinemaWorld.Models.ViewModels.Privacy
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;
    using Ganss.XSS;

    using static CinemaWorld.Models.Common.ModelValidation.Privacy;

    public class PrivacyDetailsViewModel : IMapFrom<Privacy>
    {
        public int Id { get; set; }

        [Display(Name = PageContentDisplayName)]
        public string PageContent { get; set; }

        public string SanitizedPageContent => new HtmlSanitizer().Sanitize(this.PageContent);
    }
}
