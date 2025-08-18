using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Dtos;
using WDM.Domain.Services;

namespace WDM.Ui.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;

        public EditModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UpdateUserDto User { get; set; } = new UpdateUserDto();

        [BindProperty]
        public Guid UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserId = id;
            User = new UpdateUserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                AccessLevel = user.AccessLevel,
                IsActive = user.IsActive
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _userService.UpdateUserAsync(UserId, User);
            if (result)
            {
                TempData["Message"] = "User updated successfully!";
                return RedirectToPage("./Index");
            }

            TempData["Error"] = "Failed to update user.";
            return Page();
        }
    }
}
