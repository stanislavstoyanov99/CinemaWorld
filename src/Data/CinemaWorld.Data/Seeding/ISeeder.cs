namespace CinemaWorld.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(CinemaWorldDbContext dbContext, IServiceProvider serviceProvider);
    }
}
