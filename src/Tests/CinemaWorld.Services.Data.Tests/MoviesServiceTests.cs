namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Data.Tests.Helpers;
    using CinemaWorld.Services.Mapping;

    using CloudinaryDotNet;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class MoviesServiceTests : IDisposable, IClassFixture<Configuration>
    {
        private readonly IMoviesService moviesService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;

        private EfDeletableEntityRepository<Movie> moviesRepository;
        private EfDeletableEntityRepository<Director> directorsRepository;
        private EfDeletableEntityRepository<MovieGenre> movieGenresRepository;
        private EfDeletableEntityRepository<MovieCountry> movieCountriesRepository;

        private SqliteConnection connection;

        private Movie firstMovie;
        private Director firstDirector;
        private MovieGenre firstMovieGenre;
        private MovieCountry firstMovieCountry;

        public MoviesServiceTests(Configuration configuration)
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

            this.moviesService = new MoviesService(
                this.moviesRepository,
                this.directorsRepository,
                this.movieGenresRepository,
                this.movieCountriesRepository,
                this.cloudinaryService);
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

            this.moviesRepository = new EfDeletableEntityRepository<Movie>(dbContext);
            this.directorsRepository = new EfDeletableEntityRepository<Director>(dbContext);
            this.movieGenresRepository = new EfDeletableEntityRepository<MovieGenre>(dbContext);
            this.movieCountriesRepository = new EfDeletableEntityRepository<MovieCountry>(dbContext);
        }

        private void InitializeFields()
        {
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
        }

        private async void SeedDatabase()
        {
            await this.SeedDirectors();
            await this.SeedMovies();
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

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
