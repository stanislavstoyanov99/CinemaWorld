namespace CinemaWorld.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IBaseDataService
    {
        Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id);

        Task DeleteByIdAsync(int id);
    }
}
