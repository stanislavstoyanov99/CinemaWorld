namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class TicketsServiceTests : IDisposable
    {
        private readonly ITicketsService ticketsService;
        private EfDeletableEntityRepository<Ticket> ticketsRepository;
        private EfDeletableEntityRepository<Seat> seatsRepository;
        private EfDeletableEntityRepository<MovieProjection> movieProjectionsRepository;
        private SqliteConnection connection;

        private Ticket firstTicket;

        public TicketsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();

            this.ticketsService = new TicketsService(this.ticketsRepository, this.seatsRepository, this.movieProjectionsRepository);
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
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
