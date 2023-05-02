using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Contracts;
using TggWeb.Core.DTO;
using TggWeb.Core.Entities;
using TggWeb.Data.Contexts;
using TggWeb.Services.Extensions;

namespace TggWeb.Services.Webs
{
	public class SubscriberRepository : ISubscriberRepository
	{
		private readonly WebDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public SubscriberRepository(WebDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		public async Task<Subscriber> GetSubscriberByIdAsync(int subscriberId)
		{
			return await _context.Set<Subscriber>().FindAsync(subscriberId);
		}

		public async Task<Subscriber> GetCachedSubscriberByIdAsync(int subscriberId)
		{
			return await _memoryCache.GetOrCreateAsync(
				$"subscriber.by-id.{subscriberId}",
				async (entry) =>
				{
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
					return await GetSubscriberByIdAsync(subscriberId);
				});
		}

		public async Task<IList<SubscriberItem>> GetSubscribersAsync(
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Subscriber>()
				.OrderBy(s => s.Email)
				.Select(s => new SubscriberItem()
				{
					Id = s.Id,
					Email = s.Email,
					SubscriptionDate= s.SubscriptionDate,
					UnsubscribeDate= s.UnsubscribeDate,
					UnsubscribeReason= s.UnsubscribeReason,
					IsUserInitiatedUnsubscribe = s.IsUserInitiatedUnsubscribe,
					AdminNote= s.AdminNote,
					CommentsCount = s.Comments.Count(c => c.IsApproved)
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<IPagedList<SubscriberItem>> GetPagedSubscribersAsync(
			IPagingParams pagingParams,
			string email = null,
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Subscriber>()
				.AsNoTracking()
				.WhereIf(!string.IsNullOrWhiteSpace(email),
					s => s.Email.Contains(email))
				.Select(s => new SubscriberItem()
				{
					Id = s.Id,
					Email = s.Email,
					SubscriptionDate = s.SubscriptionDate,
					UnsubscribeDate = s.UnsubscribeDate,
					UnsubscribeReason = s.UnsubscribeReason,
					IsUserInitiatedUnsubscribe = s.IsUserInitiatedUnsubscribe,
					AdminNote = s.AdminNote,
					CommentsCount = s.Comments.Count(c => c.IsApproved)
				})
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<IPagedList<Subscriber>> GetPagedSubscribersQueryAsync(
			PostQuery condition,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			return await FilterSubscribers(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Subscriber.Email), "DESC",
				cancellationToken);
		}


		public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
			Func<IQueryable<Subscriber>,
			IQueryable<T>> mapper,
			IPagingParams pagingParams,
			string email = null,
			CancellationToken cancellationToken = default)
		{
			var subscriberQuery = _context.Set<Subscriber>().AsNoTracking();

			if (!string.IsNullOrEmpty(email))
			{
				subscriberQuery = subscriberQuery.Where(t => t.Email.Contains(email));
			}

			return await mapper(subscriberQuery)
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<bool> AddOrUpdateAsync(
			Subscriber subscriber,
			CancellationToken cancellationToken = default)
		{
			if (subscriber.Id > 0)
			{
				_context.Subscribers.Update(subscriber);
				_memoryCache.Remove($"subscriber.by-id.{subscriber.Id}");
			}
			else
			{
				_context.Subscribers.Add(subscriber);
			}

			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}

		public async Task<bool> DeleteSubscriberAsync(
			int subscriberId,
			CancellationToken cancellationToken = default)
		{
			return await _context.Subscribers
				.Where(x => x.Id == subscriberId)
				.ExecuteDeleteAsync(cancellationToken) > 0;
		}


		public async Task<bool> IsSubscriberEmailExistedAsync( 
			int subscriberId,
			string email,
			CancellationToken cancellationToken = default)
		{
			return await _context.Subscribers
				.AnyAsync(s => s.Id != subscriberId && s.Email == email, cancellationToken);
		}

		private IQueryable<Subscriber> FilterSubscribers(PostQuery condition)
		{
			IQueryable<Subscriber> subscribers = _context.Subscribers;

			if (!string.IsNullOrWhiteSpace(condition.Keyword))
			{
				subscribers = subscribers.Where(s => s.Email.Contains(condition.Keyword) ||
												s.AdminNote.Contains(condition.Keyword));
			}

			return subscribers;
		}
	}
}
