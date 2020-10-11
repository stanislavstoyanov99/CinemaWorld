namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Cinemas;
    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class CinemasServiceTests : IDisposable
    {
        private readonly ICinemasService cinemasService;
        private EfDeletableEntityRepository<Cinema> cinemasRepository;
        private SqliteConnection connection;

        private Cinema firstCinema;

        public CinemasServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.cinemasService = new CinemasService(this.cinemasRepository);
        }

        [Fact]
        public async Task TestAddingCinema()
        {
            var model = new CinemaCreateInputModel
            {
                Name = "Mol Sofia",
                Address = "Bulgaria, Sofia",
            };

            await this.cinemasService.CreateAsync(model);
            var count = await this.cinemasRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfCinemaProperties()
        {
            var model = new CinemaCreateInputModel
            {
                Name = "Mol Plovdiv",
                Address = "Bulgaria, Plovdiv",
            };

            await this.cinemasService.CreateAsync(model);

            var cinema = await this.cinemasRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Mol Plovdiv", cinema.Name);
            Assert.Equal("Bulgaria, Plovdiv", cinema.Address);
        }

        [Fact]
        public async Task CheckIfAddingCinemaThrowsArgumentException()
        {
            this.SeedDatabase();

            var cinema = new CinemaCreateInputModel
            {
                Name = "Test Cinema Name",
                Address = "Test Cinema Address",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.cinemasService.CreateAsync(cinema));
            Assert.Equal(string.Format(ExceptionMessages.CinemaAlreadyExists, cinema.Name, cinema.Address), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingCinemaReturnsViewModel()
        {
            var cinema = new CinemaCreateInputModel
            {
                Name = "Mol Paradise",
                Address = "Bulgaria, Sofia",
            };

            var viewModel = await this.cinemasService.CreateAsync(cinema);
            var dbEntry = await this.cinemasRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Name, viewModel.Name);
            Assert.Equal(dbEntry.Address, viewModel.Address);
        }

        [Fact]
        public async Task CheckIfDeletingCinemaWorksCorrectly()
        {
            this.SeedDatabase();

            await this.cinemasService.DeleteByIdAsync(this.firstCinema.Id);

            var count = await this.cinemasRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingCinemaReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.cinemasService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.CinemaNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingCinemaWorksCorrectly()
        {
            this.SeedDatabase();

            var cinemaEditViewModel = new CinemaEditViewModel
            {
                Id = this.firstCinema.Id,
                Name = "Changed Cinema name",
                Address = "Changed Cinema address",
            };

            await this.cinemasService.EditAsync(cinemaEditViewModel);

            Assert.Equal(cinemaEditViewModel.Name, this.firstCinema.Name);
            Assert.Equal(cinemaEditViewModel.Address, this.firstCinema.Address);
        }

        [Fact]
        public async Task CheckIfEditingCinemaReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var cinemaEditViewModel = new CinemaEditViewModel
            {
                Id = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.cinemasService.EditAsync(cinemaEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.CinemaNotFound, cinemaEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllCinemasAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.cinemasService.GetAllCinemasAsync<CinemaDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetCinemaViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new CinemaDetailsViewModel
            {
                Id = this.firstCinema.Id,
                Name = this.firstCinema.Name,
                Address = this.firstCinema.Address,
            };

            var viewModel = await this.cinemasService.GetViewModelByIdAsync<CinemaDetailsViewModel>(this.firstCinema.Id);

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.cinemasService.GetViewModelByIdAsync<CinemaDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.CinemaNotFound, 3), exception.Message);
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

            this.cinemasRepository = new EfDeletableEntityRepository<Cinema>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstCinema = new Cinema
            {
                Name = "Test Cinema Name",
                Address = "Test Cinema Address",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedCinemas();
        }

        private async Task SeedCinemas()
        {
            await this.cinemasRepository.AddAsync(this.firstCinema);

            await this.cinemasRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
