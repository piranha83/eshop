using Microsoft.AspNetCore.JsonPatch;

namespace Infrastructure.Core;

/// <summary>
/// Модель.
/// </summary>
public class PatchModel<TEntity>: JsonPatchDocument<TEntity>
where TEntity: class
{   
}