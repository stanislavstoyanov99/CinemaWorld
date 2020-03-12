namespace CinemaWorld.Web.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using CinemaWorld.Services.Data.Contracts;

    public static class SelectListGenerator
    {
        public static IEnumerable<SelectListItem> GetAllDirectors(IDirectorsService directorsService)
        {
            var directors = directorsService.GetAllDirectorsAsync();

            return directors
                .GetAwaiter()
                .GetResult()
                .Select(d => new SelectListItem()
                {
                    Value = d.FirstName.ToString(),
                    Text = d.FirstName,
                });
        }
    }
}
