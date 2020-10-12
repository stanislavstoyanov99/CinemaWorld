namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.News;
    using CinemaWorld.Models.ViewModels.News;
    using CinemaWorld.Models.ViewModels.NewsComments;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class NewsServiceTests : IDisposable, IClassFixture<Configuration>
    {
        private const string TestImageUrl = "https://someurl.com";
        private const string TestImagePath = "Test.jpg";
        private const string TestImageContentType = "image/jpg";

        private readonly INewsService newsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;

        private EfDeletableEntityRepository<CinemaWorldUser> usersRepository;
        private EfDeletableEntityRepository<News> newsRepository;
        private SqliteConnection connection;

        private News firstNews;
        private CinemaWorldUser user;

        public NewsServiceTests(Configuration configuration)
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            Account account = new Account(
                configuration.ConfigurationRoot["Cloudinary:AppName"],
                configuration.ConfigurationRoot["Cloudinary:AppKey"],
                configuration.ConfigurationRoot["Cloudinary:AppSecret"]);

            this.cloudinary = new Cloudinary(account);
            this.cloudinaryService = new CloudinaryService(this.cloudinary);
            this.newsService = new NewsService(this.newsRepository, this.cloudinaryService);
        }

        [Fact]
        public async Task TestAddingNews()
        {
            AllNewsListingViewModel allNewsViewModel;

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var model = new NewsCreateInputModel
                {
                    Title = "Title",
                    Description = "Information about Avatar movies",
                    ShortDescription = "Just short description",
                    Image = file,
                };

                await this.SeedUsers();
                allNewsViewModel = await this.newsService.CreateAsync(model, this.user.Id);
            }

            await this.cloudinaryService.DeleteImage(this.cloudinary, allNewsViewModel.Title + Suffixes.NewsSuffix);

            var count = await this.newsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfAddingNewsThrowsArgumentException()
        {
            this.SeedDatabase();

            NewsCreateInputModel news;
            ArgumentException exception;
            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                news = new NewsCreateInputModel
                {
                    Title = "First news title",
                    Description = "First news description",
                    ShortDescription = "First news short description",
                    Image = file,
                };

                exception = await Assert
                    .ThrowsAsync<ArgumentException>(async () => await this.newsService.CreateAsync(news, this.user.Id));
            }

            await this.cloudinaryService.DeleteImage(this.cloudinary, news.Title + Suffixes.NewsSuffix);
            Assert.Equal(string.Format(ExceptionMessages.NewsAlreadyExists, news.Title, news.Description), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingNewsReturnsViewModel()
        {
            await this.SeedUsers();

            NewsCreateInputModel news;
            AllNewsListingViewModel viewModel;
            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                news = new NewsCreateInputModel
                {
                    Title = "Random news title",
                    Description = "Random news description",
                    ShortDescription = "Random short description",
                    Image = file,
                };

                viewModel = await this.newsService.CreateAsync(news, this.user.Id);
            }

            await this.cloudinaryService.DeleteImage(this.cloudinary, news.Title + Suffixes.NewsSuffix);

            var dbEntry = await this.newsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Title, viewModel.Title);
            Assert.Equal(dbEntry.Description, viewModel.Description);
            Assert.Equal(dbEntry.ShortDescription, viewModel.ShortDescription);
            Assert.Equal(dbEntry.ImagePath, viewModel.ImagePath);
        }

        [Fact]
        public async Task CheckIfDeletingNewsWorksCorrectly()
        {
            this.SeedDatabase();

            await this.newsService.DeleteByIdAsync(this.firstNews.Id);

            var count = await this.newsRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingNewsReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.newsService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.NewsNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task EditAsyncEditsNewsWhenImageStaysTheSame()
        {
            this.SeedDatabase();

            var newsEditViewModel = new NewsEditViewModel
            {
                Id = this.firstNews.Id,
                Title = "Changed News title",
                Description = "Changed News description",
                ShortDescription = "Changed Short description",
                Image = null,
            };

            await this.newsService.EditAsync(newsEditViewModel, this.user.Id);

            Assert.Equal(newsEditViewModel.Title, this.firstNews.Title);
            Assert.Equal(newsEditViewModel.Description, this.firstNews.Description);
            Assert.Equal(newsEditViewModel.ShortDescription, this.firstNews.ShortDescription);
        }

        [Fact]
        public async Task CheckIfEditAsyncEditsNewsImage()
        {
            this.SeedDatabase();

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var newsEditViewModel = new NewsEditViewModel()
                {
                    Id = this.firstNews.Id,
                    Title = this.firstNews.Title,
                    Description = this.firstNews.Description,
                    ShortDescription = this.firstNews.Description,
                    Image = file,
                };

                await this.newsService.EditAsync(newsEditViewModel, this.user.Id);

                await this.cloudinaryService.DeleteImage(this.cloudinary, newsEditViewModel.Title + Suffixes.NewsSuffix);
            }

            var news = await this.newsRepository.All().FirstAsync();
            Assert.NotEqual(TestImageUrl, news.ImagePath);
        }

        [Fact]
        public async Task CheckIfEditingNewsReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var newsEditViewModel = new NewsEditViewModel
            {
                Id = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.newsService.EditAsync(newsEditViewModel, this.user.Id));
            Assert.Equal(string.Format(ExceptionMessages.NewsNotFound, newsEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllNewsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.newsService.GetAllNewsAsync<NewsDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetTopNewsAsyncWorksCorrectlyWhenThereAreZeroTopNews()
        {
            this.SeedDatabase();

            var result = await this.newsService.GetTopNewsAsync<NewsDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfGetTopNewsAsyncWorksCorrectlyWhenThereAreTopNews()
        {
            this.SeedDatabase();

            var result = await this.newsService.GetTopNewsAsync<NewsDetailsViewModel>(1);

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetNewsViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new NewsDetailsViewModel
            {
                Id = this.firstNews.Id,
                Title = this.firstNews.Title,
                CreatedOn = this.firstNews.CreatedOn,
                Description = this.firstNews.Description,
                UserUserName = this.firstNews.User.UserName,
                ImagePath = this.firstNews.ImagePath,
                NewsComments = new HashSet<PostNewsCommentViewModel>(),
                ViewsCounter = this.firstNews.ViewsCounter,
            };

            expectedModel.CreatedOn = expectedModel.CreatedOn.ToLocalTime();

            var viewModel = await this.newsService.GetViewModelByIdAsync<NewsDetailsViewModel>(this.firstNews.Id);
            viewModel.CreatedOn = viewModel.CreatedOn.ToLocalTime();

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () =>
                    await this.newsService.GetViewModelByIdAsync<NewsDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.NewsNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllNewsAsQueryeableWorksCorrectly()
        {
            this.SeedDatabase();

            var result = this.newsService.GetAllNewsAsQueryeable<NewsDetailsViewModel>();

            var count = await result.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetUpdatedNewsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var model = new News
            {
                Title = "Updated news title",
                Description = "Updated news description",
                ShortDescription = "Updated news short description",
                ImagePath = TestImageUrl,
                UserId = this.user.Id,
                ViewsCounter = 30,
                IsUpdated = true,
            };

            await this.newsRepository.AddAsync(model);
            await this.newsRepository.SaveChangesAsync();

            var result = await this.newsService.GetUpdatedNewsAsync<NewsDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfSetViewsCounterWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new NewsDetailsViewModel
            {
                Id = this.firstNews.Id,
                Title = this.firstNews.Title,
                CreatedOn = this.firstNews.CreatedOn,
                Description = this.firstNews.Description,
                UserUserName = this.firstNews.User.UserName,
                ImagePath = this.firstNews.ImagePath,
                ViewsCounter = this.firstNews.ViewsCounter,
                NewsComments = new HashSet<PostNewsCommentViewModel>(),
            };

            var viewModel = await this.newsService.SetViewsCounter(this.firstNews.Id);

            Assert.NotEqual(expectedModel.ViewsCounter, viewModel.ViewsCounter);
        }

        [Fact]
        public async Task CheckIfSetViewsCounterThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () =>
                    await this.newsService.SetViewsCounter(3));
            Assert.Equal(string.Format(ExceptionMessages.NewsNotFound, 3), exception.Message);
        }

        public void Dispose()
        {
            this.connection.Close();
            this.connection.Dispose();
        }

        private void InitializeDatabaseAndRepositories()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
            var options = new DbContextOptionsBuilder<CinemaWorldDbContext>().UseSqlite(this.connection);
            var dbContext = new CinemaWorldDbContext(options.Options);

            dbContext.Database.EnsureCreated();

            this.newsRepository = new EfDeletableEntityRepository<News>(dbContext);
            this.usersRepository = new EfDeletableEntityRepository<CinemaWorldUser>(dbContext);
        }

        private void InitializeFields()
        {
            this.user = new CinemaWorldUser
            {
                Id = "1",
                Gender = Gender.Male,
                UserName = "pesho123",
                FullName = "Pesho Peshov",
                Email = "test_email@gmail.com",
                PasswordHash = "123456",
            };

            this.firstNews = new News
            {
                Title = "First news title",
                Description = "First news description",
                ShortDescription = "First news short description",
                ImagePath = TestImageUrl,
                UserId = this.user.Id,
                ViewsCounter = 50,
                IsUpdated = false,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedNews();
        }

        private async Task SeedNews()
        {
            await this.newsRepository.AddAsync(this.firstNews);

            await this.newsRepository.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.user);

            await this.usersRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
