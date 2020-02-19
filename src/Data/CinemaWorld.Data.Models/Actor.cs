namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common;
    using CinemaWorld.Data.Common.Models;

    public class Actor : BaseModel<int>, IDeletableEntity
    {
        public Actor()
        {
            this.MovieActors = new HashSet<MovieActor>();
        }

        [Required]
        [MaxLength(DataValidation.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataValidation.NameMaxLength)]
        public string LastName { get; set; }

        public virtual ICollection<MovieActor> MovieActors { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
