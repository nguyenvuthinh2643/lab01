using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.Services.Blogs
{

    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }


        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }


        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }


        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }


        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }




        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.Urlslug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }


        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagQuery = _context.Set<Tag>().Include(i => i.Posts);

            if (!string.IsNullOrWhiteSpace(slug))
            {
                tagQuery = tagQuery.Where(x => x.Urlslug == slug);
            }

            return await tagQuery.FirstOrDefaultAsync(cancellationToken);

        }
        public async Task<IPagedList<Post>> GetPagedPostAsync(
            PostQuery condition,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken
                );
        }

        private IQueryable<Post> FilterPosts(PostQuery query)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.Tags);

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                postsQuery = postsQuery
                  .Where(p => p.Title.Contains(query.Keyword)
                    || p.Description.Contains(query.Keyword)
                    || p.ShortDescription.Contains(query.Keyword)
                    || p.UrlSlug.Contains(query.Keyword)
                    || p.Tags.Any(t => t.Name.Contains(query.Keyword))
                  );
            }

            if (query.PostedMonth > 0)
            {
                postsQuery = postsQuery
                  .Where(p => p.PostedDate.Month == query.PostedMonth);
            }

            if (query.CategoryId > 0)
            {
                postsQuery = postsQuery
                  .Where(p => p.CategoryId == query.CategoryId);
            }

            if (query.AuthorId > 0)
            {
                postsQuery = postsQuery
                  .Where(p => p.AuthorId == query.AuthorId);
            }

            if (!string.IsNullOrEmpty(query.CategoryName))
            {
                postsQuery = postsQuery
                    .Where(p => p.Category.Name == query.CategoryName);
            }

            if (!string.IsNullOrWhiteSpace(query.CategorySlug))
            {
                postsQuery = postsQuery
                  .Where(p => p.Category.UrlSlug == query.CategorySlug);
            }

            if (!string.IsNullOrWhiteSpace(query.AuthorSlug))
            {
                postsQuery = postsQuery
                  .Where(p => p.Author.Urlslug == query.AuthorSlug);
            }


            if (!string.IsNullOrWhiteSpace(query.TagSlug))
            {
                postsQuery = postsQuery
                  .Where(p => p.Tags.Any(t => t.Urlslug == query.TagSlug));
            }

            if (query.PublishedOnly)
            {
                postsQuery = postsQuery.Where(p => p.Published);
            }

            if (query.NotPublished)
            {
                postsQuery = postsQuery.Where(p => !p.Published);
            }

            var selectedTags = query.GetSelectedTags();
            if (selectedTags.Count > 0)
            {
                foreach (var tag in selectedTags)
                {
                    postsQuery = postsQuery.Include(p => p.Tags)
                      .Where(p => p.Tags.Any(t => t.Name == tag));
                }
            }

            return postsQuery;
        }

        public async Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery postQuery, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsFindResultQuery = FilterPosts(postQuery);
            return await postsFindResultQuery
              .ToPagedListAsync(pageNumber, pageSize, cancellationToken: cancellationToken);
        }

        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<Author> author = _context.Set<Author>();
            return await author
                .OrderBy(x => x.FullName)
                .Select(x => new AuthorItem()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Urlslug = x.Urlslug,
                    Email = x.Email,
                    JoinedDate = x.JoinedDate,
                    ImageUrl = x.ImageUrl,
                    Notes = x.Notes,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public Task GetPostAsync(int id, bool v)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> GetPostByIdAsync(int postId, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }

        public async Task<Post> CreateOrUpdatePostAsync(
         Post post, IEnumerable<string> tags,
         CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.Urlslug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    Urlslug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.Urlslug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }

        public async Task<Tag> GetTagAsync(
    string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(x => x.Urlslug == slug, cancellationToken);
        }
        public async Task<IList<TagItem>> GetTagsAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.Urlslug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }
        public async Task<IList<Post>> GetFeaturePostAysnc(
          int numberPost,
          CancellationToken cancellationToken = default)
        {

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .OrderByDescending(x => x.ViewCount)
                .Take(numberPost)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(x => Guid.NewGuid())
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }
        public async Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tags = _context.Set<Tag>();
            return await tags
              .OrderBy(t => t.Name)
              .Select(t => new TagItem()
              {
                  Id = t.Id,
                  Name = t.Name,
                  UrlSlug = t.Urlslug,
                  Description = t.Description,
                  PostCount = t.Posts.Count(p => p.Published)
              })
              .ToListAsync(cancellationToken);
            ;
        }

       

        public async Task<Category> FindCategoryByIdAsync(int id, bool isDetail = false, CancellationToken cancellationToken = default)
        {
            if (isDetail)
            {
                return await _context
                  .Set<Category>()
                  .Include(c => c.Posts)
                  .Where(c => c.Id == id)
                  .FirstOrDefaultAsync(cancellationToken);
            }

            return await _context
              .Set<Category>()
              .FindAsync(id, cancellationToken);
        }

        public async Task ChangeCategoriesShowOnMenu(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Category>()
          .Where(c => c.Id == id)
          .ExecuteUpdateAsync(
              c => c.SetProperty(c => c.ShowOnMenu, c => !c.ShowOnMenu),
              cancellationToken);
        }
        public async Task<bool> DeleteCategoryByIdAsync(
      int id,
      CancellationToken cancellationToken = default
    )
        {
            var categoryToDelete = await _context
              .Set<Category>()
              .Where(c => c.Id == id)
              .FirstOrDefaultAsync(cancellationToken);

            if (categoryToDelete == null)
            {
                return false;
            }

            _context.Set<Category>().Remove(categoryToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<Category> AddOrUpdateCategoryAsync(
      Category category,
      CancellationToken cancellationToken = default
    )
        {
            if (category.Id > 0)
            {
                _context.Set<Category>().Update(category);
            }
            else
            {
                _context.Set<Category>().Add(category);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return category;
        }

        //public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        // {
        //     IQueryable<CategoryItem> categoriesQuery = _context.Set<Category>()
        //  .Select(c => new CategoryItem()
        //  {
        //      Id = c.Id,
        //      Description = c.Description,
        //      Name = c.Name,
        //      ShowOnMenu = c.ShowOnMenu, 
        //      UrlSlug = c.UrlSlug,
        //      PostCount = c.Posts.Count(p => p.Published), });

        //     return await categoriesQuery
        //       .ToPagedListAsync(pagingParams, cancellationToken);
        // }

        public async Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(Func<IQueryable<Post>,
            IQueryable<T>> mapper, PostQuery query, IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            return await mapper(FilterPosts(query).AsNoTracking()).ToPagedListAsync(pagingParams, cancellationToken);
        }


    } 
       
    }

