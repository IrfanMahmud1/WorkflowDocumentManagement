using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;

namespace WDM.Domain.Services
{
    public interface IDocumentTypeService
    {
        Task<IEnumerable<DocumentType>> GetAllDocumentTypeAsync();
        Task<DocumentType> GetDocumentTypeByIdAsync(Guid id);
        Task<bool> CreateDocumentTypeAsync(CreateDocumentTypeDto userDto);
        Task<bool> UpdateDocumentTypeAsync(Guid id, UpdateDocumentTypeDto userDto);
        Task<bool> DeleteDocumentTypeAsync(Guid id);
    }
}
