namespace CinemaWorld.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IRatingsService
    {
        Task VoteAsync(int movieId, string userId, int rating);

        Task<int> GetStarRatingsAsync(int movieId);

        Task<DateTime> GetNextVoteDateAsync(int movieId, string userId);
    }
}
