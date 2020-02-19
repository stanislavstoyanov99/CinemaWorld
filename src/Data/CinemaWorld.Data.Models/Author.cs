namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common;
    using CinemaWorld.Data.Common.Models;

    public class Author : BaseModel<int>, IDeletableEntity
    {
        public Author()
        {
            this.Reviews = new HashSet<ReviewAuthor>();
        }

        [Required]
        [MaxLength(DataValidation.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataValidation.NameMaxLength)]
        public string LastName { get; set; }

        public string Email { get; set; }

        public virtual ICollection<ReviewAuthor> Reviews { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
