using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.MoneyTransfer.Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.MoneyTransfer.Application.Common.Mappings;

public static class MappingExtensions
{
    public static async Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, 
        int pageNumber, int pageSize, OrderByStatus orderBy = OrderByStatus.Asc, string sortBy = null)
    {
        var count = await queryable.CountAsync();
        if (sortBy is not null)
        {
            var sortExpression = GetSortExpression<TDestination>(sortBy);
            if (sortExpression is not null)
            {
                if (orderBy == OrderByStatus.Asc)
                {
                    queryable = queryable.OrderBy(sortExpression);
                }
                else
                {
                    queryable = queryable.OrderByDescending(sortExpression);
                }
            }
        }

        var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<TDestination>(items, count, pageNumber, pageSize, orderBy, sortBy);
    }

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
    {
        return queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }

    public static Expression<Func<TDestination, object>> GetSortExpression<TDestination>(string propertyName)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(TDestination), "entity");
            var prop = Expression.Property(parameter, propertyName);
            Expression conversion = Expression.Convert(prop, typeof(object));
            var expression = Expression.Lambda<Func<TDestination, Object>>(conversion, parameter);

            return expression;
        }

        catch (Exception)
        {
            return null;
        }
    }
}