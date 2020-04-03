namespace CinemaWorld.Models.ViewModels.About
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;
    using Ganss.XSS;

    public class FaqDetailsViewModel : IMapFrom<FaqEntry>
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string SanitizedAnswer => new HtmlSanitizer().Sanitize(this.Answer);
    }
}
