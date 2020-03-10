namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class GenresController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
