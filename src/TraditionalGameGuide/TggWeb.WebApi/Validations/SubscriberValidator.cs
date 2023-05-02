using FluentValidation;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Validations
{
	public class SubscriberValidator : AbstractValidator<SubscriberEditModel>
	{
		public SubscriberValidator()
		{
			RuleFor(s => s.Email)
				.NotEmpty()
				.WithMessage("Email không được để trống")
				.EmailAddress()
				.WithMessage("Email không hợp lệ");

		}
	}
}
