namespace CinemaWorld.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task Create(int movieId, string userId, string content, int? parentId = null)
        {
            var comment = new Comment
            {
                MovieId = movieId,
                UserId = userId,
                Content = content,
                ParentId = parentId,
            };

            bool doesCommentExist = await this.commentsRepository
                .All()
                .AnyAsync(x => x.Id == comment.Id && x.UserId == userId);
            if (doesCommentExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.CommentAlreadyExists, comment.Id));
            }

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task<bool> IsInMovieId(int commentId, int movieId)
        {
            var commentMovieId = await this.commentsRepository
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.MovieId)
                .FirstOrDefaultAsync();

            return commentMovieId == movieId;
        }
    }
}
