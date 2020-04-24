namespace CinemaWorld.Models.InputModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCommentInputModel
    {
        public int MovieId { get; set; }

        public int ParentId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
