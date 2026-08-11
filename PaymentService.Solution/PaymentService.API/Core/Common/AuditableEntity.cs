namespace PaymentService.Core.Common;

public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// Utilizador que criou o registo.
    /// </summary>
    public string? CreatedBy { get; protected set; }

    /// <summary>
    /// Data da última alteração.
    /// </summary>
    public DateTime? LastModifiedAt { get; protected set; }

    /// <summary>
    /// Utilizador que efetuou a última alteração.
    /// </summary>
    public string? LastModifiedBy { get; protected set; }

    /// <summary>
    /// Data da eliminação lógica.
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// Utilizador que efetuou a eliminação lógica.
    /// </summary>
    public string? DeletedBy { get; protected set; }

    /// <summary>
    /// Define o utilizador responsável pela criação.
    /// Deve ser chamado apenas durante a criação da entidade.
    /// </summary>
    protected void SetCreatedBy(string? user)
    {
        CreatedBy = user;
    }

    /// <summary>
    /// Atualiza as informações de auditoria.
    /// </summary>
    protected void SetModified(string? user)
    {
        LastModifiedAt = DateTime.UtcNow;
        LastModifiedBy = user;

        MarkAsUpdated();
    }

    /// <summary>
    /// Efetua uma eliminação lógica.
    /// </summary>
    protected void SetDeleted(string? user)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = user;

        SoftDelete();
    }
}