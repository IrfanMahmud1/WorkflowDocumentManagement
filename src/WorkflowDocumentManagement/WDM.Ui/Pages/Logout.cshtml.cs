using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WDM.Ui.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var adminEmail = HttpContext.Session.GetString("AdminEmail");

            // Log logout event
            if (!string.IsNullOrEmpty(adminEmail))
            {
                _logger.LogInformation("User logged out: {Email}", adminEmail);
            }

            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to login page with a success message
            return RedirectToPage("/Login");
        }
    }
}
