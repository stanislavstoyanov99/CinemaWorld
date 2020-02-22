namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class GenresController : Controller
    {
        public IActionResult Action()
        {
            return this.View();
        }
    }
}
