namespace CinemaWorld.Web.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("li", Attributes = "active-when")]
    public class LiTagHelper : TagHelper
    {
        public string ActiveWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.ActiveWhen == null)
            {
                return;
            }

            var targetController = this.ActiveWhen.Split("/")[1];
            var targetAction = this.ActiveWhen.Split("/")[2];

            var currentController = this.ViewContextData.RouteData.Values["controller"]?.ToString();
            var currentAction = this.ViewContextData.RouteData.Values["action"]?.ToString();

            if (currentController != null || currentAction != null)
            {
                if (currentController.Equals(targetController) && currentAction.Equals(targetAction))
                {
                    if (output.Attributes.ContainsName("class"))
                    {
                        output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                    }
                    else
                    {
                        output.Attributes.SetAttribute("class", "active");
                    }
                }
            }
        }
    }
}
