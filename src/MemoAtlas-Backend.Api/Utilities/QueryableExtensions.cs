using MemoAtlas_Backend.Api.Models;
using MemoAtlas_Backend.Api.Models.Entities;

namespace MemoAtlas_Backend.Api.Utilities;

public static class QueryableExtensions
{
    public static IQueryable<T> VisibleToUser<T>(this IQueryable<T> query, User user) where T : IPrivatable
    {
        return query.Where(t => !t.Private || (t.Private && user.PrivateMode));
    }
}
