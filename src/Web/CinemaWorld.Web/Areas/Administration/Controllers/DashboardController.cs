namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        // TODO - can be used in future stage
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
