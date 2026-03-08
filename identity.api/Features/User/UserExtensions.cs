using System.Security.Cryptography;
using System.Text;
using Identity.Api.DatabaseContext.Models;

namespace Identity.Api.Featres.User;

internal static class UserExtensions
{
    public static bool CheckPasswordSignIn(this UserEntity user, string password, int lockoutAttempts = 5, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(password);

        if (user.IsDeleted)
        {
            return false;
        }

        if (user.LockoutAttempts > lockoutAttempts)
        {
            return false;
        }

        if (!password.Hash512().SequenceEqual(user.PasswordHash))
        {
            return false;
        }

        if ((user.LockoutAttempts += 1) >= lockoutAttempts)
        {
            return false;
        }

        return true;
    }

    public static bool Lock(this UserEntity user)
    {
        if (!user.IsDeleted || user.LockoutAttempts > 0)
        {
            user.IsDeleted = true;
            user.LockoutAttempts = 0;
            return true;
        }
        return false;
    }

    internal static byte[] Hash512(this string? password) =>
        SHA512.HashData(Encoding.UTF8.GetBytes(password ?? ""));
}