namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class CinemasController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> AddCinemaAsync()
        {
            return this.View();
        }

        public async Task<IActionResult> EditCinemaAsync()
        {
            return this.View();
        }

        public async Task<IActionResult> RemoveCinemaAsync()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAllCinemasAsync()
        {
            return this.View();
        }
    }
}
