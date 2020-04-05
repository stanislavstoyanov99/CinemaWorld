namespace CinemaWorld.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;

    public class RatingsService : IRatingsService
    {
        private readonly IDeletableEntityRepository<StarRating> starRatingsRepository;

        public RatingsService(IDeletableEntityRepository<StarRating> starRatingsRepository)
        {
            this.starRatingsRepository = starRatingsRepository;
        }

        public int GetStarRatings(int movieId)
        {
            var starRatings = this.starRatingsRepository
                .All()
                .Where(x => x.MovieId == movieId)
                .Sum(x => x.Rate);

            return starRatings;
        }

        public async Task VoteAsync(int movieId, string userId, int rating)
        {
            var starRating = this.starRatingsRepository
                .All()
                .FirstOrDefault(x => x.MovieId == movieId && x.UserId == userId);

            if (starRating != null)
            {
                if (starRating.CreatedOn.Date == DateTime.UtcNow.Date)
                {
                    throw new ArgumentException(
                        string.Format(ExceptionMessages.AlreadySendVote, DateTime.UtcNow.AddDays(1)));
                }

                starRating.Rate += rating;
            }
            else
            {
                starRating = new StarRating
                {
                    MovieId = movieId,
                    UserId = userId,
                    Rate = rating,
                };

                await this.starRatingsRepository.AddAsync(starRating);
            }

            await this.starRatingsRepository.SaveChangesAsync();
        }
    }
}
