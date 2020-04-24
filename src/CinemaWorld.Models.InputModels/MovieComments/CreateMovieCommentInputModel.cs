namespace CinemaWorld.Models.InputModels.MovieComments
{
    using System.ComponentModel.DataAnnotations;

    using static CinemaWorld.Models.Common.ModelValidation;

    public class CreateMovieCommentInputModel
    {
        public int MovieId { get; set; }

        public int ParentId { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Content { get; set; }
    }
}
