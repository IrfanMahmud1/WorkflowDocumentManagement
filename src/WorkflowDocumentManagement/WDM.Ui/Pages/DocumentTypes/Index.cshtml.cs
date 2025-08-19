using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Services;
using WDM.Domain.Entities;

namespace WDM.Ui.Pages.DocumentTypes
{
    public class IndexModel : PageModel
    {
        private readonly IDocumentTypeService _documentTypeService;

        public IndexModel(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        public IEnumerable<WDM.Domain.Entities.DocumentType> DocumentTypes { get; set; } = new List<WDM.Domain.Entities.DocumentType>();

        public async Task OnGetAsync()
        {
            DocumentTypes = await _documentTypeService.GetAllDocumentTypeAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var result = await _documentTypeService.DeleteDocumentTypeAsync(id);
            if (result)
            {
                TempData["Message"] = "Document type deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete Document type.";
            }
            return RedirectToPage();
        }
    }
}
