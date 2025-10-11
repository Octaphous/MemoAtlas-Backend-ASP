using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Models.Entities.Interfaces;

namespace MemoAtlas_Backend.Api.Utilities;

public static class QueryableExtensions
{
    public static IQueryable<T> VisibleToUser<T>(this IQueryable<T> query, User user) where T : IPrivatable
    {
        return query.Where(t => !t.Private || (t.Private && user.PrivateMode));
    }
}
