using System.Security.Claims;
using Identity.Api.DatabaseContext.Models;

namespace Identity.Api.Featres.User;

/// <summary>
/// Представляет сервис авторизации пользователя по паролю.
/// </summary>
//// <typeparam name="IEntity"></typeparam>
internal interface IUserService<IEntity>
{
    /// <summary>
    /// Найти пользователя по имени.
    /// </summary>
    /// <param name="userName">Имя пользователя.</param>
    /// <returns>Пользователь или null.</returns>
    Task<IEntity?> FindByName(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Найти пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Пользователь или null.</returns>
    Task<IEntity?> FindById(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить возможность авторизации по паролю пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="password">Пароль польователя.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>True если пароль польователя верный.</returns>
    Task<bool> CheckPasswordSignIn(IEntity user, string password, CancellationToken cancellationToken = default);
}

/// <summary>
/// Представляет сервис авторизации пользователя по паролю.
/// </summary>
internal interface IUserService : IUserService<UserEntity>;