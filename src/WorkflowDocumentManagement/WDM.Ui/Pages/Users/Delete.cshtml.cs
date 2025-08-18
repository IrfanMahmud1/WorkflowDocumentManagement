using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Entities;
using WDM.Domain.Services;

namespace WDM.Ui.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            User = await _userService.GetUserByIdAsync(id);
            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result)
            {
                TempData["Message"] = "User deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete user.";
            }
            return RedirectToPage("./Index");
        }
    }
}
