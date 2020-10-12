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
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class MoviesServiceTests : IDisposable, IClassFixture<Configuration>
    {
        private const string TestCoverImageUrl = "https://someurl.com";
        private const string TestWallpaperImageUrl = "https://someurl2.com";
        private const string TestImagePath = "Test.jpg";
        private const string TestImageContentType = "image/jpg";

        private readonly IMoviesService moviesService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;

        private EfDeletableEntityRepository<Movie> moviesRepository;
        private EfDeletableEntityRepository<Genre> genresRepository;
        private EfDeletableEntityRepository<Country> countriesRepository;
        private EfDeletableEntityRepository<Director> directorsRepository;
        private EfDeletableEntityRepository<MovieGenre> movieGenresRepository;
        private EfDeletableEntityRepository<MovieCountry> movieCountriesRepository;

        private SqliteConnection connection;

        private Movie firstMovie;
        private Genre firstGenre;
        private Country firstCountry;
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

        [Fact]
        public async Task TestAddingMovie()
        {
            this.SeedDatabase();

            MovieDetailsViewModel movieDetailsViewModel;

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var model = new MovieCreateInputModel
                {
                    Name = "Titanic",
                    DateOfRelease = DateTime.UtcNow,
                    Resolution = "HD",
                    Rating = 7.80m,
                    Description = "Test description here",
                    Language = "English",
                    CinemaCategory = CinemaCategory.B.ToString(),
                    TrailerPath = "test trailer path",
                    CoverImage = file,
                    Wallpaper = file,
                    IMDBLink = "test imdb link",
                    Length = 190,
                    DirectorId = 1,
                    SelectedGenres = new List<int> { 1 },
                    SelectedCountries = new List<int> { 1 },
                };

                movieDetailsViewModel = await this.moviesService.CreateAsync(model);
            }

            await this.cloudinaryService.DeleteImage(this.cloudinary, movieDetailsViewModel.Name);
            await this.cloudinaryService.DeleteImage(this.cloudinary, movieDetailsViewModel.Name + Suffixes.WallpaperSuffix);

            var count = await this.moviesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfAddingMovieReturnsViewModel()
        {
            this.SeedDatabase();

            MovieDetailsViewModel movieDetailsViewModel;

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var model = new MovieCreateInputModel
                {
                    Name = "Titanic",
                    DateOfRelease = DateTime.UtcNow,
                    Resolution = "HD",
                    Rating = 7.80m,
                    Description = "Test description here",
                    Language = "English",
                    CinemaCategory = CinemaCategory.B.ToString(),
                    TrailerPath = "test trailer path",
                    CoverImage = file,
                    Wallpaper = file,
                    IMDBLink = "test imdb link",
                    Length = 190,
                    DirectorId = 1,
                    SelectedGenres = new List<int> { 1 },
                    SelectedCountries = new List<int> { 1 },
                };

                movieDetailsViewModel = await this.moviesService.CreateAsync(model);
            }

            await this.cloudinaryService.DeleteImage(this.cloudinary, movieDetailsViewModel.Name);
            await this.cloudinaryService.DeleteImage(this.cloudinary, movieDetailsViewModel.Name + Suffixes.WallpaperSuffix);

            var movie = await this.moviesRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Titanic", movie.Name);
            Assert.Equal("HD", movie.Resolution);
            Assert.Equal(7.80m, movie.Rating);
            Assert.Equal("Test description here", movie.Description);
            Assert.Equal("English", movie.Language);
            Assert.Equal("B", movie.CinemaCategory.ToString());
            Assert.Equal("test trailer path", movie.TrailerPath);
            Assert.Equal("test imdb link", movie.IMDBLink);
            Assert.Equal(190, movie.Length);
            Assert.Equal(1, movie.DirectorId);
        }

        [Fact]
        public async Task TestAddingMovieWithInvalidCinemaCategory()
        {
            this.SeedDatabase();

            var model = new MovieCreateInputModel
            {
                Name = this.firstMovie.Name,
                DateOfRelease = this.firstMovie.DateOfRelease,
                Resolution = this.firstMovie.Resolution,
                Rating = this.firstMovie.Rating,
                Description = this.firstMovie.Description,
                Language = this.firstMovie.Language,
                CinemaCategory = "Invalid category",
                TrailerPath = this.firstMovie.TrailerPath,
                IMDBLink = this.firstMovie.IMDBLink,
                Length = this.firstMovie.Length,
                DirectorId = this.firstMovie.DirectorId,
                SelectedGenres = new List<int> { 1 },
                SelectedCountries = new List<int> { 1 },
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.moviesService.CreateAsync(model));
            Assert.Equal(string.Format(ExceptionMessages.InvalidCinemaCategoryType, model.CinemaCategory), exception.Message);
        }

        [Fact]
        public async Task TestAddingMovieWithMissingDirector()
        {
            this.SeedDatabase();

            var model = new MovieCreateInputModel
            {
                Name = this.firstMovie.Name,
                DateOfRelease = this.firstMovie.DateOfRelease,
                Resolution = this.firstMovie.Resolution,
                Rating = this.firstMovie.Rating,
                Description = this.firstMovie.Description,
                Language = this.firstMovie.Language,
                CinemaCategory = this.firstMovie.CinemaCategory.ToString(),
                TrailerPath = this.firstMovie.TrailerPath,
                IMDBLink = this.firstMovie.IMDBLink,
                Length = this.firstMovie.Length,
                DirectorId = 2,
                SelectedGenres = new List<int> { 1 },
                SelectedCountries = new List<int> { 1 },
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.moviesService.CreateAsync(model));
            Assert.Equal(string.Format(ExceptionMessages.DirectorNotFound, model.DirectorId), exception.Message);
        }

        [Fact]
        public async Task TestAddingAlreadyExistingMovie()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var model = new MovieCreateInputModel
                {
                    Name = "Titanic",
                    DateOfRelease = DateTime.UtcNow,
                    Resolution = "HD",
                    Rating = 7.80m,
                    Description = "Test description here",
                    Language = "English",
                    CinemaCategory = CinemaCategory.B.ToString(),
                    TrailerPath = "test trailer path",
                    CoverImage = file,
                    Wallpaper = file,
                    IMDBLink = "test imdb link",
                    Length = 120,
                    DirectorId = 1,
                };

                var exception = await Assert
                    .ThrowsAsync<ArgumentException>(async () => await this.moviesService.CreateAsync(model));

                await this.cloudinaryService.DeleteImage(this.cloudinary, model.Name);
                await this.cloudinaryService.DeleteImage(this.cloudinary, model.Name + Suffixes.WallpaperSuffix);
                Assert.Equal(string.Format(ExceptionMessages.MovieAlreadyExists, model.Name), exception.Message);
            }
        }

        [Fact]
        public async Task CheckIfEditingMovieWorksCorrectlyWhenImagesStaysTheSame()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            var newDirector = new Director
            {
                FirstName = "Pesho",
                LastName = "Petrov",
            };
            await this.directorsRepository.AddAsync(newDirector);
            await this.directorsRepository.SaveChangesAsync();

            var movieEditViewModel = new MovieEditViewModel
            {
                Id = this.firstMovie.Id,
                Name = "Brothers and sisters",
                DateOfRelease = DateTime.UtcNow.AddDays(1),
                Resolution = "SD",
                Rating = 7.50m,
                Description = "Changed Test description",
                Language = "USA",
                CinemaCategory = CinemaCategory.C.ToString(),
                TrailerPath = "Changed Test trailer path",
                CoverImage = null,
                Wallpaper = null,
                IMDBLink = "Changed Test imdb link",
                Length = 110,
                DirectorId = 2,
                SelectedGenres = new List<int> { 1, },
                SelectedCountries = new List<int> { 1, },
            };

            await this.moviesService.EditAsync(movieEditViewModel);

            Assert.Equal(movieEditViewModel.Id, this.firstMovie.Id);
            Assert.Equal(movieEditViewModel.Name, this.firstMovie.Name);
            Assert.Equal(movieEditViewModel.DateOfRelease, this.firstMovie.DateOfRelease);
            Assert.Equal(movieEditViewModel.Resolution, this.firstMovie.Resolution);
            Assert.Equal(movieEditViewModel.Rating, this.firstMovie.Rating);
            Assert.Equal(movieEditViewModel.Description, this.firstMovie.Description);
            Assert.Equal(movieEditViewModel.Language, this.firstMovie.Language);
            Assert.Equal(movieEditViewModel.CinemaCategory, this.firstMovie.CinemaCategory.ToString());
            Assert.Equal(movieEditViewModel.TrailerPath, this.firstMovie.TrailerPath);
            Assert.Equal(movieEditViewModel.IMDBLink, this.firstMovie.IMDBLink);
            Assert.Equal(movieEditViewModel.Length, this.firstMovie.Length);
        }

        [Fact]
        public async Task CheckEditingMovieWithInvalidCinemaCategory()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            var model = new MovieEditViewModel
            {
                Id = this.firstMovie.Id,
                Name = this.firstMovie.Name,
                DateOfRelease = this.firstMovie.DateOfRelease,
                Resolution = this.firstMovie.Resolution,
                Rating = this.firstMovie.Rating,
                Description = this.firstMovie.Description,
                Language = this.firstMovie.Language,
                CinemaCategory = "Invalid category",
                TrailerPath = this.firstMovie.TrailerPath,
                CoverImage = null,
                Wallpaper = null,
                IMDBLink = this.firstMovie.IMDBLink,
                Length = this.firstMovie.Length,
                DirectorId = this.firstMovie.DirectorId,
                SelectedGenres = new List<int> { 1, },
                SelectedCountries = new List<int> { 1, },
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.moviesService.EditAsync(model));
            Assert.Equal(string.Format(ExceptionMessages.InvalidCinemaCategoryType, model.CinemaCategory), exception.Message);
        }

        [Fact]
        public async Task CheckEditingMovieWithMissingMovie()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            var model = new MovieEditViewModel
            {
                Id = 3,
                Name = this.firstMovie.Name,
                DateOfRelease = this.firstMovie.DateOfRelease,
                Resolution = this.firstMovie.Resolution,
                Rating = this.firstMovie.Rating,
                Description = this.firstMovie.Description,
                Language = this.firstMovie.Language,
                CinemaCategory = this.firstMovie.CinemaCategory.ToString(),
                TrailerPath = this.firstMovie.TrailerPath,
                CoverImage = null,
                Wallpaper = null,
                IMDBLink = this.firstMovie.IMDBLink,
                Length = this.firstMovie.Length,
                DirectorId = this.firstMovie.DirectorId,
                SelectedGenres = new List<int> { 1, },
                SelectedCountries = new List<int> { 1, },
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.moviesService.EditAsync(model));
            Assert.Equal(string.Format(ExceptionMessages.MovieNotFound, model.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditAsyncEditsCoverImage()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var movieEditViewModel = new MovieEditViewModel()
                {
                    Id = this.firstMovie.Id,
                    Name = this.firstMovie.Name,
                    DateOfRelease = this.firstMovie.DateOfRelease,
                    Resolution = this.firstMovie.Resolution,
                    Rating = this.firstMovie.Rating,
                    Description = this.firstMovie.Description,
                    Language = this.firstMovie.Language,
                    CinemaCategory = this.firstMovie.CinemaCategory.ToString(),
                    TrailerPath = this.firstMovie.TrailerPath,
                    CoverImage = file,
                    Wallpaper = null,
                    IMDBLink = this.firstMovie.IMDBLink,
                    Length = this.firstMovie.Length,
                    DirectorId = this.firstMovie.DirectorId,
                    SelectedGenres = new List<int> { 1, },
                    SelectedCountries = new List<int> { 1, },
                };

                await this.moviesService.EditAsync(movieEditViewModel);

                await this.cloudinaryService.DeleteImage(this.cloudinary, movieEditViewModel.Name);
            }

            var movie = await this.moviesRepository.All().FirstAsync();
            Assert.NotEqual(TestCoverImageUrl, movie.CoverPath);
        }

        [Fact]
        public async Task CheckIfEditAsyncEditsWallpaperImage()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var movieEditViewModel = new MovieEditViewModel()
                {
                    Id = this.firstMovie.Id,
                    Name = this.firstMovie.Name,
                    DateOfRelease = this.firstMovie.DateOfRelease,
                    Resolution = this.firstMovie.Resolution,
                    Rating = this.firstMovie.Rating,
                    Description = this.firstMovie.Description,
                    Language = this.firstMovie.Language,
                    CinemaCategory = this.firstMovie.CinemaCategory.ToString(),
                    TrailerPath = this.firstMovie.TrailerPath,
                    CoverImage = null,
                    Wallpaper = file,
                    IMDBLink = this.firstMovie.IMDBLink,
                    Length = this.firstMovie.Length,
                    DirectorId = this.firstMovie.DirectorId,
                    SelectedGenres = new List<int> { 1, },
                    SelectedCountries = new List<int> { 1, },
                };

                await this.moviesService.EditAsync(movieEditViewModel);

                await this.cloudinaryService.DeleteImage(this.cloudinary, movieEditViewModel.Name + Suffixes.WallpaperSuffix);
            }

            var movie = await this.moviesRepository.All().FirstAsync();
            Assert.NotEqual(TestWallpaperImageUrl, movie.WallpaperPath);
        }

        [Fact]
        public async Task CheckIfEditAsyncWorksCorrectlyWithDifferentMovieGenresAndCountries()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieGenres();
            await this.SeedMovieCountries();

            var secondGenre = new Genre
            {
                Name = "Romance",
            };
            await this.genresRepository.AddAsync(secondGenre);
            await this.genresRepository.SaveChangesAsync();

            var secondCountry = new Country
            {
                Name = "Bulgaria",
            };
            await this.countriesRepository.AddAsync(secondCountry);
            await this.countriesRepository.SaveChangesAsync();

            var secondMovieGenre = new MovieGenre
            {
                MovieId = this.firstMovie.Id,
                GenreId = 2,
            };
            await this.movieGenresRepository.AddAsync(secondMovieGenre);
            await this.movieGenresRepository.SaveChangesAsync();

            var secondMovieCountry = new MovieCountry
            {
                MovieId = this.firstMovie.Id,
                CountryId = 2,
            };
            await this.movieCountriesRepository.AddAsync(secondMovieCountry);
            await this.movieCountriesRepository.SaveChangesAsync();

            using (var stream = File.OpenRead(TestImagePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = TestImageContentType,
                };

                var movieEditViewModel = new MovieEditViewModel()
                {
                    Id = this.firstMovie.Id,
                    Name = this.firstMovie.Name,
                    DateOfRelease = this.firstMovie.DateOfRelease,
                    Resolution = this.firstMovie.Resolution,
                    Rating = this.firstMovie.Rating,
                    Description = this.firstMovie.Description,
                    Language = this.firstMovie.Language,
                    CinemaCategory = this.firstMovie.CinemaCategory.ToString(),
                    TrailerPath = this.firstMovie.TrailerPath,
                    CoverImage = file,
                    Wallpaper = null,
                    IMDBLink = this.firstMovie.IMDBLink,
                    Length = this.firstMovie.Length,
                    DirectorId = this.firstMovie.DirectorId,
                    SelectedGenres = new List<int> { 2, },
                    SelectedCountries = new List<int> { 2, },
                };

                await this.moviesService.EditAsync(movieEditViewModel);

                await this.cloudinaryService.DeleteImage(this.cloudinary, movieEditViewModel.Name);

                var movie = await this.moviesRepository.All().FirstOrDefaultAsync();
                Assert.Contains(movie.MovieGenres, x => x.GenreId == 2);
                Assert.Contains(movie.MovieCountries, x => x.CountryId == 2);
            }
        }

        [Fact]
        public async Task CheckIfDeletingMovieWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieGenres();
            await this.SeedMovieCountries();

            await this.moviesService.DeleteByIdAsync(this.firstMovie.Id);

            var count = await this.moviesRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingMovieReturnsNullReferenceException()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.moviesService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.MovieNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllMoviesAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            var result = await this.moviesService.GetAllMoviesAsync<MovieDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetAllMoviesAsQueryeableWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            var result = this.moviesService.GetAllMoviesAsQueryeable<MovieDetailsViewModel>();

            var count = await result.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetRecentlyAddedMoviesAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            var secondMovie = new Movie
            {
                Name = "Titanic 3",
                DateOfRelease = DateTime.UtcNow.AddDays(3),
                Resolution = "HD",
                Rating = 7.20m,
                Description = "Test description here",
                Language = "English",
                CinemaCategory = CinemaCategory.B,
                TrailerPath = "test trailer path",
                CoverPath = TestCoverImageUrl,
                WallpaperPath = TestWallpaperImageUrl,
                IMDBLink = "test imdb link",
                Length = 120,
                DirectorId = 1,
            };
            await this.moviesRepository.AddAsync(secondMovie);
            await this.moviesRepository.SaveChangesAsync();

            var result = await this.moviesService.GetRecentlyAddedMoviesAsync<MovieDetailsViewModel>(1);

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetMostPopularMoviesAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            var secondMovie = new Movie
            {
                Name = "Titanic 3",
                DateOfRelease = DateTime.UtcNow.AddDays(3),
                Resolution = "HD",
                Rating = 7.20m,
                Description = "Test description here",
                Language = "English",
                CinemaCategory = CinemaCategory.B,
                TrailerPath = "test trailer path",
                CoverPath = TestCoverImageUrl,
                WallpaperPath = TestWallpaperImageUrl,
                IMDBLink = "test imdb link",
                Length = 120,
                DirectorId = 1,
            };
            await this.moviesRepository.AddAsync(secondMovie);
            await this.moviesRepository.SaveChangesAsync();

            var result = await this.moviesService.GetMostPopularMoviesAsync<MovieDetailsViewModel>(2);

            var count = result.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfGetAllMovieGenresAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieGenres();

            var result = await this.moviesService.GetAllMovieGenresAsync<MovieGenreViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetAllMovieCountriesAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieCountries();

            var result = await this.moviesService.GetAllMovieCountriesAsync<MovieCountryViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        // Strange error with decimal type
        [Fact]
        public async Task CheckIfGetTopImdbMoviesAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieCountries();

            var result = await this.moviesService.GetTopImdbMoviesAsync<MovieDetailsViewModel>(5.51m, 1);

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetTopRatingMoviesAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieCountries();

            var result = await this.moviesService.GetTopRatingMoviesAsync<MovieDetailsViewModel>(4.50m, 1);

            var count = result.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfGetMovieViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();
            await this.SeedMovies();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () =>
                    await this.moviesService.GetViewModelByIdAsync<MovieDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.MovieNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetByGenreNameAsQueryableWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieGenres();

            var result = this.moviesService.GetByGenreNameAsQueryable("Drama");

            var count = await result.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetAllMoviesByFilterAsQueryeableWorksCorrectlyWhenMovieNameStartsWithLetter()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieGenres();

            var result = this.moviesService.GetAllMoviesByFilterAsQueryeable<MovieDetailsViewModel>("t");
            var movie = await result.FirstAsync();

            var count = await result.CountAsync();
            Assert.Equal(1, count);
            Assert.Equal("Titanic", movie.Name);
        }

        [Fact]
        public async Task CheckIfGetAllMoviesByFilterAsQueryeableWorksCorrectlyWhenMovieNameStartsWithDigit()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieGenres();
            var secondMovie = new Movie
            {
                Name = "1917",
                DateOfRelease = DateTime.UtcNow.AddDays(4),
                Resolution = "HD",
                Rating = 7.30m,
                Description = "Test description here",
                Language = "English",
                CinemaCategory = CinemaCategory.C,
                TrailerPath = "test trailer path",
                CoverPath = TestCoverImageUrl,
                WallpaperPath = TestWallpaperImageUrl,
                IMDBLink = "test imdb link",
                Length = 80,
                DirectorId = 1,
            };
            await this.moviesRepository.AddAsync(secondMovie);
            await this.moviesRepository.SaveChangesAsync();

            var result = this.moviesService.GetAllMoviesByFilterAsQueryeable<MovieDetailsViewModel>("0 - 9");
            var movie = await result.FirstAsync();

            var count = await result.CountAsync();
            Assert.Equal(1, count);
            Assert.Equal("1917", movie.Name);
        }

        [Fact]
        public async Task CheckIfGetAllMoviesByFilterAsQueryeableWorksCorrectlyWithoutProvidedSearchCriteria()
        {
            this.SeedDatabase();
            await this.SeedMovies();
            await this.SeedMovieGenres();

            var result = this.moviesService.GetAllMoviesByFilterAsQueryeable<MovieDetailsViewModel>();
            var movie = await result.FirstAsync();

            var count = await result.CountAsync();
            Assert.Equal(1, count);
            Assert.Equal("Titanic", movie.Name);
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
            this.genresRepository = new EfDeletableEntityRepository<Genre>(dbContext);
            this.countriesRepository = new EfDeletableEntityRepository<Country>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstGenre = new Genre
            {
                Name = "Drama",
            };

            this.firstCountry = new Country
            {
                Name = "USA",
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
                Rating = 7.50m,
                Description = "Test description here",
                Language = "English",
                CinemaCategory = CinemaCategory.B,
                TrailerPath = "test trailer path",
                CoverPath = TestCoverImageUrl,
                WallpaperPath = TestWallpaperImageUrl,
                IMDBLink = "test imdb link",
                Length = 120,
                DirectorId = 1,
            };

            this.firstMovieGenre = new MovieGenre
            {
                GenreId = 1,
                MovieId = 1,
            };

            this.firstMovieCountry = new MovieCountry
            {
                CountryId = 1,
                MovieId = 1,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedDirectors();
            await this.SeedGenres();
            await this.SeedCountries();
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

        private async Task SeedMovieGenres()
        {
            await this.movieGenresRepository.AddAsync(this.firstMovieGenre);

            await this.movieGenresRepository.SaveChangesAsync();
        }

        private async Task SeedMovieCountries()
        {
            await this.movieCountriesRepository.AddAsync(this.firstMovieCountry);

            await this.movieCountriesRepository.SaveChangesAsync();
        }

        private async Task SeedGenres()
        {
            await this.genresRepository.AddAsync(this.firstGenre);

            await this.genresRepository.SaveChangesAsync();
        }

        private async Task SeedCountries()
        {
            await this.countriesRepository.AddAsync(this.firstCountry);

            await this.countriesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
