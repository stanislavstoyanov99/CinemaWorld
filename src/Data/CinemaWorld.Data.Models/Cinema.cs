namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.Cinema;

    public class Cinema : BaseModel<int>, IDeletableEntity
    {
        public Cinema()
        {
            this.Projections = new HashSet<MovieProjection>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        public virtual ICollection<MovieProjection> Projections { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
