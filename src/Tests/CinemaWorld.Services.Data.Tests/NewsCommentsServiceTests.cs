namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.NewsComments;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class NewsCommentsServiceTests : IDisposable
    {
        private const string TestImageUrl = "https://someurl.com";

        private readonly INewsCommentsService newsCommentsService;
        private EfDeletableEntityRepository<NewsComment> newsCommentsRepository;
        private EfDeletableEntityRepository<News> newsRepository;
        private EfDeletableEntityRepository<CinemaWorldUser> usersRepository;
        private SqliteConnection connection;

        private News firstNews;
        private NewsComment firstNewsComment;
        private CinemaWorldUser user;

        public NewsCommentsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.newsCommentsService = new NewsCommentsService(this.newsCommentsRepository);
        }

        [Fact]
        public async Task TestAddingNewsComment()
        {
            await this.SeedUsers();
            await this.SeedNews();

            var newsComment = new CreateNewsCommentInputModel
            {
                NewsId = this.firstNews.Id,
                Content = "Hello, how are you?",
            };

            await this.newsCommentsService.CreateAsync(newsComment.NewsId, this.user.Id, newsComment.Content);
            var count = await this.newsCommentsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfNewsCommentProperties()
        {
            await this.SeedUsers();
            await this.SeedNews();

            var model = new CreateNewsCommentInputModel
            {
                NewsId = this.firstNews.Id,
                Content = "Hello, how are you?",
            };

            await this.newsCommentsService.CreateAsync(model.NewsId, this.user.Id, model.Content);

            var newsComment = await this.newsCommentsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(model.NewsId, newsComment.NewsId);
            Assert.Equal("Hello, how are you?", newsComment.Content);
        }

        [Fact]
        public async Task CheckIfAddingNewsCommentThrowsArgumentException()
        {
            this.SeedDatabase();

            var newsComment = new CreateNewsCommentInputModel
            {
                NewsId = this.firstNews.Id,
                Content = "Test comment here",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async ()
                    => await this.newsCommentsService
                    .CreateAsync(newsComment.NewsId, this.user.Id, newsComment.Content));

            Assert.Equal(
                string.Format(
                    ExceptionMessages.NewsCommentAlreadyExists, newsComment.NewsId, newsComment.Content), exception.Message);
        }

        [Fact]
        public async Task CheckIfIsInNewsIdReturnsTrue()
        {
            this.SeedDatabase();

            var newsCommentId = await this.newsCommentsRepository
                .All()
                .Select(x => x.NewsId)
                .FirstOrDefaultAsync();

            var result = await this.newsCommentsService.IsInNewsId(newsCommentId, this.firstNews.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfIsInNewsIdReturnsFalse()
        {
           this.SeedDatabase();

           var result = await this.newsCommentsService.IsInNewsId(3, this.firstNews.Id);

           Assert.False(result);
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

            this.usersRepository = new EfDeletableEntityRepository<CinemaWorldUser>(dbContext);
            this.newsRepository = new EfDeletableEntityRepository<News>(dbContext);
            this.newsCommentsRepository = new EfDeletableEntityRepository<NewsComment>(dbContext);
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
                Id = 1,
                Title = "First news title",
                Description = "First news description",
                ShortDescription = "First news short description",
                ImagePath = TestImageUrl,
                UserId = this.user.Id,
                ViewsCounter = 30,
                IsUpdated = false,
            };

            this.firstNewsComment = new NewsComment
            {
                NewsId = this.firstNews.Id,
                Content = "Test comment here",
                UserId = this.user.Id,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedNews();
            await this.SeedNewsComments();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.user);

            await this.usersRepository.SaveChangesAsync();
        }

        private async Task SeedNews()
        {
            await this.newsRepository.AddAsync(this.firstNews);

            await this.newsRepository.SaveChangesAsync();
        }

        private async Task SeedNewsComments()
        {
            await this.newsCommentsRepository.AddAsync(this.firstNewsComment);

            await this.newsCommentsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
