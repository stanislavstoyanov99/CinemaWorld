namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.News;
    using CinemaWorld.Models.ViewModels.News;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class NewsController : AdministrationController
    {
        private readonly INewsService newsService;
        private readonly UserManager<CinemaWorldUser> userManager;

        public NewsController(INewsService newsService, UserManager<CinemaWorldUser> userManager)
        {
            this.newsService = newsService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsCreateInputModel newsCreateInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                return this.View(newsCreateInputModel);
            }

            await this.newsService.CreateAsync(newsCreateInputModel, user.Id);
            return this.RedirectToAction("GetAll", "News", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var newsToEdit = await this.newsService
                .GetViewModelByIdAsync<NewsEditViewModel>(id);

            return this.View(newsToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsEditViewModel newsEditViewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                return this.View(newsEditViewModel);
            }

            await this.newsService.EditAsync(newsEditViewModel, user.Id);
            return this.RedirectToAction("GetAll", "News", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var newsToDelete = await this.newsService.GetViewModelByIdAsync<NewsDeleteViewModel>(id);
            return this.View(newsToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(AllNewsListingViewModel newsDetailsViewModel)
        {
            await this.newsService.DeleteByIdAsync(newsDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "News", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var news = await this.newsService.GetAllNewsAsync<AllNewsListingViewModel>();
            return this.View(news);
        }
    }
}
