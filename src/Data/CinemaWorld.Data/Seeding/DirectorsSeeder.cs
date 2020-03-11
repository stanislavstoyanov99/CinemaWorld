namespace CinemaWorld.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Models;

    public class DirectorsSeeder : ISeeder
    {
        public async Task SeedAsync(CinemaWorldDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Directors.Any())
            {
                return;
            }

            var directors = new List<(string FirstName, string LastName)>
            {
                ("Steven", "Spielberg"),
                ("Martin", "Scorsese"),
                ("Quentin", "Tarantino"),
                ("James", "Cameron"),
                ("Woody", "Allen"),
            };

            foreach (var director in directors)
            {
                await dbContext.Directors.AddAsync(new Director
                {
                    FirstName = director.FirstName,
                    LastName = director.LastName,
                });
            }
        }
    }
}
