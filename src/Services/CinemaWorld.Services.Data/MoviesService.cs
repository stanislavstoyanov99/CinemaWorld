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
        private const string AllPaginationFilter = "All";
        private const string DigitPaginationFilter = "0 - 9";
        private const int MostPopularMoviesRating = 40;

        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Director> directorsRepository;
        private readonly IDeletableEntityRepository<MovieGenre> movieGenresRepository;
        private readonly IDeletableEntityRepository<MovieCountry> movieCountriesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public MoviesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Director> directorsRepository,
            IDeletableEntityRepository<MovieGenre> movieGenresRepository,
            IDeletableEntityRepository<MovieCountry> movieCountriesRepository,
            ICloudinaryService cloudinaryService)
        {
            this.moviesRepository = moviesRepository;
            this.directorsRepository = directorsRepository;
            this.movieGenresRepository = movieGenresRepository;
            this.movieCountriesRepository = movieCountriesRepository;
            this.cloudinaryService = cloudinaryService;
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

            var coverUrl = await this.cloudinaryService
                .UploadAsync(movieCreateInputModel.CoverImage, movieCreateInputModel.Name);

            var wallpaperUrl = await this.cloudinaryService
                .UploadAsync(movieCreateInputModel.Wallpaper, movieCreateInputModel.Name + Suffixes.WallpaperSuffix);

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
                CoverPath = coverUrl,
                WallpaperPath = wallpaperUrl,
                IMDBLink = movieCreateInputModel.IMDBLink,
                Length = movieCreateInputModel.Length,
                Director = director,
            };

            bool doesMovieExist = await this.moviesRepository.All().AnyAsync(x => x.Name == movieCreateInputModel.Name);
            if (doesMovieExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.MovieAlreadyExists, movieCreateInputModel.Name));
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

            foreach (var countryId in movieCreateInputModel.SelectedCountries)
            {
                var movieCountry = new MovieCountry
                {
                    MovieId = movie.Id,
                    CountryId = countryId,
                };

                await this.movieCountriesRepository.AddAsync(movieCountry);
                movie.MovieCountries.Add(movieCountry);
            }

            await this.moviesRepository.AddAsync(movie);
            await this.moviesRepository.SaveChangesAsync();
            await this.movieGenresRepository.SaveChangesAsync();
            await this.movieCountriesRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<MovieDetailsViewModel>(movie.Id);

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

            if (movieEditViewModel.CoverImage != null)
            {
                var newCoverImageUrl = await this.cloudinaryService
                    .UploadAsync(movieEditViewModel.CoverImage, movieEditViewModel.Name);
                movie.CoverPath = newCoverImageUrl;
            }

            if (movieEditViewModel.Wallpaper != null)
            {
                var newWallpaperUrl = await this.cloudinaryService
                    .UploadAsync(movieEditViewModel.Wallpaper, movieEditViewModel.Name + Suffixes.WallpaperSuffix);
                movie.WallpaperPath = newWallpaperUrl;
            }

            movie.Name = movieEditViewModel.Name;
            movie.DateOfRelease = movieEditViewModel.DateOfRelease;
            movie.Resolution = movieEditViewModel.Resolution;
            movie.Rating = movieEditViewModel.Rating;
            movie.Description = movieEditViewModel.Description;
            movie.Language = movieEditViewModel.Language;
            movie.CinemaCategory = cinemaCategory;
            movie.TrailerPath = movieEditViewModel.TrailerPath;
            movie.IMDBLink = movieEditViewModel.IMDBLink;
            movie.Length = movieEditViewModel.Length;
            movie.DirectorId = movieEditViewModel.DirectorId;
            movie.ModifiedOn = DateTime.UtcNow;

            await this.moviesRepository.SaveChangesAsync();

            var movieGenres = await this.movieGenresRepository
                .All()
                .Where(x => x.MovieId == movie.Id)
                .ToListAsync();

            var movieCountries = await this.movieCountriesRepository
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

            if (movieCountries.Count != 0)
            {
                foreach (var movieCountry in movieCountries)
                {
                    this.movieCountriesRepository.HardDelete(movieCountry);
                    await this.movieCountriesRepository.SaveChangesAsync();
                }

                await this.UpdateMovieCountries(movieEditViewModel, movie);
            }
            else
            {
                await this.UpdateMovieCountries(movieEditViewModel, movie);
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

            var movieCountries = await this.movieCountriesRepository
                .All()
                .Where(m => m.MovieId == id)
                .ToListAsync();

            foreach (var movieGenre in movieGenres)
            {
                movieGenre.IsDeleted = true;
                movieGenre.DeletedOn = DateTime.UtcNow;
                this.movieGenresRepository.Update(movieGenre);
            }

            foreach (var movieCountry in movieCountries)
            {
                movieCountry.IsDeleted = true;
                movieCountry.DeletedOn = DateTime.UtcNow;
                this.movieCountriesRepository.Update(movieCountry);
            }

            await this.movieGenresRepository.SaveChangesAsync();
            await this.movieCountriesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllMoviesAsync<TViewModel>()
        {
            var movies = await this.moviesRepository
                .All()
                .OrderByDescending(m => m.Ratings.Sum(x => x.Rate))
                .ThenByDescending(m => m.DateOfRelease.Year)
                .To<TViewModel>()
                .ToListAsync();

            return movies;
        }

        public IQueryable<TViewModel> GetAllMoviesAsQueryeable<TViewModel>()
        {
            var movies = this.moviesRepository
                .All()
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.DateOfRelease)
                .To<TViewModel>();

            return movies;
        }

        public async Task<IEnumerable<TViewModel>> GetRecentlyAddedMoviesAsync<TViewModel>(int count = 0)
        {
            var recentlyAddedMovies = await this.moviesRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.DateOfRelease)
                .Take(count)
                .To<TViewModel>()
                .ToListAsync();

            return recentlyAddedMovies;
        }

        public async Task<IEnumerable<TViewModel>> GetMostPopularMoviesAsync<TViewModel>(int count = 0)
        {
            var mostPopularMovies = await this.moviesRepository
                .All()
                .Where(x => x.Ratings.Any(x => x.Rate >= MostPopularMoviesRating))
                .Take(count)
                .To<TViewModel>()
                .ToListAsync();

            return mostPopularMovies;
        }

        public async Task<IEnumerable<TViewModel>> GetAllMovieGenresAsync<TViewModel>()
        {
            var movieGenres = await this.movieGenresRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return movieGenres;
        }

        public async Task<IEnumerable<TViewModel>> GetAllMovieCountriesAsync<TViewModel>()
        {
            var movieCountries = await this.movieCountriesRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return movieCountries;
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

        public async Task<IEnumerable<TViewModel>> GetTopImdbMoviesAsync<TViewModel>(decimal rating = 0, int count = 0)
        {
            // Cast to double because of problem with translating sql query with decimal
            var topImdbMovies = await this.moviesRepository
                .All()
                .Where(m => (double)m.Rating > (double)rating)
                .To<TViewModel>()
                .Take(count)
                .ToListAsync();

            return topImdbMovies;
        }

        public async Task<IEnumerable<TViewModel>> GetTopRatingMoviesAsync<TViewModel>(decimal rating = 0, int count = 0)
        {
            // Cast to double because of problem with translating sql query with decimal
            var topRatingMovies = await this.moviesRepository
                .All()
                .Where(m => m.Ratings.Sum(x => x.Rate) > (double)rating)
                .OrderByDescending(m => m.Ratings.Sum(x => x.Rate))
                .ThenByDescending(m => m.DateOfRelease.Year)
                .To<TViewModel>()
                .Take(count)
                .ToListAsync();

            return topRatingMovies;
        }

        public IQueryable<MovieDetailsViewModel> GetByGenreNameAsQueryable(string name)
        {
            var movies = this.moviesRepository
                .All()
                .Where(m => m.MovieGenres.Any(mg => mg.Genre.Name == name))
                .To<MovieDetailsViewModel>();

            return movies;
        }

        public IQueryable<TViewModel> GetAllMoviesByFilterAsQueryeable<TViewModel>(string letter = null)
        {
            var moviesByFilter = Enumerable.Empty<TViewModel>().AsQueryable();

            if (!string.IsNullOrEmpty(letter) && letter != AllPaginationFilter && letter != DigitPaginationFilter)
            {
                moviesByFilter = this.moviesRepository
                    .All()
                    .Where(x => x.Name.ToLower().StartsWith(letter))
                    .To<TViewModel>();
            }
            else if (letter == DigitPaginationFilter)
            {
                var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
                moviesByFilter = this.moviesRepository
                    .All()
                    .Where(x => numbers.Contains(x.Name.Substring(0, 1)))
                    .To<TViewModel>();
            }
            else
            {
                moviesByFilter = this.GetAllMoviesAsQueryeable<TViewModel>();
            }

            return moviesByFilter;
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

        private async Task UpdateMovieCountries(MovieEditViewModel model, Movie movie)
        {
            for (int i = 0; i < model.SelectedCountries.Count; i++)
            {
                var movieCountry = new MovieCountry
                {
                    MovieId = movie.Id,
                    CountryId = model.SelectedCountries[i],
                };

                await this.movieCountriesRepository.AddAsync(movieCountry);
                movie.MovieCountries.Add(movieCountry);
            }

            await this.movieCountriesRepository.SaveChangesAsync();
            await this.moviesRepository.SaveChangesAsync();
        }
    }
}
