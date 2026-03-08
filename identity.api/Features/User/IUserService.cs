using Identity.Api.DatabaseContext.Models;
using static Infrastructure.Core.Consts;

namespace Identity.Api.Featres.User;

/// <summary>
/// Представляет сервис авторизации пользователя по паролю.
/// </summary>
//// <typeparam name="IEntity"></typeparam>
internal interface IUserService<IEntity>
{
    /// <summary>
    /// Проверить возможность авторизации по паролю пользователя.
    /// </summary>
    /// <param name="userName">Имя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Польователя, если пароль верный.</returns>
    Task<UserEntity?> SignIn(string userName, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить пользователя.
    /// </summary>
    /// <param name="userName">Имя.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="roles">Роли.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>True - создан.</returns>
    Task<bool> Add(string userName, byte[] password, ClaimsRoles roles, CancellationToken cancellationToken);

    /// <summary>
    /// Разблокировать пользователей.
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Сколько рзаблокировано.</returns>
    Task<int> Unblock(CancellationToken cancellationToken = default);
}

/// <summary>
/// Представляет сервис авторизации пользователя по паролю.
/// </summary>
internal interface IUserService : IUserService<UserEntity>;