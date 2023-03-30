﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;
        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jason Mouth",
                    Urlslug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    Urlslug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoinedDate = new DateTime(2020, 4, 19)
                },
                new()
                {
                    FullName = "Author 3",
                    Urlslug = "author-3",
                    Email = "author3@gmail.com",
                    JoinedDate = new DateTime(2020, 3, 3)
                },
                new()
                {
                    FullName = "Author 4",
                    Urlslug = "author-4",
                    Email = "author4@gmail.com",
                    JoinedDate = new DateTime(2020, 4, 4)
                },
                new()
                {
                        FullName = "Author 5",
                    Urlslug = "author-5",
                    Email = "author5@gmail.com",
                    JoinedDate = new DateTime(2020, 5, 5)
                },
            };

            _dbContext.Author.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }
        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new() {Name = ".NET Core", Description = ".NET Core", UrlSlug = "dotnet-core"},
                new() {Name = "Architecture", Description = "Architecture", UrlSlug = "architecture"},
                new() {Name = "Messaging", Description = "Messaging", UrlSlug = "messaging"},
                new() {Name = "OOP", Description = "Object-Oriented Programming", UrlSlug = "oop"},
                new() {Name = "Design Patterns", Description = "Design Patterns", UrlSlug = "design-patterns"},
                new() {Name = "Category 6", Description = "Category 6", UrlSlug = "category-6"},
                new() {Name = "Category 7", Description = "Category 7", UrlSlug = "category-7"},
                new() {Name = "Category 8", Description = "Category 8", UrlSlug = "category-8"},
                new() {Name = "Category 9", Description = "Category 9", UrlSlug = "category-9"},
                new() {Name = "Category 10", Description = "Category 10", UrlSlug = "category-10"},
            };

            _dbContext.Categories.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }
        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                new() {Name = "Google", Description = "Google applications", Urlslug = "google"},
                new() {Name = "ASP.NEW MVC", Description = "ASP.NEW MVC", Urlslug = "aspdotnet-mvc"},
                new() {Name = "Razor Page", Description = "Razor Page", Urlslug = "razor-page"},
                new() {Name = "Blazor", Description = "Blazor", Urlslug = "blazor"},
                new() {Name = "Deep Learning", Description = "Deep Learning", Urlslug = "deep-learning"},
                new() {Name = "Neural Network", Description = "Neural Network", Urlslug = "neural-network"},
                new() {Name = "Tag 7", Description = "Tag 7", Urlslug = "tag-7"},
                new() {Name = "Tag 8", Description = "Tag 8", Urlslug = "tag-8"},
                new() {Name = "Tag 9", Description = "Tag 9", Urlslug = "tag-9"},
                new() {Name = "Tag 10", Description = "Tag 10", Urlslug = "tag-10"},
                new() {Name = "Tag 11", Description = "Tag 11", Urlslug = "tag-11"},
                new() {Name = "Tag 12", Description = "Tag 12", Urlslug = "tag-12"},
                new() {Name = "Tag 13", Description = "Tag 13", Urlslug = "tag-13"},
                new() {Name = "Tag 14", Description = "Tag 14", Urlslug = "tag-14"},
                new() {Name = "Tag 15", Description = "Tag 15", Urlslug = "tag-15"},
                new() {Name = "Tag 16", Description = "Tag 16", Urlslug = "tag-16"},
                new() {Name = "Tag 17", Description = "Tag 17", Urlslug = "tag-17"},
                new() {Name = "Tag 18", Description = "Tag 18", Urlslug = "tag-18"},
                new() {Name = "Tag 19", Description = "Tag 19", Urlslug = "tag-19"},
                new() {Name = "Tag 20", Description = "Tag 20", Urlslug = "tag-20"}
            };

            _dbContext.Tags.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }
        private IList<Post> AddPosts(
        IList<Author> authors,
        IList<Category> categories,
        IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new() {
                    Title = "ASP.NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository",
                    Description = "Here's a few great DON'T and Do examples",
                    Meta = "David and friends has a great repository",
                    UrlSlug = "aspdotnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
                new() {
                    Title = "Post 2",
                    ShortDescription = "Post 2 ",
                    Description = "Post 2 description",
                    Meta = "Post 2",
                    UrlSlug = "post-2",
                    Published = true,
                    PostedDate = new DateTime(2021, 4, 2, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 6,
                    Author = authors[1],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                },
                new() {
                    Title = "Post 3",
                    ShortDescription = "Post 3 ",
                    Description = "Post 3 description",
                    Meta = "Post 3 ",
                    UrlSlug = "post-3",
                    Published = true,
                    PostedDate = new DateTime(2021, 4, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 2,
                    Author = authors[2],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                        tags[2]
                    }
                },
                new() {
                    Title = "Post 4",
                    ShortDescription = "Post 4 ",
                    Description = "Post 4 description",
                    Meta = "Post 4 ",
                    UrlSlug = "post-4",
                    Published = true,
                    PostedDate = new DateTime(2021, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 2,
                    Author = authors[3],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[12]
                    }
                },
                new() {
                    Title = "Post 5",
                    ShortDescription = "Post 5 ",
                    Description = "Post 5 description",
                    Meta = "Post 5 ",
                    UrlSlug = "post-5",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 6",
                    ShortDescription = "Post 6 ",
                    Description = "Post 6 description",
                    Meta = "Post 6 ",
                    UrlSlug = "post-6",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 6, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 14,
                    Author = authors[1],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[15]
                    }
                },
                new() {
                    Title = "Post 7",
                    ShortDescription = "Post 7 ",
                    Description = "Post 7 description",
                    Meta = "Post 7 ",
                    UrlSlug = "post-7",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 5, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 1,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
                new() {
                    Title = "Post 8",
                    ShortDescription = "Post 8 ",
                    Description = "Post 8 description",
                    Meta = "Post 8 ",
                    UrlSlug = "post-8",
                    Published = true,
                    PostedDate = new DateTime(2022, 6, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 9",
                    ShortDescription = "Post 9 ",
                    Description = "Post 9 description",
                    Meta = "Post 9 ",
                    UrlSlug = "post-9",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 10",
                    ShortDescription = "Post 10 ",
                    Description = "Post 10 description",
                    Meta = "Post 10 ",
                    UrlSlug = "post-10",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[0],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 11",
                    ShortDescription = "Post 11 ",
                    Description = "Post 11 description",
                    Meta = "Post 11 ",
                    UrlSlug = "post-11",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 12",
                    ShortDescription = "Post 12 ",
                    Description = "Post 12 description",
                    Meta = "Post 12 ",
                    UrlSlug = "post-12",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 13",
                    ShortDescription = "Post 13 i",
                    Description = "Post 13 description",
                    Meta = "Post 13",
                    UrlSlug = "post-13",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 14",
                    ShortDescription = "Post 14 ",
                    Description = "Post 14 description",
                    Meta = "Post 14 ",
                    UrlSlug = "post-14",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 15",
                    ShortDescription = "Post 15 ",
                    Description = "Post 15 description",
                    Meta = "Post 15 ",
                    UrlSlug = "post-15",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 16",
                    ShortDescription = "Post 16 ",
                    Description = "Post 16 description",
                    Meta = "Post 16 ",
                    UrlSlug = "post-16",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 17",
                    ShortDescription = "Post 17 ",
                    Description = "Post 17 description",
                    Meta = "Post 17 ",
                    UrlSlug = "post-17",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 18",
                    ShortDescription = "Post 18 ",
                    Description = "Post 18 description",
                    Meta = "Post 18 ",
                    UrlSlug = "post-18",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[0],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 19",
                    ShortDescription = "Post 19 ",
                    Description = "Post 19 description",
                    Meta = "Post 19 ",
                    UrlSlug = "post-19",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[0],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 20",
                    ShortDescription = "Post 20",
                    Description = "Post 20 description",
                    Meta = "Post 20",
                    UrlSlug = "post-20",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 21",
                    ShortDescription = "Post 21 ",
                    Description = "Post 21 description",
                    Meta = "Post 21 ",
                    UrlSlug = "post-21",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 22",
                    ShortDescription = "Post 22 ",
                    Description = "Post 22 description",
                    Meta = "Post 22 ",
                    UrlSlug = "post-22",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 23",
                    ShortDescription = "Post 23 ",
                    Description = "Post 23 description",
                    Meta = "Post 23 ",
                    UrlSlug = "post-23",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 24",
                    ShortDescription = "Post 24 ",
                    Description = "Post 24 description",
                    Meta = "Post 24 ",
                    UrlSlug = "post-24",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[0],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 25",
                    ShortDescription = "Post 25",
                    Description = "Post 25 description",
                    Meta = "Post 25",
                    UrlSlug = "post-25",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 26",
                    ShortDescription = "Post 26 ",
                    Description = "Post 26 description",
                    Meta = "Post 26 ",
                    UrlSlug = "post-26",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 27",
                    ShortDescription = "Post 27 ",
                    Description = "Post 27 description",
                    Meta = "Post 27 ",
                    UrlSlug = "post-27",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 28",
                    ShortDescription = "Post 28 ",
                    Description = "Post 28 description",
                    Meta = "Post 28 ",
                    UrlSlug = "post-28",
                    Published = true,
                    PostedDate = new DateTime(2022, 2, 3, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[1],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                },
                new() {
                    Title = "Post 29",
                    ShortDescription = "Post 29 ",
                    Description = "Post 29 description",
                    Meta = "Post 29 ",
                    UrlSlug = "post-29",
                    Published = true,
                    PostedDate = new DateTime(2022, 5, 12, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 4,
                    Author = authors[3],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new() {
                    Title = "Post 30",
                    ShortDescription = "Post 30 ",
                    Description = "Post 30 description",
                    Meta = "Post 30",
                    UrlSlug = "post-30",
                    Published = true,
                    PostedDate = new DateTime(2022, 1, 1, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 16,
                    Author = authors[4],
                    Category = categories[9],
                    Tags = new List<Tag>()
                    {
                        tags[18]
                    }
                },
            };

            _dbContext.Posts.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }
    }
}
