﻿using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class FeaturedPosts : ViewComponent
	{
		private readonly IBlogRepository _blogRepository;
		public FeaturedPosts(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var post = await _blogRepository.GetFeaturePostAysnc(3);
			return View(post);
		}
	}
}
