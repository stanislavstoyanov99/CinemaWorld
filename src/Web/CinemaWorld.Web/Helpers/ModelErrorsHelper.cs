namespace CinemaWorld.Web.Helpers
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public static class ModelErrorsHelper
    {
        public static string GetModelErrors(ModelStateDictionary modelState)
        {
            return string.Join(Environment.NewLine, modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
        }
    }
}
