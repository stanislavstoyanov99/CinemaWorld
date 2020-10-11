namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Privacy;
    using CinemaWorld.Models.ViewModels.Privacy;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class PrivacyServiceTests : IDisposable
    {
        private readonly IPrivacyService privacyService;
        private EfDeletableEntityRepository<Privacy> privaciesRepository;
        private SqliteConnection connection;

        private Privacy firstPrivacy;

        public PrivacyServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.privacyService = new PrivacyService(this.privaciesRepository);
        }

        [Fact]
        public async Task TestAddingPrivacy()
        {
            var model = new PrivacyCreateInputModel
            {
                PageContent = "Some test page content",
            };

            await this.privacyService.CreateAsync(model);
            var count = await this.privaciesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfPrivacyProperties()
        {
            var model = new PrivacyCreateInputModel
            {
                PageContent = "Test page content",
            };

            await this.privacyService.CreateAsync(model);

            var privacy = await this.privaciesRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Test page content", privacy.PageContent);
        }

        [Fact]
        public async Task CheckIfAddingPrivacyThrowsArgumentException()
        {
            this.SeedDatabase();

            var privacy = new PrivacyCreateInputModel
            {
                PageContent = "Some page content here",
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.privacyService.CreateAsync(privacy));
            Assert.Equal(string.Format(ExceptionMessages.PrivacyAlreadyExists, privacy.PageContent), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingPrivacyReturnsViewModel()
        {
            var privacy = new PrivacyCreateInputModel
            {
                PageContent = "Information about privacy rules of website",
            };

            var viewModel = await this.privacyService.CreateAsync(privacy);
            var dbEntry = await this.privaciesRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel.Id);
            Assert.Equal(dbEntry.PageContent, viewModel.PageContent);
        }

        [Fact]
        public async Task CheckIfDeletingPrivacyWorksCorrectly()
        {
            this.SeedDatabase();

            await this.privacyService.DeleteByIdAsync(this.firstPrivacy.Id);

            var count = await this.privaciesRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingPrivacyReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.privacyService.DeleteByIdAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.PrivacyNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfEditingPrivacyWorksCorrectly()
        {
            this.SeedDatabase();

            var privacyEditViewModel = new PrivacyEditViewModel
            {
                Id = this.firstPrivacy.Id,
                PageContent = "Changed page content of privacy",
            };

            await this.privacyService.EditAsync(privacyEditViewModel);

            Assert.Equal(privacyEditViewModel.PageContent, this.firstPrivacy.PageContent);
        }

        [Fact]
        public async Task CheckIfEditingPrivacyReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var privacyEditViewModel = new PrivacyEditViewModel
            {
                Id = 3,
            };

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.privacyService.EditAsync(privacyEditViewModel));
            Assert.Equal(string.Format(ExceptionMessages.PrivacyNotFound, privacyEditViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetPrivacyViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new PrivacyDetailsViewModel
            {
                Id = this.firstPrivacy.Id,
                PageContent = this.firstPrivacy.PageContent,
            };

            var viewModel = await this.privacyService.GetViewModelByIdAsync<PrivacyDetailsViewModel>(this.firstPrivacy.Id);

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
                    await this.privacyService.GetViewModelByIdAsync<PrivacyDetailsViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.PrivacyNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetViewModelAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new PrivacyDetailsViewModel
            {
                Id = this.firstPrivacy.Id,
                PageContent = this.firstPrivacy.PageContent,
            };

            var viewModel = await this.privacyService.GetViewModelAsync<PrivacyDetailsViewModel>();

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelThrowsNullReferenceException()
        {
            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () =>
                    await this.privacyService.GetViewModelAsync<PrivacyDetailsViewModel>());
            Assert.Equal(ExceptionMessages.PrivacyViewModelNotFound, exception.Message);
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

            this.privaciesRepository = new EfDeletableEntityRepository<Privacy>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstPrivacy = new Privacy
            {
                PageContent = "Some page content here",
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedPrivacies();
        }

        private async Task SeedPrivacies()
        {
            await this.privaciesRepository.AddAsync(this.firstPrivacy);

            await this.privaciesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
