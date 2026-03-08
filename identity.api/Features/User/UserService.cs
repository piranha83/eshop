using Identity.Api.DatabaseContext;
using Identity.Api.DatabaseContext.Models;
using Infrastructure.Core;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Core.Consts;

namespace Identity.Api.Featres.User;

///<inheritdoc/>
internal class UserService(ApplicationDbContext context)
    : IUserService
{
    ///<inheritdoc/>
    public async Task<UserEntity?> SignIn(string userName, string password, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);

        using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                // Идентификация.
                var user = await context.Users.ForUpdate()
                    .SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower(), cancellationToken);
                if (user != null)
                {
                    // Аутентификация.
                    if (!user.CheckPasswordSignIn(password, 5, cancellationToken))
                    {
                        // Блокировка.
                        user.Lock();
                    }

                    user.LastLogin = DateTimeOffset.UtcNow;
                    context.Update(user);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return user;
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        return null;
    }

    ///<inheritdoc/>
    public async Task<bool> Add(string userName, byte[] password, ClaimsRoles roles, CancellationToken cancellationToken = default)
    {
        if (await context.Users.AllAsync(x => x.UserName != userName, cancellationToken))
        {
            await context.Users.AddAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                PasswordHash = password,
                ExternalId = Guid.NewGuid().ToString(),
                Roles = roles
            }, cancellationToken);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
        return false;
    }

    ///<inheritdoc/>
    public async Task<int> Unblock(CancellationToken cancellationToken = default)
    {
        var date = DateTimeOffset.UtcNow.AddMinutes(-Consts.UsersUnblockTimeMinutes);
        return await context.Users
            .Where(x => x.IsDeleted && x.UpdatedDate != null && x.UpdatedDate <= date)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsDeleted, false).SetProperty(x => x.LockoutAttempts, 0), cancellationToken);
    }
}