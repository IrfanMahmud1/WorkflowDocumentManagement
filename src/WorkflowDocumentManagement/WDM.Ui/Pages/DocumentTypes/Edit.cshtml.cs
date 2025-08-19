using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Dtos;
using WDM.Domain.Services;

namespace WDM.Ui.Pages.DocumentTypes
{
    public class EditModel : PageModel
    {
        private readonly IDocumentTypeService _documentTypeService;

        public EditModel(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        [BindProperty]
        public UpdateDocumentTypeDto DocumentTypeDto { get; set; } = new UpdateDocumentTypeDto();

        [BindProperty]
        public Guid DocumentTypeId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var type = await _documentTypeService.GetDocumentTypeByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }

            DocumentTypeId = id;
            DocumentTypeDto = new UpdateDocumentTypeDto
            {
                Name = type.Name,
                Description = type.Description,
                IsActive = type.IsActive
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _documentTypeService.UpdateDocumentTypeAsync(DocumentTypeId, DocumentTypeDto);
            if (result)
            {
                TempData["Message"] = "Document type updated successfully!";
                return RedirectToPage("./Index");
            }

            TempData["Error"] = "Failed to update document type.";
            return Page();
        }
    }
}
