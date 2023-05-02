using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using TggWeb.Data.Contexts;
using TggWeb.Services.Webs;

namespace TggWeb.WebApi.Models
{
	public class PostFilterModel
	{

		public string Keyword { get; set; }

		public int? GameId { get; set; }

		public int? Year { get; set; }

		public int? Month { get; set; }

		public IEnumerable<SelectListItem> GameList { get; set; }

		public IEnumerable<SelectListItem> MonthList { get; set; }

		public PostFilterModel()
		{
			MonthList = Enumerable.Range(1, 12)
				.Select(m => new SelectListItem()
				{
					Value = m.ToString(),
					Text = CultureInfo.CurrentCulture
						 .DateTimeFormat.GetMonthName(m)
				}).ToList();
		}

	}
}
