namespace CinemaWorld.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Contacts;
    using CinemaWorld.Models.ViewModels.Contacts;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;
    using CinemaWorld.Services.Messaging;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class ContactsServiceTests : IDisposable, IClassFixture<Configuration>
    {
        private readonly IEmailSender emailSender;
        private readonly IContactsService contactsService;

        private EfRepository<ContactFormEntry> userContactsRepository;
        private EfRepository<AdminContactFromEntry> adminContactsRepository;
        private SqliteConnection connection;

        private ContactFormEntry firstUserContactFormEntry;
        private AdminContactFromEntry firstAdminContactFormEntry;

        public ContactsServiceTests(Configuration configuration)
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.emailSender = new SendGridEmailSender(configuration.ConfigurationRoot["SendGrid:ApiKey"]);
            this.contactsService = new ContactsService(
                this.userContactsRepository,
                this.adminContactsRepository,
                this.emailSender);
        }

        [Fact]
        public async Task CheckIfGetAllUserEmailsAsyncWorksCorrectly()
        {
            await this.SeedUserContacts();

            var result = await this.contactsService.GetAllUserEmailsAsync<UserEmailViewModel>();

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfSendContactToAdminWorksCorrectly()
        {
            var model = new ContactFormEntryViewModel
            {
                FirstName = "Peter",
                LastName = "Kirov",
                Email = "peter.kirov@abv.bg",
                Subject = "Question about cinema news",
                Content = "Sample content about cinema news",
            };

            await this.contactsService.SendContactToAdmin(model);
            var count = await this.userContactsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfSendContactToUserWorksCorrectly()
        {
            var model = new SendContactInputModel
            {
                FullName = "Administrator fullname",
                Email = "admin@abv.bg",
                Subject = "Answer about cinema news",
                Content = "Sample content about cinema news",
            };

            await this.contactsService.SendContactToUser(model);
            var count = await this.adminContactsRepository.All().CountAsync();

            Assert.Equal(1, count);
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

            this.userContactsRepository = new EfRepository<ContactFormEntry>(dbContext);
            this.adminContactsRepository = new EfRepository<AdminContactFromEntry>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstUserContactFormEntry = new ContactFormEntry
            {
                FirstName = "Stanislav",
                LastName = "Stoyanov",
                Email = "slavkata_99@abv.bg",
                Subject = "I have a question",
                Content = "I have to ask you something connected with the system.",
            };

            this.firstAdminContactFormEntry = new AdminContactFromEntry
            {
                FullName = "Administrator fullname",
                Email = "slavi_biserov@mail.bg",
                Subject = "My answer to your question",
                Content = "Feel free to ask any other questions about the system.",
            };
        }

        private async Task SeedUserContacts()
        {
            await this.userContactsRepository.AddAsync(this.firstUserContactFormEntry);

            await this.userContactsRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("CinemaWorld.Models.ViewModels"));
    }
}
