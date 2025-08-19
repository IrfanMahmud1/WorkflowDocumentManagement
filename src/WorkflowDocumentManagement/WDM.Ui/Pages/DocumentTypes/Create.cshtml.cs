using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WDM.Domain.Dtos;
using WDM.Domain.Services;

namespace WDM.Ui.Pages.DocumentType
{
    public class CreateModel : PageModel
    {
        private readonly IDocumentTypeService _documentTypeService;

        public CreateModel(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        [BindProperty]
        public CreateDocumentTypeDto DocumentType { get; set; } = new CreateDocumentTypeDto();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _documentTypeService.CreateDocumentTypeAsync(DocumentType);
            if (result)
            {
                TempData["Message"] = "Document type created successfully!";
                return RedirectToPage("./Index");
            }

            TempData["Error"] = "Failed to create document type.";
            return Page();
        }
    }
}
