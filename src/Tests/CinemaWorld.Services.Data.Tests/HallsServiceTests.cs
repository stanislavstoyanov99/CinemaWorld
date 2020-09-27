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
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Halls;
    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class HallsServiceTests : IDisposable
    {
        private readonly IHallsService hallsService;
        private EfDeletableEntityRepository<Hall> hallsRepository;
        private SqliteConnection connection;

        private Hall firstHall;

        public HallsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.hallsService = new HallsService(this.hallsRepository);
        }

        [Fact]
        public async Task TestAddingHall()
        {
            var model = new HallCreateInputModel
            {
                Category = HallCategory.Large.ToString(),
                Capacity = 100,
            };

            await this.hallsService.CreateAsync(model);
            var count = await this.hallsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfHallProperties()
        {
            var model = new HallCreateInputModel
            {
                Category = HallCategory.Large.ToString(),
                Capacity = 50,
            };

            await this.hallsService.CreateAsync(model);

            var hall = await this.hallsRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Large", hall.Category.ToString());
            Assert.Equal(50, hall.Capacity);
        }

        [Fact]
        public async Task CheckIfAddingHallThrowsArgumentException()
        {
            this.SeedDatabase();

            var hall = new HallCreateInputModel
            {
                Category = HallCategory.Small.ToString(),
                Capacity = 20,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.hallsService.CreateAsync(hall));
            Assert.Equal(string.Format(ExceptionMessages.HallAlreadyExists, hall.Category, hall.Capacity), exception.Message);
        }

        [Fact]
        public async Task CheckAddingHallWithInvalidCategoryType()
        {
            this.SeedDatabase();

            var hall = new HallCreateInputModel
            {
                Category = HallCategory.Small.ToString() + "Invalid type",
                Capacity = 20,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.hallsService.CreateAsync(hall));
            Assert.Equal(string.Format(ExceptionMessages.InvalidHallCategoryType, hall.Category), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingHallReturnsViewModel()
        {
            var hall = new HallCreateInputModel
            {
                Category = HallCategory.Medium.ToString(),
                Capacity = 30,
            };

            var viewModel = await this.hallsService.CreateAsync(hall);
            var dbEntry = await this.hallsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Category, viewModel.Category);
            Assert.Equal(dbEntry.Capacity, viewModel.Capacity);
        }

        [Fact]
        public async Task CheckIfDeletingHallWorksCorrectly()
        {
            this.SeedDatabase();

            await this.hallsService.DeleteByIdAsync(this.firstHall.Id);

            var count = await this.hallsRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingHallReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.hallsService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.HallNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingHallWorksCorrectly()
        {
            this.SeedDatabase();

            var hallEditViewModel = new HallEditViewModel
            {
                Id = this.firstHall.Id,
                Category = HallCategory.Medium.ToString(),
                Capacity = 35,
            };

            await this.hallsService.EditAsync(hallEditViewModel);

            Assert.Equal(hallEditViewModel.Category, this.firstHall.Category.ToString());
            Assert.Equal(hallEditViewModel.Capacity, this.firstHall.Capacity);
        }

        [Fact]
        public async Task CheckIfEditingHallReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var hallEditViewModel = new HallEditViewModel
            {
                Id = 3,
                Category = HallCategory.Small.ToString(),
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.hallsService.EditAsync(hallEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.HallNotFound, hallEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckEditingHallWithInvalidCategoryType()
        {
            this.SeedDatabase();

            var hallEditViewModel = new HallEditViewModel
            {
                Id = this.firstHall.Id,
                Category = "Invalid category",
                Capacity = 10,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.hallsService.EditAsync(hallEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.InvalidHallCategoryType, hallEditViewModel.Category), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllHallsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.hallsService.GetAllHallsAsync<HallDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetHallViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new HallDetailsViewModel
            {
                Id = this.firstHall.Id,
                Category = this.firstHall.Category,
                Capacity = this.firstHall.Capacity,
            };

            var viewModel = await this.hallsService.GetViewModelByIdAsync<HallDetailsViewModel>(this.firstHall.Id);

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.hallsService.GetViewModelByIdAsync<HallDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.HallNotFound, 3), exception.Message);
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

            this.hallsRepository = new EfDeletableEntityRepository<Hall>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstHall = new Hall
            {
                Category = HallCategory.Small,
                Capacity = 20,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedHalls();
        }

        private async Task SeedHalls()
        {
            await this.hallsRepository.AddAsync(this.firstHall);

            await this.hallsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
