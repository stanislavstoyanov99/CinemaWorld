namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : Controller
    {
        public IActionResult Contact()
        {
            return this.View();
        }
    }
}
