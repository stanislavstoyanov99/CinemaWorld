namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Seats;
    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Models.ViewModels.Seats;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class SeatsController : AdministrationController
    {
        private const int PageSize = 10;

        private readonly ISeatsService seatsService;
        private readonly IHallsService hallsService;

        public SeatsController(ISeatsService seatsService, IHallsService hallsService)
        {
            this.seatsService = seatsService;
            this.hallsService = hallsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var halls = await this.hallsService
                .GetAllHallsAsync<HallDetailsViewModel>();

            var viewModel = new SeatCreateInputModel
            {
                Halls = halls,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeatCreateInputModel seatCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var halls = await this.hallsService
                   .GetAllHallsAsync<HallDetailsViewModel>();

                seatCreateInputModel.Halls = halls;
                return this.View(seatCreateInputModel);
            }

            await this.seatsService.CreateAsync(seatCreateInputModel);
            return this.RedirectToAction("GetAll", "Seats", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var halls = await this.hallsService
                .GetAllHallsAsync<HallDetailsViewModel>();

            var seatToEdit = await this.seatsService
                .GetViewModelByIdAsync<SeatEditViewModel>(id);

            seatToEdit.Halls = halls;

            return this.View(seatToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeatEditViewModel seatEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                var halls = await this.hallsService
                    .GetAllHallsAsync<HallDetailsViewModel>();
                seatEditViewModel.Halls = halls;

                return this.View(seatEditViewModel);
            }

            await this.seatsService.EditAsync(seatEditViewModel);
            return this.RedirectToAction("GetAll", "Seats", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var seatToDelete = await this.seatsService.GetViewModelByIdAsync<SeatDetailsViewModel>(id);
            return this.View(seatToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(SeatDetailsViewModel seatDetailsViewModel)
        {
            await this.seatsService.DeleteByIdAsync(seatDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Seats", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll(int? pageNumber)
        {
            var seats = await Task.Run(() => this.seatsService.GetAllSeatsAsQueryeable<SeatDetailsViewModel>());
            return this.View(await PaginatedList<SeatDetailsViewModel>.CreateAsync(seats, pageNumber ?? 1, PageSize));
        }
    }
}
