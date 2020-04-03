namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels.About;
    using CinemaWorld.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class AboutController : Controller
    {
        private readonly IAboutService aboutService;

        public AboutController(IAboutService aboutService)
        {
            this.aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            var faqs = await this.aboutService.GetAllFaqsAsync<FaqDetailsViewModel>();

            return this.View(faqs);
        }
    }
}
