using FluentValidation;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Validations
{
	public class CommentValidator : AbstractValidator<CommentEditModel>
	{
		public CommentValidator()
		{
			RuleFor(c => c.Content)
				.NotEmpty()
				.WithMessage("Nội dung bình luận không được để trống")
				.MaximumLength(200)
				.WithMessage("Nội dung bình luận dài tối đa 200 ký tự");

		}
	}

}
