using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;
using WDM.Domain.Repositories;

namespace WDM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeController(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentType>>> GetAllDocumentTypes()
        {
            try
            {
                var documentTypes = await _documentTypeRepository.GetAllAsync();
                return Ok(documentTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentType>> GetDocumentType(Guid id) // Fixed return type
        {
            try
            {
                var documentType = await _documentTypeRepository.GetByIdAsync(id);
                if (documentType == null)
                {
                    return NotFound($"DocumentType with ID {id} not found.");
                }
                return Ok(documentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DocumentType>> CreateDocumentType([FromBody] CreateDocumentTypeDto documentTypeDto) // Fixed return type
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var documentType = new DocumentType
                {
                    Id = Guid.NewGuid(),
                    Name = documentTypeDto.Name,
                    Description = documentTypeDto.Description,
                    CreatedBy = documentTypeDto.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _documentTypeRepository.AddAsync(documentType);
                if (result)
                {
                    return CreatedAtAction(nameof(GetDocumentType), new { id = documentType.Id }, documentType);
                }
                return BadRequest("Failed to create document type.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDocumentType(Guid id, [FromBody] UpdateDocumentTypeDto documentTypeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDocumentType = await _documentTypeRepository.GetByIdAsync(id);
                if (existingDocumentType == null)
                {
                    return NotFound($"DocumentType with ID {id} not found."); // Fixed error message
                }

                existingDocumentType.Name = documentTypeDto.Name;
                existingDocumentType.Description = documentTypeDto.Description;
                existingDocumentType.IsActive = documentTypeDto.IsActive;

                var result = await _documentTypeRepository.UpdateAsync(existingDocumentType);
                if (result)
                {
                    return NoContent();
                }
                return BadRequest("Failed to update document type.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDocumentType(Guid id)
        {
            try
            {
                var existingDocumentType = await _documentTypeRepository.GetByIdAsync(id);
                if (existingDocumentType == null)
                {
                    return NotFound($"DocumentType with ID {id} not found."); // Fixed error message
                }

                var result = await _documentTypeRepository.DeleteAsync(existingDocumentType.Id);
                if (result)
                {
                    return NoContent();
                }
                return BadRequest("Failed to delete document type.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}