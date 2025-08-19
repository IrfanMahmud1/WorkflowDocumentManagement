using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Services;

namespace WDM.Ui.Pages.DocumentTypes
{
    public class DeleteModel : PageModel
    {
        private readonly IDocumentTypeService _documentTypeService;

        public DeleteModel(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        [BindProperty]
        public WDM.Domain.Entities.DocumentType DocumentType { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            DocumentType = await _documentTypeService.GetDocumentTypeByIdAsync(id);
            if (DocumentType == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var result = await _documentTypeService.DeleteDocumentTypeAsync(id);
            if (result)
            {
                TempData["Message"] = "Document type deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete document type.";
            }
            return RedirectToPage("./Index");
        }
    }
}
