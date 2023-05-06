using Microsoft.Extensions.Caching.Memory;
using TggWeb.Data.Contexts;
using TggWeb.Data.Seeders;
using TggWeb.Services.Webs;

var context = new WebDbContext();

var seeder = new DataSeeder(context);

seeder.Initialize();

var categories = context.Categories.ToList();
//ICategoryRepository webRepository = new CategoryRepository(context);

//var cats = await webRepository.GetCategoriesAsync();

foreach (var cat in categories)
{
	Console.WriteLine("ID        : {0}", cat.Id);
	Console.WriteLine("Name     : {0}", cat.Name);
}