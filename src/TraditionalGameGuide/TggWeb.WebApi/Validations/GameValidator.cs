using FluentValidation;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Validations
{
	public class GameValidator: AbstractValidator<GameEditModel>
	{
		public GameValidator() 
		{
			RuleFor(g => g.Name)
				.NotEmpty()
				.WithMessage("Tên trò chơi không được để trống")
				.MaximumLength(100)
				.WithMessage("Tên trò chơi dài tối đa 100 ký tự");

			RuleFor(g => g.UrlSlug)
				.NotEmpty()
				.WithMessage("Slug không được để trống")
				.MaximumLength(100)
				.WithMessage("Slug dài tối đa 100 ký tự");

			RuleFor(g => g.PlayerCount)
				.NotEmpty()
				.WithMessage("Số người chơi không được để trống");

			RuleFor(g => g.Description)
				.NotEmpty()
				.WithMessage("Mô tả không được để trống")
				.MaximumLength(1000)
				.WithMessage("Mô tả chứa tối đa 1000 ký tự");

			RuleFor(g => g.Age)
				.NotEmpty()
				.WithMessage("Độ tuổi không được để trống");

			RuleFor(g => g.CategoryId)
				.NotEmpty()
				.WithMessage("Bạn phải chọn danh mục của trò chơi");
		}
	}
}
