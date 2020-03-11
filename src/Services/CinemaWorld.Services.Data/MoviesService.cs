namespace CinemaWorld.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class MoviesService : IMoviesService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Director> directorsRepository;

        public MoviesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Director> directorsRepository)
        {
            this.moviesRepository = moviesRepository;
            this.directorsRepository = directorsRepository;
        }

        public async Task<MovieViewModel> CreateAsync(MovieCreateInputModel movieCreateInputModel)
        {
            if (!Enum.TryParse(movieCreateInputModel.CinemaCategory, true, out CinemaCategory cinemaCategory))
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.InvalidCinemaCategoryType, movieCreateInputModel.CinemaCategory));
            }

            var director = await this.directorsRepository
                .All()
                .FirstOrDefaultAsync(d => d.Id == movieCreateInputModel.DirectorId);

            if (director == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.DirectorNotFound, movieCreateInputModel.DirectorId));
            }

            var movie = new Movie
            {
                Name = movieCreateInputModel.Name,
                DateOfRelease = movieCreateInputModel.DateOfRelease,
                Resolution = movieCreateInputModel.Resolution,
                Rating = movieCreateInputModel.Rating,
                Description = movieCreateInputModel.Description,
                Language = movieCreateInputModel.Language,
                CinemaCategory = cinemaCategory,
                TrailerPath = movieCreateInputModel.TrailerPath,
                CoverPath = movieCreateInputModel.CoverPath,
                IMDBLink = movieCreateInputModel.IMDBLink,
                Length = movieCreateInputModel.Length,
                Director = director,
            };

            // Check if movie already exists
            if (await this.GetViewModelByIdAsync<MovieViewModel>(movie.Id) != null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.MovieAlreadyExists, movie.Id));
            }

            await this.moviesRepository.AddAsync(movie);
            await this.moviesRepository.SaveChangesAsync();

            var viewModel = this.moviesRepository
                .All()
                .Where(x => x.Id == movie.Id)
                .To<MovieViewModel>()
                .FirstOrDefault();

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task EditAsync(MovieEditViewModel movieEditViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieViewModel>> GetAllMoviesAsync()
        {
            var movies = await this.moviesRepository
                .All()
                .To<MovieViewModel>()
                .ToListAsync();

            return movies;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var movie = await this.moviesRepository
                .All()
                .Where(m => m.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.MovieNotFound, id));
            }

            return movie;
        }
    }
}
