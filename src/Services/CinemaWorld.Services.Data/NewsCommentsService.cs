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

    public class NewsCommentsService : INewsCommentsService
    {
        private readonly IDeletableEntityRepository<NewsComment> newsCommentsRepository;

        public NewsCommentsService(IDeletableEntityRepository<NewsComment> newsCommentsRepository)
        {
            this.newsCommentsRepository = newsCommentsRepository;
        }

        public async Task CreateAsync(int newsId, string userId, string content, int? parentId = null)
        {
            var newsComment = new NewsComment
            {
                NewsId = newsId,
                UserId = userId,
                Content = content,
                ParentId = parentId,
            };

            bool doesNewsCommentExist = await this.newsCommentsRepository
                .All()
                .AnyAsync(x => x.NewsId == newsComment.NewsId && x.UserId == userId && x.Content == content);
            if (doesNewsCommentExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.NewsCommentAlreadyExists, newsComment.NewsId, newsComment.Content));
            }

            await this.newsCommentsRepository.AddAsync(newsComment);
            await this.newsCommentsRepository.SaveChangesAsync();
        }

        public async Task<bool> IsInNewsId(int commentId, int newsId)
        {
            var commentNewsId = await this.newsCommentsRepository
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.NewsId)
                .FirstOrDefaultAsync();

            return commentNewsId == newsId;
        }
    }
}
