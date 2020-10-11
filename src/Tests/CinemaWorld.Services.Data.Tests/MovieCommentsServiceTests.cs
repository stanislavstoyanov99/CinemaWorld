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
    using CinemaWorld.Models.InputModels.MovieComments;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class MovieCommentsServiceTests : IDisposable
    {
        private const string TestCoverPath = "https://somecoverurl.com";
        private const string TestTrailerPath = "https://sometrailerurl.com";
        private const string TestWallpaperPath = "https://somewallpaperurl.com";

        private readonly IMovieCommentsService movieCommentsService;
        private EfDeletableEntityRepository<MovieComment> movieCommentsRepository;
        private EfDeletableEntityRepository<Movie> moviesRepository;
        private EfDeletableEntityRepository<CinemaWorldUser> usersRepository;
        private EfDeletableEntityRepository<Director> directorsRepository;
        private SqliteConnection connection;

        private Movie firstMovie;
        private Director firstDirector;
        private MovieComment firstMovieComment;
        private CinemaWorldUser user;

        public MovieCommentsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.movieCommentsService = new MovieCommentsService(this.movieCommentsRepository);
        }

        [Fact]
        public async Task TestAddingMovieComment()
        {
            await this.SeedUsers();
            await this.SeedDirectors();
            await this.SeedMovies();

            var movieComment = new CreateMovieCommentInputModel
            {
                MovieId = this.firstMovie.Id,
                Content = "I like this movie.",
            };

            await this.movieCommentsService.CreateAsync(movieComment.MovieId, this.user.Id, movieComment.Content);
            var count = await this.movieCommentsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfMovieCommentProperties()
        {
            await this.SeedUsers();
            await this.SeedDirectors();
            await this.SeedMovies();

            var model = new CreateMovieCommentInputModel
            {
                MovieId = this.firstMovie.Id,
                Content = "What's your opinion for the movie?",
            };

            await this.movieCommentsService.CreateAsync(model.MovieId, this.user.Id, model.Content);

            var movieComment = await this.movieCommentsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(model.MovieId, movieComment.MovieId);
            Assert.Equal("What's your opinion for the movie?", movieComment.Content);
        }

        [Fact]
        public async Task CheckIfAddingMovieCommentThrowsArgumentException()
        {
            this.SeedDatabase();

            var movieComment = new CreateMovieCommentInputModel
            {
                MovieId = this.firstMovie.Id,
                Content = this.firstMovieComment.Content,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async ()
                    => await this.movieCommentsService
                    .CreateAsync(movieComment.MovieId, this.user.Id, movieComment.Content));

            Assert.Equal(
                string.Format(
                    ExceptionMessages.MovieCommentAlreadyExists, movieComment.MovieId, movieComment.Content), exception.Message);
        }

        [Fact]
        public async Task CheckIfIsInMovieIdReturnsTrue()
        {
            this.SeedDatabase();

            var movieCommentId = await this.movieCommentsRepository
                .All()
                .Select(x => x.MovieId)
                .FirstOrDefaultAsync();

            var result = await this.movieCommentsService.IsInMovieId(movieCommentId, this.firstMovie.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfIsInMovieIdReturnsFalse()
        {
           this.SeedDatabase();

           var result = await this.movieCommentsService.IsInMovieId(3, this.firstMovie.Id);

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
            this.directorsRepository = new EfDeletableEntityRepository<Director>(dbContext);
            this.moviesRepository = new EfDeletableEntityRepository<Movie>(dbContext);
            this.movieCommentsRepository = new EfDeletableEntityRepository<MovieComment>(dbContext);
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
                Id = 1,
                FirstName = "Kiril",
                LastName = "Petrov",
            };

            this.firstMovie = new Movie
            {
                Id = 1,
                Name = "Avatar",
                DateOfRelease = DateTime.UtcNow,
                Resolution = "HD",
                Rating = 7.80m,
                Description = "Avatar movie description",
                Language = "English",
                CoverPath = TestCoverPath,
                WallpaperPath = TestWallpaperPath,
                TrailerPath = TestTrailerPath,
                CinemaCategory = CinemaCategory.A,
                Length = 120,
                DirectorId = this.firstDirector.Id,
            };

            this.firstMovieComment = new MovieComment
            {
                MovieId = this.firstMovie.Id,
                Content = "Test comment here",
                UserId = this.user.Id,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedDirectors();
            await this.SeedMovies();
            await this.SeedMovieComments();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.user);

            await this.usersRepository.SaveChangesAsync();
        }

        private async Task SeedDirectors()
        {
            await this.directorsRepository.AddAsync(this.firstDirector);

            await this.directorsRepository.SaveChangesAsync();
        }

        private async Task SeedMovies()
        {
            await this.moviesRepository.AddAsync(this.firstMovie);

            await this.moviesRepository.SaveChangesAsync();
        }

        private async Task SeedMovieComments()
        {
            await this.movieCommentsRepository.AddAsync(this.firstMovieComment);

            await this.movieCommentsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
