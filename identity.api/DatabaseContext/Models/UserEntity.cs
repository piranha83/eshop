namespace Identity.Api.DatabaseContext.Models;

using Infrastructure.Core.Abstractions;
using static Infrastructure.Core.Consts;

///<inheritdoc/>
public class UserEntity : IEntity<Guid>, IUpdated
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Внешний идентификатор.
    /// </summary>
    public string ExternalId { get; set; } = default!;

    /// <summary>
    /// Логин.
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// Пароль.
    /// </summary>
    public byte[] PasswordHash { get; set; } = default!;

    /// <summary>
    /// Кто редактировал.
    /// </summary>
    public Guid? Updated { get; set; }
    
    /// <summary>
    /// Дата редактирования.
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Неудачные попытки.
    /// </summary>
    public int LockoutAttempts { get; set; }

    /// <summary>
    /// Маягко удален.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Роли.
    /// </summary>
    public ClaimsRoles Roles { get; set; }   
 
    /// <summary>
    /// Дата входа.
    /// </summary>
    public DateTimeOffset? LastLogin { get; set; }
}