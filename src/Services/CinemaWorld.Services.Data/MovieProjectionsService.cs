namespace CinemaWorld.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class MovieProjectionsService : IMovieProjectionsService
    {
        private readonly IDeletableEntityRepository<MovieProjection> movieProjectionsRepository;
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Hall> hallsRepository;
        private readonly IDeletableEntityRepository<Cinema> cinemasRepository;

        public MovieProjectionsService(
            IDeletableEntityRepository<MovieProjection> movieProjectionsRepository,
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Hall> hallsRepository,
            IDeletableEntityRepository<Cinema> cinemasRepository)
        {
            this.movieProjectionsRepository = movieProjectionsRepository;
            this.moviesRepository = moviesRepository;
            this.hallsRepository = hallsRepository;
            this.cinemasRepository = cinemasRepository;
        }

        public async Task<MovieProjectionDetailsViewModel> CreateAsync(MovieProjectionCreateInputModel model)
        {
            var movie = await this.moviesRepository
                .All()
                .FirstOrDefaultAsync(m => m.Id == model.MovieId);

            if (movie == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.MovieNotFound, model.MovieId));
            }

            var hall = await this.hallsRepository
                .All()
                .FirstOrDefaultAsync(h => h.Id == model.HallId);

            if (hall == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.HallNotFound, model.HallId));
            }

            var cinema = await this.cinemasRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == model.CinemaId);

            if (cinema == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.CinemaNotFound, model.CinemaId));
            }

            var movieProjection = new MovieProjection
            {
                Date = model.Date,
                Movie = movie,
                MovieId = model.MovieId,
                Hall = hall,
                HallId = model.HallId,
                Cinema = cinema,
                CinemaId = model.CinemaId,
            };

            bool doesMovieProjectionExist = await this.movieProjectionsRepository
                .All()
                .AnyAsync(x => x.MovieId == movieProjection.MovieId && x.HallId == movieProjection.HallId);
            if (doesMovieProjectionExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.MovieProjectionAlreadyExists, movieProjection.MovieId, movieProjection.HallId));
            }

            await this.movieProjectionsRepository.AddAsync(movieProjection);
            await this.movieProjectionsRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(movieProjection.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var movieProjection = await this.movieProjectionsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            if (movieProjection == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.MovieProjectionNotFound, id));
            }

            movieProjection.IsDeleted = true;
            movieProjection.DeletedOn = DateTime.UtcNow;
            this.movieProjectionsRepository.Update(movieProjection);
            await this.movieProjectionsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(MovieProjectionEditViewModel movieProjectionEditViewModel)
        {
            var movieProjection = await this.movieProjectionsRepository
                .All()
                .FirstOrDefaultAsync(g => g.Id == movieProjectionEditViewModel.Id);

            if (movieProjection == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.MovieProjectionNotFound, movieProjectionEditViewModel.Id));
            }

            movieProjection.Date = movieProjectionEditViewModel.Date;
            movieProjection.MovieId = movieProjectionEditViewModel.MovieId;
            movieProjection.HallId = movieProjectionEditViewModel.HallId;
            movieProjection.CinemaId = movieProjectionEditViewModel.CinemaId;
            movieProjection.ModifiedOn = DateTime.UtcNow;

            this.movieProjectionsRepository.Update(movieProjection);
            await this.movieProjectionsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllMovieProjectionsAsync<TViewModel>()
        {
            var movieProjections = await this.movieProjectionsRepository
                .All()
                .OrderBy(x => x.Movie.Name)
                .ThenBy(x => x.Movie.DateOfRelease)
                .To<TViewModel>()
                .ToListAsync();

            return movieProjections;
        }

        public IQueryable<TViewModel> GetAllMovieProjectionsByCinemaAsQueryeable<TViewModel>(string cinemaName = null)
        {
            var movieProjections = this.movieProjectionsRepository
                .All()
                .Where(x => x.Cinema.Name == cinemaName)
                .OrderBy(x => x.Movie.Name)
                .ThenBy(x => x.HallId)
                .To<TViewModel>();

            return movieProjections;
        }

        public IQueryable<TViewModel> GetAllMovieProjectionsAsQueryeable<TViewModel>()
        {
            var movieProjections = this.movieProjectionsRepository
                .All()
                .OrderBy(x => x.Movie.Name)
                .ThenBy(x => x.HallId)
                .To<TViewModel>();

            return movieProjections;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var movieProjectionViewModel = await this.movieProjectionsRepository
                .All()
                .Where(mp => mp.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (movieProjectionViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.MovieProjectionNotFound, id));
            }

            return movieProjectionViewModel;
        }
    }
}
