namespace CinemaWorld.Models.ViewModels.Comments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using Ganss.XSS;

    using static CinemaWorld.Models.Common.ModelValidation;

    public class PostCommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError, AllowEmptyStrings = false)]
        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }

        public string UserFullName { get; set; }
    }
}
