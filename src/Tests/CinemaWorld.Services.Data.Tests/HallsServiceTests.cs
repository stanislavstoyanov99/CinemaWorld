namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Halls;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

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
