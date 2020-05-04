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

        public async Task<AllNewsListingViewModel> CreateAsync(NewsCreateInputModel newsCreateInputModel, string userId)
        {
            var imageUrl = await this.cloudinaryService
               .UploadAsync(newsCreateInputModel.Image, newsCreateInputModel.Title + Suffixes.NewsSuffix);

            var news = new News
            {
                Title = newsCreateInputModel.Title,
                Description = newsCreateInputModel.Description,
                ShortDescription = newsCreateInputModel.ShortDescription,
                ImagePath = imageUrl,
                UserId = userId,
            };

            bool doesNewsExist = await this.newsRepository
                .All()
                .AnyAsync(x => x.Title == news.Title && x.Description == news.Description);
            if (doesNewsExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.NewsAlreadyExists, news.Title, news.Description));
            }

            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<AllNewsListingViewModel>(news.Id);

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
            news.ShortDescription = newsEditViewModel.ShortDescription;
            news.UserId = userId;
            news.ModifiedOn = DateTime.UtcNow;
            news.IsUpdated = true;

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

        public async Task<IEnumerable<TViewModel>> GetTopNewsAsync<TViewModel>(int count = 0)
        {
            var topNews = await this.newsRepository
                 .All()
                 .Take(count)
                 .OrderByDescending(x => x.ViewsCounter)
                 .To<TViewModel>()
                 .ToListAsync();

            return topNews;
        }

        public async Task<IEnumerable<TViewModel>> GetUpdatedNewsAsync<TViewModel>()
        {
            var updatedNews = await this.newsRepository
                 .All()
                 .OrderByDescending(x => x.ModifiedOn)
                 .Where(x => x.IsUpdated == true)
                 .To<TViewModel>()
                 .ToListAsync();

            return updatedNews;
        }

        public async Task<NewsDetailsViewModel> SetViewsCounter(int id)
        {
            var news = await this.newsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news == null)
            {
                throw new NullReferenceException(
                       string.Format(ExceptionMessages.NewsNotFound, id));
            }

            news.ViewsCounter++;
            this.newsRepository.Update(news);
            await this.newsRepository.SaveChangesAsync();

            var viewModel = await this.newsRepository
                .All()
                .Where(x => x.Id == id)
                .To<NewsDetailsViewModel>()
                .FirstOrDefaultAsync(x => x.Id == id);

            return viewModel;
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

        public IQueryable<TViewModel> GetAllNewsAsQueryeable<TViewModel>()
        {
            var news = this.newsRepository
                .All()
                .To<TViewModel>();

            return news;
        }
    }
}
