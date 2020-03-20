namespace CinemaWorld.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Directors;
    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class DirectorsService : IDirectorsService
    {
        private readonly IDeletableEntityRepository<Director> directorsRepository;

        public DirectorsService(IDeletableEntityRepository<Director> directorsRepository)
        {
            this.directorsRepository = directorsRepository;
        }

        public async Task<DirectorViewModel> CreateAsync(DirectorCreateInputModel directorCreateInputModel)
        {
            var director = new Director
            {
                FirstName = directorCreateInputModel.FirstName,
                LastName = directorCreateInputModel.LastName,
            };

            bool doesDirectorExist = await this.directorsRepository.All().AnyAsync(x => x.Id == director.Id);
            if (doesDirectorExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.GenreAlreadyExists, director.Id));
            }

            await this.directorsRepository.AddAsync(director);
            await this.directorsRepository.SaveChangesAsync();

            var viewModel = this.GetViewModelByIdAsync<DirectorViewModel>(director.Id)
                .GetAwaiter()
                .GetResult();

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var director = this.directorsRepository.All().FirstOrDefault(x => x.Id == id);
            if (director == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.DirectorNotFound, id));
            }

            director.IsDeleted = true;
            director.DeletedOn = DateTime.UtcNow;
            this.directorsRepository.Update(director);
            await this.directorsRepository.SaveChangesAsync();
        }

        // TODO
        public Task EditAsync(DirectorEditViewModel directorEditViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TViewModel>> GetAllDirectorsAsync<TViewModel>()
        {
            var directors = await this.directorsRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return directors;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var director = await this.directorsRepository
                .All()
                .Where(d => d.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (director == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.DirectorNotFound, id));
            }

            return director;
        }
    }
}
