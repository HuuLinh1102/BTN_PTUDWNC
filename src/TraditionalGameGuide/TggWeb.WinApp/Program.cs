using TggWeb.Data.Contexts;
using TggWeb.Data.Seeders;

var context = new WebDbContext();
var seeder = new DataSeeder(context);

seeder.Initialize();

var games = context.Games.ToList();

foreach (var game in games)
{
	Console.WriteLine(game.Name);
}	