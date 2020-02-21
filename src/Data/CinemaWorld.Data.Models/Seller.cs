﻿namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common;
    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class Seller : BaseModel<int>, IDeletableEntity
    {
        public Seller()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        [Required]
        [MaxLength(DataValidation.FullNameMaxLength)]
        public string FullName { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public int? PhoneNumber { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}