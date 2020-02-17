namespace CinemaWorld.Data.Models
{
    using System;

    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class Seat : BaseModel<int>, IDeletableEntity
    {
        public int HallId { get; set; }

        public virtual Hall Hall { get; set; }

        public SeatCategory Category { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
