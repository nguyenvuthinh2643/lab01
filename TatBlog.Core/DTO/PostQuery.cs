﻿namespace TatBlog.Core.DTO
{
	public class PostQuery
	{
	
		public string Keyword { get; set; }
		public int? CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string CategorySlug { get; set; }
		public int? AuthorId { get; set; }
		public string AuthorSlug { get; set; }
		public string TagSlug { get; set; }
		public int? PostedMonth { get; set; }
		public int? PostedYear { get; set; }
		public string SelectedTags { get; set; }
		public bool PublishedOnly { get; set; }
		public bool NotPublished { get; set; }

		public List<string> GetSelectedTags()
		{
			return (SelectedTags ?? "")
			  .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			  .ToList();
		}
	}
}