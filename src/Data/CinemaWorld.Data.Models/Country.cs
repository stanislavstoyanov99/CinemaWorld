namespace CinemaWorld.Data.Models
{
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Country : BaseModel<int>
    {
        public Country()
        {
            this.MovieCountries = new HashSet<MovieCountry>();
        }

        public string Name { get; set; }

        public ICollection<MovieCountry> MovieCountries { get; set; }
    }
}
