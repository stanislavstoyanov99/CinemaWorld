namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Genres;
    using CinemaWorld.Models.ViewModels.Genres;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class GenresServiceTests : IDisposable
    {
        private readonly IGenresService genresService;
        private EfDeletableEntityRepository<Genre> genresRepository;
        private SqliteConnection connection;

        private Genre firstGenre;

        public GenresServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.genresService = new GenresService(this.genresRepository);
        }

        [Fact]
        public async Task TestAddingGenre()
        {
            var model = new GenreCreateInputModel
            {
                Name = "Comedy",
            };

            await this.genresService.CreateAsync(model);
            var count = await this.genresRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfGenresProperties()
        {
            var model = new GenreCreateInputModel
            {
                Name = "Drama",
            };

            await this.genresService.CreateAsync(model);

            var genre = await this.genresRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Drama", genre.Name);
        }

        [Fact]
        public async Task CheckIfAddingGenreThrowsArgumentException()
        {
            this.SeedDatabase();

            var genre = new GenreCreateInputModel
            {
                Name = "Action",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.genresService.CreateAsync(genre));
            Assert.Equal(string.Format(ExceptionMessages.GenreAlreadyExists, genre.Name), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingGenreReturnsViewModel()
        {
            var genre = new GenreCreateInputModel
            {
                Name = "Historical",
            };

            var viewModel = await this.genresService.CreateAsync(genre);
            var dbEntry = await this.genresRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Name, viewModel.Name);
        }

        [Fact]
        public async Task CheckIfDeletingGenreWorksCorrectly()
        {
            this.SeedDatabase();

            await this.genresService.DeleteByIdAsync(this.firstGenre.Id);

            var count = await this.genresRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingGenreReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.genresService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.GenreNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingGenreWorksCorrectly()
        {
            this.SeedDatabase();

            var genreEditViewModel = new GenreEditViewModel
            {
                Id = this.firstGenre.Id,
                Name = "Changed genre name",
            };

            await this.genresService.EditAsync(genreEditViewModel);

            Assert.Equal(genreEditViewModel.Name, this.firstGenre.Name);
        }

        [Fact]
        public async Task CheckIfEditingGenreReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var genreEditViewModel = new GenreEditViewModel
            {
                Id = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.genresService.EditAsync(genreEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.GenreNotFound, genreEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetGenreViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new GenreDetailsViewModel
            {
                Id = this.firstGenre.Id,
                Name = this.firstGenre.Name,
            };

            var viewModel = await this.genresService.GetViewModelByIdAsync<GenreDetailsViewModel>(this.firstGenre.Id);

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
                    await this.genresService.GetViewModelByIdAsync<GenreDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.GenreNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllGenresAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.genresService.GetAllGenresAsync<GenreDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
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

            this.genresRepository = new EfDeletableEntityRepository<Genre>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstGenre = new Genre
            {
                Name = "Action",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedGenres();
        }

        private async Task SeedGenres()
        {
            await this.genresRepository.AddAsync(this.firstGenre);

            await this.genresRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
