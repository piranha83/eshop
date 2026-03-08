using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Core.Interceptors;

///<inheritdoc/>
public sealed class SelectForUpdateInterceptor : DbCommandInterceptor
{
    ///<inheritdoc/>
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ModifyCommand(command);
        return result;
    }

    ///<inheritdoc/>
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        ModifyCommand(command);
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private static void ModifyCommand(DbCommand command)
    {
        if (command.CommandText.Contains(Consts.ForUpdateTag))
        {
            command.CommandText += " FOR UPDATE";
        }
    }
}