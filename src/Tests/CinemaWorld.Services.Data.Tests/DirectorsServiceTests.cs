namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Directors;
    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class DirectorsServiceTests : IDisposable
    {
        private readonly IDirectorsService directorsService;
        private EfDeletableEntityRepository<Director> directorsRepository;
        private SqliteConnection connection;

        private Director firstDirector;

        public DirectorsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.directorsService = new DirectorsService(this.directorsRepository);
        }

        [Fact]
        public async Task TestAddingDirector()
        {
            var model = new DirectorCreateInputModel
            {
                FirstName = "Kiril",
                LastName = "Petrov",
            };

            await this.directorsService.CreateAsync(model);
            var count = await this.directorsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfDirectorProperties()
        {
            var model = new DirectorCreateInputModel
            {
                FirstName = "Stamat",
                LastName = "Stamatov",
            };

            await this.directorsService.CreateAsync(model);

            var director = await this.directorsRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Stamat", director.FirstName);
            Assert.Equal("Stamatov", director.LastName);
        }

        [Fact]
        public async Task CheckIfAddingDirectorThrowsArgumentException()
        {
            this.SeedDatabase();

            var director = new DirectorCreateInputModel
            {
                FirstName = "Pesho",
                LastName = "Peshov",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.directorsService.CreateAsync(director));
            Assert.Equal(string.Format(ExceptionMessages.DirectorAlreadyExists, director.FirstName, director.LastName), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingDirectorReturnsViewModel()
        {
            var director = new DirectorCreateInputModel
            {
                FirstName = "James",
                LastName = "Cameron",
            };

            var viewModel = await this.directorsService.CreateAsync(director);
            var dbEntry = await this.directorsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.FirstName, viewModel.FirstName);
            Assert.Equal(dbEntry.LastName, viewModel.LastName);
        }

        [Fact]
        public async Task CheckIfDeletingDirectorWorksCorrectly()
        {
            this.SeedDatabase();

            await this.directorsService.DeleteByIdAsync(this.firstDirector.Id);

            var count = await this.directorsRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingDirectorReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.directorsService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.DirectorNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingDirectorWorksCorrectly()
        {
            this.SeedDatabase();

            var directorEditViewModel = new DirectorEditViewModel
            {
                Id = this.firstDirector.Id,
                FirstName = "Changed Director first name",
                LastName = "Changed Director last name",
            };

            await this.directorsService.EditAsync(directorEditViewModel);

            Assert.Equal(directorEditViewModel.FirstName, this.firstDirector.FirstName);
            Assert.Equal(directorEditViewModel.LastName, this.firstDirector.LastName);
        }

        [Fact]
        public async Task CheckIfEditingDirectorReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var directorEditViewModel = new DirectorEditViewModel
            {
                Id = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.directorsService.EditAsync(directorEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.DirectorNotFound, directorEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllDirectorsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.directorsService.GetAllDirectorsAsync<DirectorDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetDirectorViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new DirectorDetailsViewModel
            {
                Id = this.firstDirector.Id,
                FirstName = this.firstDirector.FirstName,
                LastName = this.firstDirector.LastName,
            };

            var viewModel = await this.directorsService.GetViewModelByIdAsync<DirectorDetailsViewModel>(this.firstDirector.Id);

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
                    await this.directorsService.GetViewModelByIdAsync<DirectorDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.DirectorNotFound, 3), exception.Message);
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

            this.directorsRepository = new EfDeletableEntityRepository<Director>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstDirector = new Director
            {
                FirstName = "Pesho",
                LastName = "Peshov",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedDirectors();
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
