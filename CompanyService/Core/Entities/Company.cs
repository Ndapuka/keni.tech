using BuildingBlocks.Shared.Contracts.Company.Common;
using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Core.Exceptions;
using CompanyService.Core.ValueObjects;
using Core.Constants;

namespace CompanyService.Core.Entities;

public sealed class Company : AuditableEntity
{
    private readonly List<CompanyUser> _users = new();

    protected Company()
    {
    }

    private Company(
        Guid ownerUserId,
        string name,
        BusinessType businessType,
        Address address,
        Contact contact)
    {
        OwnerUserId = ownerUserId;
        Name = name;
        BusinessType = businessType;
        Address = address;
        Contact = contact;

        Status = CompanyStatus.PendingConfiguration;
        WizardStep = CompanyWizardStep.BasicInformation;

        _users.Add(new CompanyUser(ownerUserId, CompanyRole.Owner));
    }

    #region Properties

    public Guid OwnerUserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string TaxNumber { get; private set; } = string.Empty;

    public string? LogoUrl { get; private set; }

    public BusinessType BusinessType { get; private set; }

    public CompanyStatus Status { get; private set; }

    public CompanyWizardStep WizardStep { get; private set; }

    public Contact Contact { get; private set; } = default!;

    public Address Address { get; private set; } = default!;

    public IReadOnlyCollection<CompanyUser> Users => _users.AsReadOnly();

    #endregion

    #region Factory

    public static Company Register(
        Guid ownerUserId,
        string name,
        BusinessType businessType,
        string country,
        string city)
    {
        return new Company(
            ownerUserId,
            name,
            businessType,
            new Address(
                string.Empty,
                city,
                string.Empty,
                country),
            new Contact(
                string.Empty,
                string.Empty));
    }

    #endregion

    #region Wizard

    public void CompleteBasicInformation(string slug)
    {
        Slug = slug;

        WizardStep = CompanyWizardStep.ContactInformation;
    }

    public void CompleteContactInformation(Contact contact)
    {
        Contact = contact;

        WizardStep = CompanyWizardStep.FiscalInformation;
    }

    public void CompleteFiscalInformation(
        string taxNumber,
        Address address)
    {
        TaxNumber = taxNumber;
        Address = address;

        WizardStep = CompanyWizardStep.Branding;
    }

    public void CompleteBranding(
        string? description,
        string? logoUrl)
    {
        Description = description;
        LogoUrl = logoUrl;

        WizardStep = CompanyWizardStep.Completed;

        Status = CompanyStatus.Active;
    }

    #endregion

    #region Company

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void ChangeBusinessType(BusinessType businessType)
    {
        BusinessType = businessType;
    }

    #endregion
    #region Authorization
    /// <summary>
    /// Garante que o utilizador é membro ativo desta empresa e tem
    /// permissão (Owner ou Admin) para executar ações de gestão
    /// (atualizar dados, convidar/remover membros, etc.).
    /// </summary>
    public void EnsureCanManage(Guid userId)
    {
        var member = _users.FirstOrDefault(x => x.UserId == userId && x.IsActive);

        if (member is null)
            throw new CompanyMembershipRequiredException(Id, userId);

        if (member.Role is not (CompanyRole.Owner or CompanyRole.Administrator))
            throw new InsufficientCompanyRoleException(Id, userId, member.Role);
    }

    public void EnsureIsOwner(Guid userId)
    {
        var member = _users.FirstOrDefault(x => x.UserId == userId && x.IsActive);

        if (member is null)
            throw new CompanyMembershipRequiredException(Id, userId);

        if (member.Role != CompanyRole.Owner)
            throw new InsufficientCompanyRoleException(Id, userId, member.Role);
    }

    #endregion


    #region Status

    public void Activate()
    {
        Status = CompanyStatus.Active;
    }

    public void Suspend()
    {
        Status = CompanyStatus.Suspended;
    }

    public void Deactivate()
    {
        Status = CompanyStatus.Inactive;
    }

    #endregion

    #region Users

    public void InviteUser(Guid userId, CompanyRole role)
    {
        if (_users.Any(x => x.UserId == userId))
            throw new InvalidOperationException("User already belongs to this company.");

        _users.Add(new CompanyUser(userId, role));
    }

    public void RemoveUser(Guid userId)
    {
        var member = _users.FirstOrDefault(x => x.UserId == userId);

        if (member is null)
            return;

        if (member.Role == CompanyRole.Owner)
            throw new InvalidOperationException("The owner cannot be removed.");

        member.Deactivate();
    }

    #endregion



}