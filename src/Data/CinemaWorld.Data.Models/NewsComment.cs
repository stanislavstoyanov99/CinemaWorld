namespace CinemaWorld.Data.Models
{
    using CinemaWorld.Data.Common.Models;

    public class NewsComment : BaseDeletableModel<int>
    {
        public int NewsId { get; set; }

        public virtual News News { get; set; }

        public int? ParentId { get; set; }

        public virtual NewsComment Parent { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public virtual CinemaWorldUser User { get; set; }
    }
}
