namespace Identity.Api.DatabaseContext.Models;

using System.ComponentModel;
using Infrastructure.Core.Abstractions;
using static Infrastructure.Core.Consts;

///<inheritdoc/>
public class UserEntity : IEntity<Guid>, IUpdated
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Description("Идентификатор")]
    public Guid Id { get; set; }

    /// <summary>
    /// Внешний идентификатор.
    /// </summary>
    [Description("Внешний идентификатор")]
    public string ExternalId { get; set; } = default!;

    /// <summary>
    /// Логин.
    /// </summary>
    [Description("Логин")]
    public string UserName { get; set; } = default!;

    /// <summary>
    /// Пароль.
    /// </summary>
    [Description("Пароль")]
    public byte[] PasswordHash { get; set; } = default!;

    /// <summary>
    /// Кто редактировал.
    /// </summary>
    [Description("Кто редактировал")]
    public Guid? Updated { get; set; }
    
    /// <summary>
    /// Дата редактирования.
    /// </summary>
    [Description("Дата редактирования")]
    public DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Неудачные попытки.
    /// </summary>
    [Description("Неудачные попытки")]
    public int LockoutAttempts { get; set; }

    /// <summary>
    /// Маягко удален.
    /// </summary>
    [Description("Маягко удален")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Роли.
    /// </summary>
    [Description("Роли")]
    public ClaimsRoles Roles { get; set; }
}