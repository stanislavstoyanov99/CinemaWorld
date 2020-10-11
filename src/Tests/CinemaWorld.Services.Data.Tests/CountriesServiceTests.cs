namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Countries;
    using CinemaWorld.Models.ViewModels.Countries;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

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

        [Fact]
        public async Task TestAddingCountry()
        {
            var model = new CountryCreateInputModel
            {
                Name = "China",
            };

            await this.countriesService.CreateAsync(model);
            var count = await this.countriesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfCountryProperties()
        {
            var model = new CountryCreateInputModel
            {
                Name = "Germany",
            };

            await this.countriesService.CreateAsync(model);

            var country = await this.countriesRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Germany", country.Name);
        }

        [Fact]
        public async Task CheckIfAddingCountryThrowsArgumentException()
        {
            this.SeedDatabase();

            var country = new CountryCreateInputModel
            {
                Name = "Bulgaria",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.countriesService.CreateAsync(country));
            Assert.Equal(string.Format(ExceptionMessages.CountryAlreadyExists, country.Name), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingCountryReturnsViewModel()
        {
            var country = new CountryCreateInputModel
            {
                Name = "Germany",
            };

            var viewModel = await this.countriesService.CreateAsync(country);
            var dbEntry = await this.countriesRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.Name, viewModel.Name);
        }

        [Fact]
        public async Task CheckIfDeletingCountryWorksCorrectly()
        {
            this.SeedDatabase();

            await this.countriesService.DeleteByIdAsync(this.firstCountry.Id);

            var count = await this.countriesRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingCountryReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.countriesService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.CountryNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingCountryWorksCorrectly()
        {
            this.SeedDatabase();

            var countryEditViewModel = new CountryEditViewModel
            {
                Id = this.firstCountry.Id,
                Name = "Changed Country name",
            };

            await this.countriesService.EditAsync(countryEditViewModel);

            Assert.Equal(countryEditViewModel.Name, this.firstCountry.Name);
        }

        [Fact]
        public async Task CheckIfEditingCountryReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var countryEditViewModel = new CountryEditViewModel
            {
                Id = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.countriesService.EditAsync(countryEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.CountryNotFound, countryEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllCountriesAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.countriesService.GetAllCountriesAsync<CountryDetailsViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetCountryViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new CountryDetailsViewModel
            {
                Id = this.firstCountry.Id,
                Name = this.firstCountry.Name,
            };

            var viewModel = await this.countriesService.GetViewModelByIdAsync<CountryDetailsViewModel>(this.firstCountry.Id);

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
                    await this.countriesService.GetViewModelByIdAsync<CountryDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.CountryNotFound, 3), exception.Message);
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
            this.firstCountry = new Country
            {
                Name = "Bulgaria",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedCountries();
        }

        private async Task SeedCountries()
        {
            await this.countriesRepository.AddAsync(this.firstCountry);

            await this.countriesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
