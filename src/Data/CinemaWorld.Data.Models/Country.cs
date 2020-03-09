namespace CinemaWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.Country;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.MovieCountries = new HashSet<MovieCountry>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<MovieCountry> MovieCountries { get; set; }
    }
}
