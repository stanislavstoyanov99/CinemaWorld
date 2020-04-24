namespace CinemaWorld.Data.Models
{
    using CinemaWorld.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int? ParentId { get; set; }

        public virtual Comment Parent { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public virtual CinemaWorldUser User { get; set; }
    }
}
