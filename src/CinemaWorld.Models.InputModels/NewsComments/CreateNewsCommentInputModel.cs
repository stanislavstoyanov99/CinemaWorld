namespace CinemaWorld.Models.InputModels.NewsComments
{
    using System.ComponentModel.DataAnnotations;

    using static CinemaWorld.Models.Common.ModelValidation;

    public class CreateNewsCommentInputModel
    {
        public int NewsId { get; set; }

        public int ParentId { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Content { get; set; }
    }
}
