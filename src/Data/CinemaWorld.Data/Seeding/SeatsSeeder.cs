namespace CinemaWorld.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class SeatsSeeder : ISeeder
    {
        private readonly Random random;

        public SeatsSeeder(Random random)
        {
            this.random = random;
        }

        public async Task SeedAsync(CinemaWorldDbContext dbContext, IServiceProvider serviceProvider)
        {
            // Custom seed halls with requested amount of seats
            if (dbContext.Seats.Any())
            {
                return;
            }

            var seats = new List<Seat>();

            for (int row = 1; row <= 3; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    int category = this.random.Next((int)SeatCategory.Normal, (int)SeatCategory.Wheelchair + 1);

                    var seat = new Seat
                    {
                        RowNumber = row,
                        Number = col,
                        HallId = 3,
                        Category = (SeatCategory)category,
                    };

                    seats.Add(seat);
                }
            }

            foreach (var seat in seats)
            {
                await dbContext.Seats.AddAsync(new Seat
                {
                    Number = seat.Number,
                    RowNumber = seat.RowNumber,
                    Category = seat.Category,
                    HallId = seat.HallId,
                    IsAvailable = true,
                });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
