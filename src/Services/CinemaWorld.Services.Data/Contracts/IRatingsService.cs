namespace CinemaWorld.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IRatingsService
    {
        Task VoteAsync(int movieId, string userId, int rating);

        int GetStarRatings(int movieId);
    }
}
