using Microsoft.Extensions.Logging;
using smartRestaurant.Application.ServiceContracts;
using System.Net.Http.Json;

namespace smartRestaurant.Infrastructure.Clients
{
    public class CompanyServiceClient : ICompanyServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CompanyServiceClient> _logger;

        public CompanyServiceClient(HttpClient httpClient, ILogger<CompanyServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<bool> IsActiveMemberAsync(
            Guid companyId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"api/companies/{companyId}/members/{userId}/is-active",
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "CompanyService devolveu {StatusCode} ao validar pertença de {UserId} em {CompanyId}.",
                        response.StatusCode,
                        userId,
                        companyId);

                    return false;
                }

                var payload = await response.Content.ReadFromJsonAsync<IsActiveMemberResponse>(
                    cancellationToken: cancellationToken);

                return payload?.IsActiveMember ?? false;
            }
            catch (Exception ex)
            {
                // Fail-closed: uma falha de rede/timeout NUNCA deve resultar
                // em acesso concedido por defeito. É preferível o utilizador
                // ter de tentar de novo a ganhar acesso silenciosamente a
                // uma empresa a que não pertence.
                _logger.LogError(
                    ex,
                    "Falha ao contactar CompanyService para validar pertença de {UserId} em {CompanyId}.",
                    userId,
                    companyId);

                return false;
            }
        }

        private sealed record IsActiveMemberResponse(bool IsActiveMember);
    }
}

