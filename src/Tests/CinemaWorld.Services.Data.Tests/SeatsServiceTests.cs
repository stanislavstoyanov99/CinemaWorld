namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Seats;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class SeatsServiceTests : IDisposable
    {
        private readonly ISeatsService seatsService;
        private EfDeletableEntityRepository<Seat> seatsRepository;
        private EfDeletableEntityRepository<Hall> hallsRepository;
        private SqliteConnection connection;

        private Hall firstHall;
        private Seat firstSeat;

        public SeatsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.seatsService = new SeatsService(this.seatsRepository, this.hallsRepository);
        }

        [Fact]
        public async Task CheckIfAddingSeatWorksCorrectly()
        {
            this.SeedDatabase();

            var model = new SeatCreateInputModel
            {
                Number = 1,
                RowNumber = 1,
                HallId = 1,
                Category = SeatCategory.Normal.ToString(),
            };

            await this.seatsService.CreateAsync(model);
            var count = await this.seatsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingsOfSeatProperties()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var model = new SeatCreateInputModel
            {
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = SeatCategory.Wheelchair.ToString(),
            };

            await this.seatsService.CreateAsync(model);

            var seat = await this.seatsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(this.firstSeat.Id, seat.Id);
            Assert.Equal(this.firstSeat.Number, seat.Number);
            Assert.Equal(this.firstSeat.RowNumber, seat.RowNumber);
            Assert.Equal(this.firstSeat.HallId, seat.HallId);
            Assert.Equal(this.firstSeat.Category, seat.Category);
        }

        [Fact]
        public async Task CheckAddingSeatWithInvalidSeatCategoryType()
        {
            this.SeedDatabase();

            var seat = new SeatCreateInputModel
            {
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = "Invalid category",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.seatsService.CreateAsync(seat));
            Assert.Equal(string.Format(ExceptionMessages.InvalidSeatCategoryType, seat.Category), exception.Message);
        }

        [Fact]
        public async Task CheckAddingSeatWithMissingHall()
        {
            this.SeedDatabase();

            var seat = new SeatCreateInputModel
            {
                Number = 2,
                RowNumber = 2,
                HallId = 2,
                Category = SeatCategory.Normal.ToString(),
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.seatsService.CreateAsync(seat));
            Assert.Equal(string.Format(ExceptionMessages.HallNotFound, seat.HallId), exception.Message);
        }

        [Fact]
        public async Task CheckAddingAlreadyExistingSeat()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var seat = new SeatCreateInputModel
            {
                Number = 1,
                RowNumber = 1,
                HallId = 1,
                Category = SeatCategory.Normal.ToString(),
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.seatsService.CreateAsync(seat));
            Assert.Equal(string.Format(ExceptionMessages.SeatAlreadyExists, seat.Number, seat.RowNumber), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingSeatReturnsViewModel()
        {
            this.SeedDatabase();

            var seat = new SeatCreateInputModel
            {
                Number = 3,
                RowNumber = 3,
                HallId = 1,
                Category = SeatCategory.Normal.ToString(),
            };

            var viewModel = await this.seatsService.CreateAsync(seat);
            var dbEntry = await this.seatsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Number, viewModel.Number);
            Assert.Equal(dbEntry.RowNumber, viewModel.RowNumber);
            Assert.Equal(dbEntry.HallId, viewModel.Hall.Id);
            Assert.Equal(dbEntry.Category, viewModel.Category);
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

            this.seatsRepository = new EfDeletableEntityRepository<Seat>(dbContext);
            this.hallsRepository = new EfDeletableEntityRepository<Hall>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstHall = new Hall
            {
                Category = HallCategory.Medium,
                Capacity = 50,
            };

            this.firstSeat = new Seat
            {
                Number = 1,
                RowNumber = 1,
                HallId = 1,
                Category = SeatCategory.Normal,
                IsAvailable = true,
                IsSold = false,
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

        private async Task SeedSeats()
        {
            await this.seatsRepository.AddAsync(this.firstSeat);

            await this.seatsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
