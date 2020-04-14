namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.News;
    using CinemaWorld.Models.ViewModels.News;

    public interface INewsService : IBaseDataService
    {
        Task<AllNewsListingViewModel> CreateAsync(NewsCreateInputModel newsCreateInputModel, string userId);

        Task EditAsync(NewsEditViewModel newsEditViewModel, string userId);

        Task<IEnumerable<TEntity>> GetAllNewsAsync<TEntity>();

        IQueryable<TViewModel> GetAllNewsAsQueryeable<TViewModel>();

        Task<IEnumerable<TViewModel>> GetTopNewsAsync<TViewModel>(int count = 0);

        Task<IEnumerable<TViewModel>> GetUpdatedNewsAsync<TViewModel>();

        Task<NewsDetailsViewModel> SetViewsCounter(int id);
    }
}
