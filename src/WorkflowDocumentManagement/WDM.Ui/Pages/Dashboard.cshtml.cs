using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Ui.Attributes;

namespace WDM.Ui.Pages
{
    [AdminAuthorize]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
