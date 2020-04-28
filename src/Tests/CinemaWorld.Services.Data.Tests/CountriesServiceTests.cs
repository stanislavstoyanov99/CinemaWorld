namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class CountriesServiceTests : IDisposable
    {
        private readonly ICountriesService countriesService;
        private EfDeletableEntityRepository<Country> countriesRepository;
        private SqliteConnection connection;

        private Country firstCountry;

        public CountriesServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.countriesService = new CountriesService(this.countriesRepository);
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

            this.countriesRepository = new EfDeletableEntityRepository<Country>(dbContext);
        }

        private void InitializeFields()
        {
            // TODO
        }

        private async void SeedDatabase()
        {
            await this.SeedFaqEntries();
        }

        private async Task SeedFaqEntries()
        {
            // TODO await this.countriesRepository.AddAsync();

            await this.countriesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));

    }
}
