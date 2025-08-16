using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Dtos;
using WDM.Domain.Services;

namespace WDM.Ui.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // Check if already authenticated
            if (HttpContext.Session.GetString("AdminId") != null)
            {
                Response.Redirect("/Dashboard");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _authService.AuthenticateAsync(LoginRequest);

            if (result.IsSuccess && result.User != null)
            {
                // Store admin info in session
                HttpContext.Session.SetString("AdminId", result.User.Id.ToString());
                HttpContext.Session.SetString("AdminEmail", result.User.Email);
                HttpContext.Session.SetString("AdminName", result.User.UserName);
                HttpContext.Session.SetString("AccessLevel", result.User.AccessLevel);
                HttpContext.Session.SetString("JwtToken", result.Token ?? "");

                return RedirectToPage("/Dashboard");
            }

            ErrorMessage = result.Message;
            return Page();
        }
    }
}
