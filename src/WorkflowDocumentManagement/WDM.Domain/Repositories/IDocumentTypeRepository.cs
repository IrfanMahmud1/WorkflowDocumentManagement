using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Entities;

namespace WDM.Domain.Repositories
{
    public interface IDocumentTypeRepository : IRepository<DocumentType>
    {
        Task<DocumentType?> GetByNameAsync(string name);
    }
}
