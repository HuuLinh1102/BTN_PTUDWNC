using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TggWeb.Core.Entities;

namespace TggWeb.Data.Mappings
{
	public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
	{
		public void Configure(EntityTypeBuilder<Subscriber> builder)
		{
			builder.ToTable("Subscriber");
			builder.HasKey(s => s.Id);
			builder.Property(s => s.Email)
				.IsRequired()
				.HasMaxLength(255);

			builder.Property(s => s.SubscriptionDate)
				.IsRequired();

			builder.Property(s => s.UnsubscribeReason)
				.HasMaxLength(500);

			builder.Property(s => s.IsUserInitiatedUnsubscribe)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(s => s.AdminNote)
				.HasMaxLength(500);
		}
	}
}
