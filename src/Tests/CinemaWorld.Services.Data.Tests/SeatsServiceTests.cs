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
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Seats;
    using CinemaWorld.Models.ViewModels.Seats;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
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

        [Fact]
        public async Task CheckIfDeletingSeatWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            await this.seatsService.DeleteByIdAsync(this.firstSeat.Id);

            var count = await this.seatsRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingSeatReturnsNullReferenceException()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.seatsService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.SeatNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingSeatWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var seatEditViewModel = new SeatEditViewModel
            {
                Id = this.firstSeat.Id,
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = SeatCategory.Wheelchair.ToString(),
            };

            await this.seatsService.EditAsync(seatEditViewModel);

            Assert.Equal(seatEditViewModel.Number, this.firstSeat.Number);
            Assert.Equal(seatEditViewModel.RowNumber, this.firstSeat.RowNumber);
            Assert.Equal(seatEditViewModel.HallId, this.firstSeat.HallId);
            Assert.Equal(seatEditViewModel.Category, this.firstSeat.Category.ToString());
        }

        [Fact]
        public async Task CheckEditingSeatWithInvalidSeatCategoryType()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var seatEditViewModel = new SeatEditViewModel
            {
                Id = this.firstSeat.Id,
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = "Invalid category",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.seatsService.EditAsync(seatEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.InvalidSeatCategoryType, seatEditViewModel.Category), exception.Message);
        }

        [Fact]
        public async Task CheckEditingSeatWithMissingSeat()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var seatEditViewModel = new SeatEditViewModel
            {
                Id = 3,
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = SeatCategory.Wheelchair.ToString(),
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.seatsService.EditAsync(seatEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.SeatNotFound, seatEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckEditingSeatWithMissingHall()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var seatEditViewModel = new SeatEditViewModel
            {
                Id = this.firstSeat.Id,
                Number = 2,
                RowNumber = 2,
                HallId = 2,
                Category = SeatCategory.Wheelchair.ToString(),
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.seatsService.EditAsync(seatEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.HallNotFound, seatEditViewModel.HallId), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllSeatsAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var secondSeat = new Seat
            {
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = SeatCategory.Normal,
            };
            await this.seatsRepository.AddAsync(secondSeat);
            await this.seatsRepository.SaveChangesAsync();

            var result = await this.seatsService.GetAllSeatsAsync<SeatDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(2, count);
            Assert.Equal(1, result.First().Number);
        }

        [Fact]
        public async Task CheckIfGetAllSoldSeatsAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var secondSeat = new Seat
            {
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = SeatCategory.Normal,
                IsSold = true,
            };
            await this.seatsRepository.AddAsync(secondSeat);
            await this.seatsRepository.SaveChangesAsync();

            var result = await this.seatsService.GetAllSoldSeatsAsync(1);

            var count = result.Count();
            Assert.Equal(1, count);
            Assert.Equal("2_2", result.First());
        }

        [Fact]
        public async Task CheckIfGetAllAvailableSeatsAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var secondSeat = new Seat
            {
                Number = 2,
                RowNumber = 2,
                HallId = 1,
                Category = SeatCategory.Normal,
                IsSold = false,
                IsAvailable = true,
            };
            await this.seatsRepository.AddAsync(secondSeat);
            await this.seatsRepository.SaveChangesAsync();

            var result = await this.seatsService.GetAllAvailableSeatsAsync(1);

            var count = result.Count();
            Assert.Equal(1, count);
            Assert.Equal("aaaaaaaaaa", result.First());
        }

        [Fact]
        public async Task CheckIfGetAllAvailableSeatsAsyncWorksCorrectlyWithMoreSeats()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            for (int i = 2; i <= 20; i++)
            {
                var seat = new Seat
                {
                    Number = i,
                    RowNumber = i,
                    HallId = 1,
                    Category = SeatCategory.Normal,
                    IsSold = false,
                    IsAvailable = true,
                };
                await this.seatsRepository.AddAsync(seat);
                await this.seatsRepository.SaveChangesAsync();
            }

            var result = await this.seatsService.GetAllAvailableSeatsAsync(1);

            var count = result.Count();
            Assert.Equal(2, count);
            Assert.Equal("aaaaaaaaaa", result.First());
            Assert.Equal("aaaaaaaaaa", result.Last());
        }

        [Fact]
        public async Task CheckIfGetAllSeatsAsQueryeableWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var result = this.seatsService.GetAllSeatsAsQueryeable<SeatDetailsViewModel>();

            var count = await result.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var expectedModel = new SeatDetailsViewModel
            {
                Id = this.firstSeat.Id,
                RowNumber = this.firstSeat.RowNumber,
                Number = this.firstSeat.Number,
                IsAvailable = this.firstSeat.IsAvailable,
                IsSold = this.firstSeat.IsSold,
                Category = this.firstSeat.Category,
            };

            var viewModel = await this.seatsService.GetViewModelByIdAsync<SeatDetailsViewModel>(this.firstSeat.Id);
            expectedModel.Hall = viewModel.Hall;

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();
            await this.SeedSeats();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.seatsService.GetViewModelByIdAsync<SeatDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.SeatNotFound, 3), exception.Message);
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
