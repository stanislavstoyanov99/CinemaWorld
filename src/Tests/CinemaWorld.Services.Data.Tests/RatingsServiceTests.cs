namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class RatingsServiceTests : IDisposable
    {
        private readonly IRatingsService ratingsService;
        private EfDeletableEntityRepository<StarRating> starRatingsRepository;
        private EfDeletableEntityRepository<Movie> moviesRepository;
        private EfDeletableEntityRepository<Director> directorsRepository;
        private EfDeletableEntityRepository<CinemaWorldUser> usersRepository;
        private SqliteConnection connection;

        private StarRating firstStarRating;
        private Director firstDirector;
        private Movie firstMovie;
        private CinemaWorldUser user;

        public RatingsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.ratingsService = new RatingsService(this.starRatingsRepository);
        }

        [Fact]
        public async Task CheckIfVoteAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            await this.ratingsService.VoteAsync(this.firstMovie.Id, this.user.Id, 5);
            var count = await this.starRatingsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfVoteAsyncThrowsArgumentException()
        {
            this.SeedDatabase();
            await this.SeedStarRatings();

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.ratingsService.VoteAsync(this.firstMovie.Id, this.user.Id, 6));
            Assert.Equal(ExceptionMessages.AlreadySentVote, exception.Message);
        }

        [Fact]
        public async Task CheckIfVoteAsyncWorksCorrectlyAfterSecondVoting()
        {
            this.SeedDatabase();
            var starRating = new StarRating
            {
                Rate = 10,
                MovieId = 1,
                UserId = this.user.Id,
                NextVoteDate = DateTime.UtcNow.AddDays(-1),
            };
            await this.starRatingsRepository.AddAsync(starRating);
            await this.starRatingsRepository.SaveChangesAsync();

            await this.ratingsService.VoteAsync(this.firstMovie.Id, this.user.Id, 5);
            var count = await this.starRatingsRepository.All().CountAsync();
            var currentStarRating = await this.starRatingsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(1, count);
            Assert.Equal(15, currentStarRating.Rate);
            Assert.Equal(starRating.NextVoteDate, currentStarRating.NextVoteDate);
        }

        [Fact]
        public async Task CheckIfGetStarRatingsAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedStarRatings();

            var result = await this.ratingsService.GetStarRatingsAsync(this.firstMovie.Id);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task CheckIfGetNextVoteDateAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedStarRatings();

            var result = await this.ratingsService.GetNextVoteDateAsync(this.firstMovie.Id, this.user.Id);

            Assert.Equal(this.firstStarRating.NextVoteDate, result);
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
            this.starRatingsRepository = new EfDeletableEntityRepository<StarRating>(dbContext);
            this.moviesRepository = new EfDeletableEntityRepository<Movie>(dbContext);
            this.directorsRepository = new EfDeletableEntityRepository<Director>(dbContext);
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

            this.firstDirector = new Director
            {
                FirstName = "Peter",
                LastName = "Kirilov",
            };

            this.firstMovie = new Movie
            {
                Name = "Titanic",
                DateOfRelease = DateTime.UtcNow,
                Resolution = "HD",
                Rating = 7.80m,
                Description = "Test description here",
                Language = "English",
                CinemaCategory = CinemaCategory.B,
                TrailerPath = "test trailer path",
                CoverPath = "test cover path",
                WallpaperPath = "test wallpaper path",
                IMDBLink = "test imdb link",
                Length = 120,
                DirectorId = 1,
            };

            this.firstStarRating = new StarRating
            {
                Rate = 5,
                MovieId = 1,
                UserId = this.user.Id,
                NextVoteDate = DateTime.UtcNow.AddDays(1),
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedDirectors();
            await this.SeedMovies();
            await this.SeedUsers();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.user);

            await this.usersRepository.SaveChangesAsync();
        }

        private async Task SeedStarRatings()
        {
            await this.starRatingsRepository.AddAsync(this.firstStarRating);

            await this.starRatingsRepository.SaveChangesAsync();
        }

        private async Task SeedMovies()
        {
            await this.moviesRepository.AddAsync(this.firstMovie);

            await this.moviesRepository.SaveChangesAsync();
        }

        private async Task SeedDirectors()
        {
            await this.directorsRepository.AddAsync(this.firstDirector);

            await this.directorsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
