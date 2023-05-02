using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;

namespace TggWeb.Services.Webs
{
	public interface ISubscriberRepository
	{
		Task<Subscriber> GetSubscriberByIdAsync(int subscriberId);

		Task<Subscriber> GetCachedSubscriberByIdAsync(int subscriberId);

		Task<IList<SubscriberItem>> GetSubscribersAsync(
			CancellationToken cancellationToken = default);

		Task<IPagedList<SubscriberItem>> GetPagedSubscribersAsync(
			IPagingParams pagingParams,
			string email = null,
			CancellationToken cancellationToken = default);

		Task<IPagedList<Subscriber>> GetPagedSubscribersQueryAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);

		Task<IPagedList<T>> GetPagedTagsAsync<T>(
			Func<IQueryable<Subscriber>,
			IQueryable<T>> mapper,
			IPagingParams pagingParams,
			string email = null,
			CancellationToken cancellationToken = default);

		Task<bool> AddOrUpdateAsync(
			Subscriber subscriber,
			CancellationToken cancellationToken = default);

		Task<bool> DeleteSubscriberAsync(
			int subscriberId,
			CancellationToken cancellationToken = default);

		Task<bool> IsSubscriberEmailExistedAsync(
			int subscriberId,
			string email,
			CancellationToken cancellationToken = default);


	}
}
