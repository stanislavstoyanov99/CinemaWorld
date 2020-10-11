namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.ViewModels.Tickets;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class TicketsServiceTests : IDisposable
    {
        private readonly ITicketsService ticketsService;
        private EfDeletableEntityRepository<Ticket> ticketsRepository;
        private EfDeletableEntityRepository<Seat> seatsRepository;
        private EfDeletableEntityRepository<MovieProjection> movieProjectionsRepository;
        private EfDeletableEntityRepository<Hall> hallsRepository;
        private EfDeletableEntityRepository<Cinema> cinemasRepository;
        private EfDeletableEntityRepository<Director> directorsRepository;
        private EfDeletableEntityRepository<Movie> moviesRepository;
        private SqliteConnection connection;

        private Ticket firstTicket;
        private MovieProjection firstMovieProjection;
        private Seat firstSeat;
        private Hall firstHall;
        private Cinema firstCinema;
        private Movie firstMovie;
        private Director firstDirector;

        public TicketsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.ticketsService = new TicketsService(this.ticketsRepository, this.seatsRepository, this.movieProjectionsRepository);
        }

        [Fact]
        public async Task CheckIfBuyAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var model = new TicketInputModel
            {
                Row = 1,
                Seat = this.firstSeat.Id,
                Price = 15,
                TimeSlot = TimeSlot.Afternoon.ToString(),
                TicketType = TicketType.Regular.ToString(),
                Quantity = 2,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            await this.ticketsService.BuyAsync(model, this.firstMovieProjection.Id);
            var count = await this.ticketsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfTicketProperties()
        {
            this.SeedDatabase();

            var model = new TicketInputModel
            {
                Row = 1,
                Seat = this.firstSeat.Id,
                Price = 20,
                TimeSlot = TimeSlot.Morning.ToString(),
                TicketType = TicketType.ForChildren.ToString(),
                Quantity = 1,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            await this.ticketsService.BuyAsync(model, this.firstMovieProjection.Id);

            var ticket = await this.ticketsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(1, ticket.Row);
            Assert.Equal(this.firstSeat.Id, ticket.Seat);
            Assert.Equal(20, ticket.Price);
            Assert.Equal("Afternoon", ticket.TimeSlot.ToString());
            Assert.Equal("ForChildren", ticket.TicketType.ToString());
            Assert.Equal(1, ticket.Quantity);
        }

        [Fact]
        public async Task CheckAddingTicketWithInvalidTicketType()
        {
            this.SeedDatabase();

            var ticket = new TicketInputModel
            {
                Row = 1,
                Seat = this.firstSeat.Id,
                Price = 20,
                TimeSlot = TimeSlot.Morning.ToString(),
                TicketType = "Invalid type",
                Quantity = 1,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.ticketsService.BuyAsync(ticket, this.firstMovieProjection.Id));
            Assert.Equal(string.Format(ExceptionMessages.InvalidTicketType, ticket.TicketType), exception.Message);
        }

        [Fact]
        public async Task CheckAddingTicketWithMissingMovieProjection()
        {
            this.SeedDatabase();

            var ticket = new TicketInputModel
            {
                Row = 1,
                Seat = this.firstSeat.Id,
                Price = 20,
                TimeSlot = TimeSlot.Morning.ToString(),
                TicketType = TicketType.ForChildren.ToString(),
                Quantity = 1,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.ticketsService.BuyAsync(ticket, 3));
            Assert.Equal(string.Format(ExceptionMessages.MovieProjectionNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckAddingTicketWithMissingSeat()
        {
            this.SeedDatabase();

            var ticket = new TicketInputModel
            {
                Row = 3,
                Seat = 3,
                Price = 20,
                TimeSlot = TimeSlot.Morning.ToString(),
                TicketType = TicketType.ForChildren.ToString(),
                Quantity = 1,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.ticketsService.BuyAsync(ticket, this.firstMovieProjection.Id));
            Assert.Equal(string.Format(ExceptionMessages.SeatNotFound, ticket.Seat), exception.Message);
        }

        [Fact]
        public async Task CheckAddingTicketWithSoldSeat()
        {
            this.SeedDatabase();

            this.firstSeat.IsSold = true;

            var ticket = new TicketInputModel
            {
                Row = 1,
                Seat = this.firstSeat.Id,
                Price = 10,
                TimeSlot = TimeSlot.Еvening.ToString(),
                TicketType = TicketType.Retired.ToString(),
                Quantity = 2,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.ticketsService.BuyAsync(ticket, this.firstMovieProjection.Id));
            Assert.Equal(string.Format(ExceptionMessages.SeatSoldError, this.firstSeat.Id), exception.Message);
        }

        [Fact]
        public async Task CheckAddingAlreadyExistingTicket()
        {
            this.SeedDatabase();
            await this.SeedTickets();

            var ticket = new TicketInputModel
            {
                Row = this.firstTicket.Row,
                Seat = this.firstTicket.Seat,
                Price = this.firstTicket.Price,
                TimeSlot = this.firstTicket.TimeSlot.ToString(),
                TicketType = this.firstTicket.TicketType.ToString(),
                Quantity = this.firstTicket.Quantity,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.ticketsService.BuyAsync(ticket, this.firstMovieProjection.Id));
            Assert.Equal(string.Format(ExceptionMessages.TicketAlreadyExists, ticket.Row, ticket.Seat), exception.Message);
        }

        [Fact]
        public async Task CheckIfBuyingTicketReturnsViewModel()
        {
            this.SeedDatabase();

            var ticket = new TicketInputModel
            {
                Row = this.firstTicket.Row,
                Seat = this.firstTicket.Seat,
                Price = this.firstTicket.Price,
                TimeSlot = this.firstTicket.TimeSlot.ToString(),
                TicketType = this.firstTicket.TicketType.ToString(),
                Quantity = this.firstTicket.Quantity,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            var viewModel = await this.ticketsService.BuyAsync(ticket, this.firstMovieProjection.Id);
            var dbEntry = await this.ticketsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Row, viewModel.Row);
            Assert.Equal(dbEntry.Seat, viewModel.Seat);
            Assert.Equal(dbEntry.Price, viewModel.Price);
            Assert.Equal(dbEntry.TimeSlot, viewModel.TimeSlot);
            Assert.Equal(dbEntry.TicketType, viewModel.TicketType);
            Assert.Equal(dbEntry.Quantity, viewModel.Quantity);
        }

        [Fact]
        public async Task CheckIfBuyAsyncWorksCorrectlyWithLunchTimeSlot()
        {
            this.SeedDatabase();

            var dateAsString = "30/09/2020 10:31:00";
            var newDate = DateTime.ParseExact(dateAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            this.firstMovieProjection.Date = newDate;

            var model = new TicketInputModel
            {
                Row = this.firstTicket.Row,
                Seat = this.firstSeat.Id,
                Price = this.firstTicket.Price,
                TicketType = this.firstTicket.TicketType.ToString(),
                Quantity = this.firstTicket.Quantity,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            await this.ticketsService.BuyAsync(model, this.firstMovieProjection.Id);
            var ticket = await this.ticketsRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Lunch", ticket.TimeSlot.ToString());
        }

        [Fact]
        public async Task CheckIfBuyAsyncWorksCorrectlyWithЕveningTimeSlot()
        {
            this.SeedDatabase();

            var dateAsString = "30/09/2020 17:30:00";
            var newDate = DateTime.ParseExact(dateAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            this.firstMovieProjection.Date = newDate;

            var model = new TicketInputModel
            {
                Row = this.firstTicket.Row,
                Seat = this.firstSeat.Id,
                Price = this.firstTicket.Price,
                TicketType = this.firstTicket.TicketType.ToString(),
                Quantity = this.firstTicket.Quantity,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            await this.ticketsService.BuyAsync(model, this.firstMovieProjection.Id);
            var ticket = await this.ticketsRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Еvening", ticket.TimeSlot.ToString());
        }

        [Fact]
        public async Task CheckIfBuyAsyncWorksCorrectlyWithMorningTimeSlot()
        {
            this.SeedDatabase();

            var dateAsString = "30/09/2020 09:30:00";
            var newDate = DateTime.ParseExact(dateAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            this.firstMovieProjection.Date = newDate;

            var model = new TicketInputModel
            {
                Row = this.firstTicket.Row,
                Seat = this.firstSeat.Id,
                Price = this.firstTicket.Price,
                TicketType = this.firstTicket.TicketType.ToString(),
                Quantity = this.firstTicket.Quantity,
                MovieProjectionTime = this.firstMovieProjection.Date,
            };

            await this.ticketsService.BuyAsync(model, this.firstMovieProjection.Id);
            var ticket = await this.ticketsRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Morning", ticket.TimeSlot.ToString());
        }

        [Fact]
        public async Task CheckIfDeletingTicketWorksCorrectly()
        {
            await this.SeedTickets();

            await this.ticketsService.DeleteByIdAsync(this.firstTicket.Id);

            var count = await this.ticketsRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingTicketReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.ticketsService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.TicketNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetTicketViewModelByIdAsyncWorksCorrectly()
        {
            await this.SeedTickets();

            var expectedModel = new TicketDetailsViewModel
            {
                Row = this.firstTicket.Row,
                Seat = this.firstTicket.Seat,
                Price = this.firstTicket.Price,
                TimeSlot = this.firstTicket.TimeSlot,
                TicketType = this.firstTicket.TicketType,
                Quantity = this.firstTicket.Quantity,
            };

            var viewModel = await this.ticketsService.GetViewModelByIdAsync<TicketDetailsViewModel>(this.firstTicket.Id);

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            await this.SeedTickets();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.ticketsService.GetViewModelByIdAsync<TicketDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.TicketNotFound, 3), exception.Message);
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

            this.ticketsRepository = new EfDeletableEntityRepository<Ticket>(dbContext);
            this.seatsRepository = new EfDeletableEntityRepository<Seat>(dbContext);
            this.movieProjectionsRepository = new EfDeletableEntityRepository<MovieProjection>(dbContext);
            this.hallsRepository = new EfDeletableEntityRepository<Hall>(dbContext);
            this.cinemasRepository = new EfDeletableEntityRepository<Cinema>(dbContext);
            this.moviesRepository = new EfDeletableEntityRepository<Movie>(dbContext);
            this.directorsRepository = new EfDeletableEntityRepository<Director>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstTicket = new Ticket
            {
                Row = 1,
                Seat = 1,
                Price = 10,
                TimeSlot = TimeSlot.Afternoon,
                TicketType = TicketType.Regular,
                Quantity = 1,
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

            this.firstHall = new Hall
            {
                Category = HallCategory.Small,
                Capacity = 20,
            };

            this.firstCinema = new Cinema
            {
                Name = "Bulgaria Mall",
                Address = "Sofia bul.Stamboliiski",
            };

            this.firstMovieProjection = new MovieProjection
            {
                Date = DateTime.ParseExact("30/09/2020 14:30:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                MovieId = 1,
                HallId = 1,
                CinemaId = 1,
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
            await this.SeedDirectors();
            await this.SeedMovies();
            await this.SeedHalls();
            await this.SeedCinemas();
            await this.SeedMovieProjections();
            await this.SeedSeats();
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

        private async Task SeedSeats()
        {
            await this.seatsRepository.AddAsync(this.firstSeat);

            await this.seatsRepository.SaveChangesAsync();
        }

        private async Task SeedTickets()
        {
            await this.ticketsRepository.AddAsync(this.firstTicket);

            await this.ticketsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
