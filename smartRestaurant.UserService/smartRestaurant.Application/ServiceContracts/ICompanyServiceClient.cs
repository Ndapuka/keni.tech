using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartRestaurant.Application.ServiceContracts
{
    /// <summary>
    /// Cliente interno para o CompanyService. Usado no fluxo de
    /// switch-company para validar pertença ativa antes de reemitir o JWT.
    /// Chamada direta (sem gateway) — tráfego east-west entre serviços.
    /// </summary>
    public interface ICompanyServiceClient
    {
        Task<bool> IsActiveMemberAsync(
           Guid companyId,
           Guid userId,
           CancellationToken cancellationToken = default);
    }
}
