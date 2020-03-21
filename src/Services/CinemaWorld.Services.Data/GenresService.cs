namespace CinemaWorld.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Genres;
    using CinemaWorld.Models.ViewModels.Genres;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class GenresService : IGenresService
    {
        private readonly IDeletableEntityRepository<Genre> genresRepository;

        public GenresService(IDeletableEntityRepository<Genre> genresRepository)
        {
            this.genresRepository = genresRepository;
        }

        public async Task<GenreDetailsViewModel> CreateAsync(GenreCreateInputModel genreCreateInputModel)
        {
            var genre = new Genre
            {
                Name = genreCreateInputModel.Name,
            };

            bool doesGenreExist = await this.genresRepository.All().AnyAsync(x => x.Id == genre.Id);
            if (doesGenreExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.GenreAlreadyExists, genre.Id));
            }

            await this.genresRepository.AddAsync(genre);
            await this.genresRepository.SaveChangesAsync();

            var viewModel = this.GetViewModelByIdAsync<GenreDetailsViewModel>(genre.Id)
                .GetAwaiter()
                .GetResult();

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var genre = this.genresRepository.All().FirstOrDefault(x => x.Id == id);
            if (genre == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.GenreNotFound, id));
            }

            genre.IsDeleted = true;
            genre.DeletedOn = DateTime.UtcNow;
            this.genresRepository.Update(genre);
            await this.genresRepository.SaveChangesAsync();
        }

        public async Task EditAsync(GenreEditViewModel genreEditViewModel)
        {
            var genre = this.genresRepository.All().FirstOrDefault(g => g.Id == genreEditViewModel.Id);

            if (genre == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.GenreNotFound, genreEditViewModel.Id));
            }

            genre.Name = genreEditViewModel.Name;
            genre.ModifiedOn = DateTime.UtcNow;

            this.genresRepository.Update(genre);
            await this.genresRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllGenresAsync<TViewModel>()
        {
            var genres = await this.genresRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return genres;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var genreViewModel = await this.genresRepository
                .All()
                .Where(m => m.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (genreViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.GenreNotFound, id));
            }

            return genreViewModel;
        }
    }
}
