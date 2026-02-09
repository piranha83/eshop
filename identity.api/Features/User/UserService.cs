using Identity.Api.DatabaseContext;
using Identity.Api.DatabaseContext.Models;
using Identity.Api.Featres.Flow;
using Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Core.Consts;

namespace Identity.Api.Featres.User;

///<inheritdoc/>
internal class UserService(
    ApplicationDbContext context,
    int lockoutAttempts = 5)
    : IUserService
{
    private readonly IEnumerable<ClaimsRoles> roles = Enum.GetValues(typeof(ClaimsRoles)).Cast<ClaimsRoles>();

    ///<inheritdoc/>
    public Task<UserEntity?> FindByName(string userName, CancellationToken cancellationToken = default) =>
        context.Users.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

    ///<inheritdoc/>
    public Task<UserEntity?> FindById(string userId, CancellationToken cancellationToken = default) =>
        context.Users.FirstOrDefaultAsync(x => x.ExternalId == userId, cancellationToken);

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

    ///<inheritdoc/>
    public Task<IEnumerable<string>> GetRoles(UserEntity user, CancellationToken cancellationToken)
    {
        return Task.FromResult(roles
            .Where(x => user.Roles.HasFlag(x) && !x.Equals(Enum.Parse(typeof(ClaimsRoles), ClaimsRoles.None.ToString())))
            .Select(x => x.ToString()));
    }
}