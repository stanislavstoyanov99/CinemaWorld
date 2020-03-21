namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Countries;
    using CinemaWorld.Models.ViewModels.Countries;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class CountriesController : AdministrationController
    {
        private readonly ICountriesService countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            this.countriesService = countriesService;
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
        public async Task<IActionResult> Create(CountryCreateInputModel countryCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(countryCreateInputModel);
            }

            await this.countriesService.CreateAsync(countryCreateInputModel);
            return this.RedirectToAction("GetAll", "Countries", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var countryToEdit = await this.countriesService
                .GetViewModelByIdAsync<CountryEditViewModel>(id);

            return this.View(countryToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CountryEditViewModel countryEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(countryEditViewModel);
            }

            await this.countriesService.EditAsync(countryEditViewModel);
            return this.RedirectToAction("GetAll", "Countries", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var countryToDelete = await this.countriesService.GetViewModelByIdAsync<CountryDetailsViewModel>(id);
            return this.View(countryToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(CountryDetailsViewModel countryDetailsViewModel)
        {
            await this.countriesService.DeleteByIdAsync(countryDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Countries", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var countries = await this.countriesService.GetAllCountriesAsync<CountryDetailsViewModel>();
            return this.View(countries);
        }
    }
}
