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
        private readonly IDeletableEntityRepository<MovieGenre> movieGenresRepository;

        public MoviesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Director> directorsRepository,
            IDeletableEntityRepository<MovieGenre> movieGenresRepository)
        {
            this.moviesRepository = moviesRepository;
            this.directorsRepository = directorsRepository;
            this.movieGenresRepository = movieGenresRepository;
        }

        public async Task<MovieDetailsViewModel> CreateAsync(MovieCreateInputModel movieCreateInputModel)
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

            bool doesMovieExist = await this.moviesRepository.All().AnyAsync(x => x.Id == movie.Id);
            if (doesMovieExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.MovieAlreadyExists, movie.Id));
            }

            foreach (var genreId in movieCreateInputModel.SelectedGenres)
            {
                var movieGenre = new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = genreId,
                };

                await this.movieGenresRepository.AddAsync(movieGenre);
                movie.MovieGenres.Add(movieGenre);
            }

            await this.moviesRepository.AddAsync(movie);
            await this.moviesRepository.SaveChangesAsync();
            await this.movieGenresRepository.SaveChangesAsync();

            var viewModel = this.GetViewModelByIdAsync<MovieDetailsViewModel>(movie.Id)
                .GetAwaiter()
                .GetResult();

            return viewModel;
        }

        public async Task EditAsync(MovieEditViewModel movieEditViewModel)
        {
            if (!Enum.TryParse(movieEditViewModel.CinemaCategory, true, out CinemaCategory cinemaCategory))
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.InvalidCinemaCategoryType, movieEditViewModel.CinemaCategory));
            }

            var movie = await this.moviesRepository
                .All()
                .FirstOrDefaultAsync(m => m.Id == movieEditViewModel.Id);

            if (movie == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.MovieNotFound, movieEditViewModel.Id));
            }

            movie.Name = movieEditViewModel.Name;
            movie.DateOfRelease = movieEditViewModel.DateOfRelease;
            movie.Resolution = movieEditViewModel.Resolution;
            movie.Rating = movieEditViewModel.Rating;
            movie.Description = movieEditViewModel.Description;
            movie.Language = movieEditViewModel.Language;
            movie.CinemaCategory = cinemaCategory;
            movie.TrailerPath = movieEditViewModel.TrailerPath;
            movie.CoverPath = movieEditViewModel.CoverPath;
            movie.IMDBLink = movieEditViewModel.IMDBLink;
            movie.Length = movieEditViewModel.Length;
            movie.DirectorId = movieEditViewModel.DirectorId;
            movie.MovieGenres.Clear();

            await this.moviesRepository.SaveChangesAsync();

            var movieGenres = await this.movieGenresRepository
                .All()
                .Where(x => x.MovieId == movie.Id)
                .ToListAsync();

            if (movieGenres.Count != 0)
            {
                foreach (var movieGenre in movieGenres)
                {
                    this.movieGenresRepository.HardDelete(movieGenre);
                    await this.movieGenresRepository.SaveChangesAsync();
                }

                await this.UpdateMovieGenres(movieEditViewModel, movie);
            }
            else
            {
                await this.UpdateMovieGenres(movieEditViewModel, movie);
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            var movie = await this.moviesRepository.All().FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.MovieNotFound, id));
            }

            movie.IsDeleted = true;
            movie.DeletedOn = DateTime.UtcNow;
            this.moviesRepository.Update(movie);
            await this.moviesRepository.SaveChangesAsync();

            var movieGenres = await this.movieGenresRepository
                .All()
                .Where(m => m.MovieId == id)
                .ToListAsync();

            foreach (var movieGenre in movieGenres)
            {
                movieGenre.DeletedOn = DateTime.UtcNow;
                this.movieGenresRepository.Delete(movieGenre);
            }

            await this.movieGenresRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllMoviesAsync<TViewModel>()
        {
            var movies = await this.moviesRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return movies;
        }

        public async Task<IEnumerable<TViewModel>> GetAllMovieGenresAsync<TViewModel>(int movieId)
        {
            var movieGenres = await this.movieGenresRepository
                .All()
                .Where(x => x.MovieId == movieId)
                .To<TViewModel>()
                .ToListAsync();

            return movieGenres;
        }

        public async Task<MovieGenreViewModel> GetMovieGenreIdAsync(int movieId)
        {
            var movieGenre = await this.movieGenresRepository
                .All()
                .To<MovieGenreViewModel>()
                .FirstOrDefaultAsync(x => x.MovieId == movieId);

            if (movieGenre == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.MovieGenreNotFound, movieId, movieGenre.GenreId));
            }

            return movieGenre;
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

        private async Task UpdateMovieGenres(MovieEditViewModel model, Movie movie)
        {
            for (int i = 0; i < model.SelectedGenres.Count; i++)
            {
                var movieGenre = new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = model.SelectedGenres[i],
                };

                await this.movieGenresRepository.AddAsync(movieGenre);
                movie.MovieGenres.Add(movieGenre);
            }

            await this.movieGenresRepository.SaveChangesAsync();
            await this.moviesRepository.SaveChangesAsync();
        }
    }
}
