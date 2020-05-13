namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.MovieComments;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class MovieCommentsController : Controller
    {
        private readonly IMovieCommentsService movieCommentsService;
        private readonly UserManager<CinemaWorldUser> userManager;

        public MovieCommentsController(
            IMovieCommentsService commentsService,
            UserManager<CinemaWorldUser> userManager)
        {
            this.movieCommentsService = commentsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateMovieCommentInputModel input)
        {
            var parentId = input.ParentId == 0 ? (int?)null : input.ParentId;

            if (parentId.HasValue)
            {
                if (!await this.movieCommentsService.IsInMovieId(parentId.Value, input.MovieId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);

            try
            {
                await this.movieCommentsService.CreateAsync(input.MovieId, userId, input.Content, parentId);
            }
            catch (ArgumentException aex)
            {
                return this.BadRequest(aex.Message);
            }

            return this.RedirectToAction("Details", "Movies", new { id = input.MovieId });
        }
    }
}
