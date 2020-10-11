namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.ViewModels.Settings;
    using CinemaWorld.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using Xunit;

    // Test class with example for mocking
    public class SettingsServiceTests
    {
        private CinemaWorldDbContext dbContext;

        public SettingsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabase();
        }

        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            var repository = new Mock<IDeletableEntityRepository<Setting>>();
            repository.Setup(r => r.All()).Returns(new List<Setting>
                                                        {
                                                            new Setting(),
                                                            new Setting(),
                                                            new Setting(),
                                                        }.AsQueryable());
            var service = new SettingsService(repository.Object);
            Assert.Equal(3, service.GetCount());
            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task GetGetAllShouldWorkCorrectly()
        {
            this.dbContext.Settings.Add(new Setting());
            this.dbContext.Settings.Add(new Setting());

            await this.dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Setting>(this.dbContext);
            var service = new SettingsService(repository);
            var settings = service.GetAll<SettingViewModel>();
            Assert.Equal(2, settings.Count());
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            this.dbContext.Settings.Add(new Setting());
            this.dbContext.Settings.Add(new Setting());
            this.dbContext.Settings.Add(new Setting());

            await this.dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Setting>(this.dbContext);
            var service = new SettingsService(repository);
            Assert.Equal(3, service.GetCount());
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));

        private void InitializeDatabase()
        {
            var options = new DbContextOptionsBuilder<CinemaWorldDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new CinemaWorldDbContext(options);
        }
    }
}
