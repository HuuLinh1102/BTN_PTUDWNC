using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TggWeb.Core.Entities;

namespace TggWeb.Data.Mappings
{
    public class GameMap : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Game");
            
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.UrlSlug)
                .IsRequired()
                .HasMaxLength(100);

			builder.Property(c => c.Description)
				.HasMaxLength(500);

			builder.Property(a => a.ImageUrl)
                .HasMaxLength(500);

			builder.Property(p => p.Age)
				.IsRequired()
				.HasDefaultValue(0);

			builder.Property(p => p.PlayerCount)
				.IsRequired()
				.HasDefaultValue(0);

			builder.HasOne(p => p.Category)
				.WithMany(c => c.Games)
				.HasForeignKey(p => p.CategoryId)
				.HasConstraintName("FK_Games_Categories")
				.OnDelete(DeleteBehavior.Cascade);

        }
    }
}
