using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Entities;
using TggWeb.Data.Mappings;

namespace TggWeb.Data.Contexts
{
	public class WebDbContext : DbContext
	{
		public DbSet<Game> Games { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Post> Posts { get; set; }

		public DbSet<Tag> Tags { get; set; }

		public DbSet<Subscriber> Subscribers { get; set; }

		public DbSet<Comment> Comments { get; set; }

		//protected override void OnConfiguring
		//	(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer(@"server=quang-thanh\sqlexpress;database=TggWeb; trusted_connection=true;multipleactiveresultsets=true;trustservercertificate=true");
		//}
		public WebDbContext(DbContextOptions<WebDbContext> options)
		   : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.ApplyConfigurationsFromAssembly(
				typeof(CategoryMap).Assembly);
		}
	}
}
