using CompanyService.Core.Entities;

namespace CompanyService.Application.Interfaces.Persistence;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetByIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTaxNumberAsync(
        string taxNumber,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByOwnerAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken = default);

    Task<Company?> GetByOwnerUserIdAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Company>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Devolve todas as empresas às quais o utilizador pertence
    /// através de uma membership ativa.
    /// </summary>
    Task<IReadOnlyCollection<Company>> GetByMemberUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Devolve uma empresa específica apenas se o utilizador
    /// possuir uma membership ativa nessa empresa.
    /// </summary>
    Task<Company?> GetByIdForMemberAsync(
        Guid companyId,
        Guid userId,
        CancellationToken cancellationToken = default);
}