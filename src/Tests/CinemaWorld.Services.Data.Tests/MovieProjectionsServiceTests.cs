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
    using CinemaWorld.Models.InputModels.AdministratorInputModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class MovieProjectionsServiceTests : IDisposable
    {
        private readonly IMovieProjectionsService movieProjectionsService;
        private EfDeletableEntityRepository<MovieProjection> movieProjectionsRepository;
        private EfDeletableEntityRepository<Hall> hallsRepository;
        private EfDeletableEntityRepository<Cinema> cinemasRepository;
        private EfDeletableEntityRepository<Movie> moviesRepository;
        private EfDeletableEntityRepository<Director> directorsRepository;
        private SqliteConnection connection;

        private Hall firstHall;
        private Cinema firstCinema;
        private Movie firstMovie;
        private Director firstDirector;
        private MovieProjection firstMovieProjection;

        public MovieProjectionsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.movieProjectionsService = new MovieProjectionsService(this.movieProjectionsRepository, this.moviesRepository, this.hallsRepository, this.cinemasRepository);
        }

        [Fact]
        public async Task CheckIfAddingMovieProjectionWorksCorrectly()
        {
            this.SeedDatabase();

            var model = new MovieProjectionCreateInputModel
            {
                Date = DateTime.UtcNow,
                MovieId = this.firstMovie.Id,
                HallId = this.firstHall.Id,
                CinemaId = this.firstCinema.Id,
            };

            await this.movieProjectionsService.CreateAsync(model);
            var count = await this.movieProjectionsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingsOfMovieProjectionProperties()
        {
            this.SeedDatabase();

            var model = new MovieProjectionCreateInputModel
            {
                Date = DateTime.UtcNow,
                MovieId = this.firstMovie.Id,
                HallId = this.firstHall.Id,
                CinemaId = this.firstCinema.Id,
            };

            await this.movieProjectionsService.CreateAsync(model);

            var movieProjection = await this.movieProjectionsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(this.firstMovie.Id, movieProjection.MovieId);
            Assert.Equal(this.firstHall.Id, movieProjection.HallId);
            Assert.Equal(this.firstCinema.Id, movieProjection.CinemaId);
        }

        [Fact]
        public async Task CheckAddingMovieProjectionWithMissingMovie()
        {
            this.SeedDatabase();

            var movieProjection = new MovieProjectionCreateInputModel
            {
                Date = DateTime.UtcNow,
                MovieId = 3,
                HallId = this.firstHall.Id,
                CinemaId = this.firstCinema.Id,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.movieProjectionsService.CreateAsync(movieProjection));
            Assert.Equal(string.Format(ExceptionMessages.MovieNotFound, movieProjection.MovieId), exception.Message);
        }

        [Fact]
        public async Task CheckAddingMovieProjectionWithMissingHall()
        {
            this.SeedDatabase();

            var movieProjection = new MovieProjectionCreateInputModel
            {
                Date = DateTime.UtcNow,
                MovieId = this.firstMovie.Id,
                HallId = 3,
                CinemaId = this.firstCinema.Id,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.movieProjectionsService.CreateAsync(movieProjection));
            Assert.Equal(string.Format(ExceptionMessages.HallNotFound, movieProjection.HallId), exception.Message);
        }

        [Fact]
        public async Task CheckAddingMovieProjectionWithMissingCinema()
        {
            this.SeedDatabase();

            var movieProjection = new MovieProjectionCreateInputModel
            {
                Date = DateTime.UtcNow,
                MovieId = this.firstMovie.Id,
                HallId = this.firstHall.Id,
                CinemaId = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.movieProjectionsService.CreateAsync(movieProjection));
            Assert.Equal(string.Format(ExceptionMessages.CinemaNotFound, movieProjection.CinemaId), exception.Message);
        }

        [Fact]
        public async Task CheckAddingAlreadyExistingMovieProjection()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            var movieProjection = new MovieProjectionCreateInputModel
            {
                Date = this.firstMovieProjection.Date,
                MovieId = this.firstMovieProjection.MovieId,
                HallId = this.firstMovieProjection.HallId,
                CinemaId = this.firstMovieProjection.CinemaId,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.movieProjectionsService.CreateAsync(movieProjection));
            Assert.Equal(string.Format(ExceptionMessages.MovieProjectionAlreadyExists, movieProjection.MovieId, movieProjection.HallId), exception.Message);
        }

        [Fact]
        public async Task CheckIfCreatingMovieProjectionReturnsViewModel()
        {
            this.SeedDatabase();

            var movieProjection = new MovieProjectionCreateInputModel
            {
                MovieId = 1,
                HallId = 1,
                CinemaId = 1,
            };

            var viewModel = await this.movieProjectionsService.CreateAsync(movieProjection);
            var dbEntry = await this.movieProjectionsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.MovieId, viewModel.Movie.Id);
            Assert.Equal(dbEntry.HallId, viewModel.Hall.Id);
            Assert.Equal(dbEntry.CinemaId, viewModel.Cinema.Id);
        }

        [Fact]
        public async Task CheckIfDeletingMovieProjectionWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            await this.movieProjectionsService.DeleteByIdAsync(this.firstMovieProjection.Id);

            var count = await this.movieProjectionsRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingMovieProjectionReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.movieProjectionsService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.MovieProjectionNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingMovieProjectionWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            var secondHall = new Hall
            {
                Category = HallCategory.Medium,
                Capacity = 50,
            };
            await this.hallsRepository.AddAsync(secondHall);
            await this.hallsRepository.SaveChangesAsync();

            var secondCinema = new Cinema
            {
                Name = "Paradise Center",
                Address = "Sofia",
            };
            await this.cinemasRepository.AddAsync(secondCinema);
            await this.cinemasRepository.SaveChangesAsync();

            var secondMovie = new Movie
            {
                Name = "Titanic 2",
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
            await this.moviesRepository.AddAsync(secondMovie);
            await this.moviesRepository.SaveChangesAsync();

            var movieProjectionEditViewModel = new MovieProjectionEditViewModel
            {
                Id = this.firstMovieProjection.Id,
                Date = DateTime.UtcNow.AddDays(2),
                HallId = 2,
                CinemaId = 2,
                MovieId = 2,
            };

            await this.movieProjectionsService.EditAsync(movieProjectionEditViewModel);

            Assert.Equal(movieProjectionEditViewModel.Date, this.firstMovieProjection.Date);
            Assert.Equal(movieProjectionEditViewModel.HallId, this.firstMovieProjection.HallId);
            Assert.Equal(movieProjectionEditViewModel.CinemaId, this.firstMovieProjection.CinemaId);
            Assert.Equal(movieProjectionEditViewModel.MovieId, this.firstMovieProjection.MovieId);
        }

        [Fact]
        public async Task CheckIfEditingMovieProjectionReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var movieProjectionEditViewModel = new MovieProjectionEditViewModel
            {
                Id = 3,
                Date = DateTime.UtcNow.AddDays(2),
                HallId = 2,
                CinemaId = 2,
                MovieId = 2,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.movieProjectionsService.EditAsync(movieProjectionEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.MovieProjectionNotFound, movieProjectionEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllMovieProjectionsAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            var secondMovie = new Movie
            {
                Name = "Anabel",
                DateOfRelease = DateTime.UtcNow.AddDays(3),
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
            await this.moviesRepository.AddAsync(secondMovie);
            await this.moviesRepository.SaveChangesAsync();

            var secondMovieProjection = new MovieProjection
            {
                Date = DateTime.UtcNow,
                MovieId = 2,
                HallId = 1,
                CinemaId = 1,
            };
            await this.movieProjectionsRepository.AddAsync(secondMovieProjection);
            await this.movieProjectionsRepository.SaveChangesAsync();

            var result = await this.movieProjectionsService.GetAllMovieProjectionsAsync<MovieProjectionDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(2, count);
            Assert.Equal("Anabel", result.First().Movie.Name);
        }

        [Fact]
        public async Task CheckIfGetAllMovieProjectionsByCinemaAsQueryeableWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            var secondMovie = new Movie
            {
                Name = "Anabel",
                DateOfRelease = DateTime.UtcNow.AddDays(3),
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
            await this.moviesRepository.AddAsync(secondMovie);
            await this.moviesRepository.SaveChangesAsync();

            var secondMovieProjection = new MovieProjection
            {
                Date = DateTime.UtcNow,
                MovieId = 2,
                HallId = 1,
                CinemaId = 1,
            };
            await this.movieProjectionsRepository.AddAsync(secondMovieProjection);
            await this.movieProjectionsRepository.SaveChangesAsync();

            var result = this.movieProjectionsService.GetAllMovieProjectionsByCinemaAsQueryeable<MovieProjectionDetailsViewModel>("Bulgaria Mall");

            var count = result.Count();
            Assert.Equal(2, count);
            Assert.Equal("Bulgaria Mall", result.First().Cinema.Name);
        }

        [Fact]
        public async Task CheckIfGetAllMovieProjectionsAsQueryeableWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            var secondMovie = new Movie
            {
                Name = "Anabel",
                DateOfRelease = DateTime.UtcNow.AddDays(3),
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
            await this.moviesRepository.AddAsync(secondMovie);
            await this.moviesRepository.SaveChangesAsync();

            var secondMovieProjection = new MovieProjection
            {
                Date = DateTime.UtcNow,
                MovieId = 2,
                HallId = 1,
                CinemaId = 1,
            };
            await this.movieProjectionsRepository.AddAsync(secondMovieProjection);
            await this.movieProjectionsRepository.SaveChangesAsync();

            var result = this.movieProjectionsService.GetAllMovieProjectionsAsQueryeable<MovieProjectionDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(2, count);
            Assert.Equal("Bulgaria Mall", result.First().Cinema.Name);
        }

        [Fact]
        public async Task CheckIfIfGetViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            var expectedModel = new MovieProjectionDetailsViewModel
            {
                Id = this.firstMovieProjection.Id,
            };

            var viewModel = await this.movieProjectionsService.GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(this.firstMovieProjection.Id);
            expectedModel.Hall = viewModel.Hall;
            expectedModel.Cinema = viewModel.Cinema;
            expectedModel.Movie = viewModel.Movie;
            expectedModel.Date = viewModel.Date;

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();
            await this.SeedMovieProjections();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () =>
                    await this.movieProjectionsService.GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.MovieProjectionNotFound, 3), exception.Message);
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

            this.movieProjectionsRepository = new EfDeletableEntityRepository<MovieProjection>(dbContext);
            this.hallsRepository = new EfDeletableEntityRepository<Hall>(dbContext);
            this.cinemasRepository = new EfDeletableEntityRepository<Cinema>(dbContext);
            this.moviesRepository = new EfDeletableEntityRepository<Movie>(dbContext);
            this.directorsRepository = new EfDeletableEntityRepository<Director>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstHall = new Hall
            {
                Category = HallCategory.Large,
                Capacity = 100,
            };

            this.firstCinema = new Cinema
            {
                Name = "Bulgaria Mall",
                Address = "Sofia, bul. Stamboliiski",
            };

            this.firstDirector = new Director
            {
                FirstName = "Steven",
                LastName = "Spielberg",
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

            this.firstMovieProjection = new MovieProjection
            {
                Date = DateTime.UtcNow,
                MovieId = 1,
                HallId = 1,
                CinemaId = 1,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedDirectors();
            await this.SeedMovies();
            await this.SeedHalls();
            await this.SeedCinemas();
        }

        private async Task SeedHalls()
        {
            await this.hallsRepository.AddAsync(this.firstHall);

            await this.hallsRepository.SaveChangesAsync();
        }

        private async Task SeedCinemas()
        {
            await this.cinemasRepository.AddAsync(this.firstCinema);

            await this.cinemasRepository.SaveChangesAsync();
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

        private async Task SeedMovieProjections()
        {
            await this.movieProjectionsRepository.AddAsync(this.firstMovieProjection);

            await this.movieProjectionsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
