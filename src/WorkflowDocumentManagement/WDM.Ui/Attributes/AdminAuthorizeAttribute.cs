using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WDM.Ui.Attributes
{
    public class AdminAuthorizeAttribute : Attribute, IPageFilter
    {
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var adminId = context.HttpContext.Session.GetString("AdminId");

            if (string.IsNullOrEmpty(adminId))
            {
                context.Result = new RedirectToPageResult("/Login");
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            // not needed
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            // not needed
        }
    }

}
