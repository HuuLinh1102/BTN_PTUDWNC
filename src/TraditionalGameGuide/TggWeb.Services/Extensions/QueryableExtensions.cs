using System.Linq.Expressions;

namespace TggWeb.Services.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> WhereIf<T>(
			this IQueryable<T> source,
			bool condition,
			Expression<Func<T, bool>> predicate)
		{
			if (condition)
			{
				return source.Where(predicate);
			}
			else
			{
				return source;
			}
		}
	}
}
