using Identity.Api.DatabaseContext;
using Identity.Api.DatabaseContext.Models;
using Identity.Api.Featres.Flow;
using Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Featres.User;

///<inheritdoc/>
internal class UserService(
    ApplicationDbContext context,
    int lockoutAttempts = 5)
    : IUserService
{
    ///<inheritdoc/>
    public Task<UserEntity?> FindByName(string userName, CancellationToken cancellationToken = default) =>
        context.Users.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

    ///<inheritdoc/>
    public Task<UserEntity?> FindById(string userId, CancellationToken cancellationToken = default) =>
        context.Users.FirstOrDefaultAsync(x => x.ExternalId == userId, cancellationToken);

    ///<inheritdoc/>
    public async Task<bool> Add(string userName, byte[] password, CancellationToken cancellationToken = default)
    {
        if (await context.Users.AllAsync(x => x.UserName != userName, cancellationToken))
        {
            await context.Users.AddAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                PasswordHash = password,
                ExternalId = Guid.NewGuid().ToString()
            }, cancellationToken);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
        return false;
    }

    ///<inheritdoc/>
    public async Task<bool> CheckPasswordSignIn(UserEntity user, string password, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(password);

        if (user.IsDeleted)
        {
            return false;
        }

        if (user.LockoutAttempts < lockoutAttempts && password.Hash512().SequenceEqual(user.PasswordHash))
        {
            return true;
        }

        if ((user.LockoutAttempts += 1) >= lockoutAttempts)
        {
            user.IsDeleted = true;
            user.LockoutAttempts = 0;
        }

        await context.SaveChangesAsync(cancellationToken);

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