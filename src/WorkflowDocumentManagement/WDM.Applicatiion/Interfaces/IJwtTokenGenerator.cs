using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Entities;

namespace WDM.Applicatiion.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User admin);
    }
}
