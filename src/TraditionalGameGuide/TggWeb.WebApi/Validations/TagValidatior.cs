using FluentValidation;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Validations
{
	public class TagValidator : AbstractValidator<TagEditModel>
	{
		public TagValidator()
		{
			RuleFor(t => t.Name)
				.NotEmpty()
				.WithMessage("Tên thẻ không được để trống")
				.MaximumLength(100)
				.WithMessage("Tên thẻ dài tối đa 100 ký tự");

			RuleFor(t => t.UrlSlug)
				.NotEmpty()
				.WithMessage("Slug không được để trống")
				.MaximumLength(100)
				.WithMessage("Slug dài tối đa 100 ký tự");

			RuleFor(t => t.Description)
				.NotEmpty()
				.WithMessage("Mô tả không được để trống")
				.MaximumLength(1000)
				.WithMessage("Mô tả chứa tối đa 1000 ký tự");
		}
	}
}
