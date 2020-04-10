namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.News;
    using CinemaWorld.Models.ViewModels.News;

    public interface INewsService : IBaseDataService
    {
        Task<NewsDetailsViewModel> CreateAsync(NewsCreateInputModel newsCreateInputModel, string userId);

        Task EditAsync(NewsEditViewModel newsEditViewModel, string userId);

        Task<IEnumerable<TEntity>> GetAllNewsAsync<TEntity>();
    }
}
