using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Controllers;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Properties.Controllers
{
	public class BlogController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly ILogger<PostsController> _logger;
		private readonly IMediaManager _mediaManager;
		private readonly IMapper _mapper;


		public BlogController(IBlogRepository blogRepository,ILogger<PostsController> logger, IMediaManager mediaManager,IMapper mapper)
		{
			
			_blogRepository = blogRepository;
			_logger = logger;
			_mediaManager = mediaManager;
			_mapper = mapper;

		}
		
        public async Task<IActionResult> Index(
			[FromQuery(Name = "k")] string keyword = null,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{
			
			PostQuery query = new PostQuery()
			{

				PublishedOnly = true,
				Keyword = (keyword)
					
			};
			var postsList = await _blogRepository.GetPagedPostsAsync(query, pageNumber, pageSize);
			ViewBag.PostQuery = query;
			return View(postsList);
		}

		private static string GetKeyword(string keyword)
		{
			return keyword;
		}

		public IActionResult About()
			=> View();
		public IActionResult Contact() => View();
		public IActionResult Rss() => View();
	}
}
