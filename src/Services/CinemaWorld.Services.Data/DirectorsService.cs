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

        // TODO
        public Task<DirectorViewModel> CreateAsync(DirectorCreateInputModel directorCreateInputModel)
        {
            throw new NotImplementedException();
        }

        // TODO
        public Task DeleteByIdAsync(int id)
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
