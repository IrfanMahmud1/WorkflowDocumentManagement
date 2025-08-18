using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Services;
using WDM.Domain.Entities;

namespace WDM.Ui.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<User> Users { get; set; } = new List<User>();

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllUsersAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
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
            return RedirectToPage();
        }
    }
}
