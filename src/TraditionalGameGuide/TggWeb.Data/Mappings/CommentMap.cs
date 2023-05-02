using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TggWeb.Core.Entities;

namespace TggWeb.Data.Mappings
{
	public class CommentMap : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.ToTable("Comment");
			builder.HasKey(c => c.Id);

			builder.Property(c => c.Id)
				.HasColumnName("CommentId")
				.ValueGeneratedOnAdd();

			builder.Property(c => c.Content)
				.IsRequired();

			builder.Property(c => c.CreatedDate)
				.IsRequired();

			builder.Property(c => c.IsApproved)
				.IsRequired();

			builder.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.HasForeignKey(c => c.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(c => c.Subscriber)
				.WithMany(s => s.Comments)
				.HasForeignKey(c => c.SubscriberId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
