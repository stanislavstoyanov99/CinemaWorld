namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.Comments;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;
        private readonly UserManager<CinemaWorldUser> userManager;

        public CommentsController(
            ICommentsService commentsService,
            UserManager<CinemaWorldUser> userManager)
        {
            this.commentsService = commentsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            var parentId = input.ParentId == 0 ? (int?)null : input.ParentId;

            if (parentId.HasValue)
            {
                if (!await this.commentsService.IsInMovieId(parentId.Value, input.MovieId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);

            try
            {
                await this.commentsService.Create(input.MovieId, userId, input.Content, parentId);
            }
            catch (ArgumentException aex)
            {
                return this.BadRequest(aex.Message);
            }

            return this.RedirectToAction("Details", "Movies", new { id = input.MovieId });
        }
    }
}
