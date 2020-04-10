namespace CinemaWorld.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.News;
    using CinemaWorld.Models.ViewModels.News;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly ICloudinaryService cloudinaryService;

        public NewsService(
            IDeletableEntityRepository<News> newsRepository,
            ICloudinaryService cloudinaryService)
        {
            this.newsRepository = newsRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<NewsDetailsViewModel> CreateAsync(NewsCreateInputModel newsCreateInputModel, string userId)
        {
            var imageUrl = await this.cloudinaryService
               .UploadAsync(newsCreateInputModel.Image, newsCreateInputModel.Title + Suffixes.NewsSuffix);

            var news = new News
            {
                Title = newsCreateInputModel.Title,
                Description = newsCreateInputModel.Description,
                ImagePath = imageUrl,
                UserId = userId,
            };

            bool doesNewsExist = await this.newsRepository.All().AnyAsync(x => x.Id == news.Id);
            if (doesNewsExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.NewsAlreadyExists, news.Id));
            }

            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<NewsDetailsViewModel>(news.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var news = this.newsRepository.All().FirstOrDefault(x => x.Id == id);
            if (news == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.NewsNotFound, id));
            }

            news.IsDeleted = true;
            news.DeletedOn = DateTime.UtcNow;
            this.newsRepository.Update(news);
            await this.newsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(NewsEditViewModel newsEditViewModel, string userId)
        {
            var news = this.newsRepository.All().FirstOrDefault(g => g.Id == newsEditViewModel.Id);

            if (news == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.NewsNotFound, newsEditViewModel.Id));
            }

            if (newsEditViewModel.Image != null)
            {
                var newImageUrl = await this.cloudinaryService
                    .UploadAsync(newsEditViewModel.Image, newsEditViewModel.Title + Suffixes.NewsSuffix);
                news.ImagePath = newImageUrl;
            }

            news.Title = newsEditViewModel.Title;
            news.Description = newsEditViewModel.Description;
            news.UserId = userId;
            news.ModifiedOn = DateTime.UtcNow;

            this.newsRepository.Update(news);
            await this.newsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllNewsAsync<TViewModel>()
        {
            var news = await this.newsRepository
                 .All()
                 .OrderByDescending(x => x.CreatedOn)
                 .To<TViewModel>()
                 .ToListAsync();

            return news;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var newsViewModel = await this.newsRepository
                .All()
                .Where(m => m.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (newsViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.NewsNotFound, id));
            }

            return newsViewModel;
        }
    }
}
