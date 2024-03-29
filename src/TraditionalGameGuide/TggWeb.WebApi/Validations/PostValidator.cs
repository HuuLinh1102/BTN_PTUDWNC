﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TggWeb.Services.Webs;
using TggWeb.WebApi.Models;

namespace TggWeb.WebApi.Validations
{
	public class PostValidator: AbstractValidator<PostEditModel>
	{
		public readonly IWebRepository _webRepository;
		public PostValidator(IWebRepository webRepository) 
		{
			_webRepository = webRepository;
			RuleFor(x => x.Title)
				.NotEmpty()
				.MaximumLength(500)
				.WithMessage("Tiêu đề không được để trống");


			RuleFor(x => x.ShortDescription)
				.NotEmpty()
				.WithMessage("Nội dung tóm tắt không được để trống");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Nội dung không được để trống");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.MaximumLength(1000)
				.WithMessage("Tên định danh không được để trống");

			RuleFor(x => x.GameId)
				.NotEmpty()
				.WithMessage("Bạn phải chọn trò chơi của bài viết");

			RuleFor(x => x.SelectedTags)
				.Must(HasAtLeastOneTag)
				.NotEmpty()
				.WithMessage("Bạn phải chọn ít nhất một thẻ");

			When(x => x.Id <= 0, () =>
			{
				RuleFor(x => x.ImageFile)
				.Must(x => x is { Length: > 0 })
				.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
			})
			.Otherwise(() =>
			{
				RuleFor(x => x.ImageFile)
				.MustAsync(SetImageIfNotExist)
				.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
			});
		}
		// KT ng dùng đã nhập ít nhất 1 tag
		private bool HasAtLeastOneTag(
			PostEditModel postModel, string selectedTags)
		{
			return postModel.GetSelectedTag().Any();
		}

		// KT hình ảnh
		private async Task<bool> SetImageIfNotExist(
			PostEditModel postModel,
			IFormFile imageFile,
			CancellationToken cancellationToken)
		{
			var post = await _webRepository.GetPostByIdAsync(
				postModel.Id, false, cancellationToken);

			if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
				return true;

			return imageFile is { Length: > 0 };
		}
	}
}
