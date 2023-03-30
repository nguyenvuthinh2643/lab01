using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        
        public Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);

        public Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);
        public Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);

        public Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);
   
        public Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);

      
        public Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        public Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);
		Task<IPagedList<Post>> GetPagedPostsAsync(
		    PostQuery postQuery,
		   int pageNumber = 1,
		   int pageSize = 10,
		   CancellationToken cancellationToken = default);
		//Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);
        Task<Post> GetPostByIdAsync(int postId, bool includeDetails = false, CancellationToken cancellationToken = default);
    
        Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default);
        Task<IList<TagItem>> GetTagsAsync(
        CancellationToken cancellationToken = default);
        Task<IList<Post>> GetFeaturePostAysnc(
          int numberPost,
          CancellationToken cancellationToken = default);
        Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default);
        Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default);
    

        Task<Category> FindCategoryByIdAsync(
      int id,
      bool isDetail = false,
      CancellationToken cancellationToken = default
    );
        Task ChangeCategoriesShowOnMenu(int id, CancellationToken cancellationToken = default);
        Task<bool> DeleteCategoryByIdAsync(
      int id,
      CancellationToken cancellationToken = default
    );
        Task<Category> AddOrUpdateCategoryAsync(
      Category category,
      CancellationToken cancellationToken = default
    );
        //    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        //  IPagingParams pagingParams,
        //  CancellationToken cancellationToken = default
        //);
        Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper,
            PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);
    }
}