using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Dtos;
using WDM.Domain.Services;

namespace WDM.Ui.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public CreateUserDto User { get; set; } = new CreateUserDto();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _userService.CreateUserAsync(User);
            if (result)
            {
                TempData["Message"] = "User created successfully!";
                return RedirectToPage("./Index");
            }

            TempData["Error"] = "Failed to create user.";
            return Page();
        }
    }
}
