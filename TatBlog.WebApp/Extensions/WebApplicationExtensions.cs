using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Middlewares;

namespace TatBlog.WebApp.Extensions
{
	public static class WebApplicationExtensions
	{
		public static WebApplicationBuilder ConfigureMvc(
		  this WebApplicationBuilder builder)
		{
			builder.Services.AddControllersWithViews();
			builder.Services.AddResponseCompression();
			return builder;
		}

		public static WebApplicationBuilder ConfigureServices(
		  this WebApplicationBuilder builder)
		{
			builder.Logging.ClearProviders();
			builder.Host.UseNLog();
			builder.Services.AddDbContext<BlogDbContext>(options =>
			  options.UseSqlServer(
				builder.Configuration
				  .GetConnectionString("DefaultConnection")));
            //builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
            builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();

			return builder;
		}
		// Cấu hình HTTP Request pipeline
		public static WebApplication UseRequestPipeline(
		  this WebApplication app)
		{
			// Thêm middleware để hiển thị thông báo lỗi 
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Blog/Error");

				// Thêm middleware cho việc áp dụng HSTS (thêm header Strict-Transport-Security vaof HTTP Response).
				app.UseHsts();
			}

			//Thêm middleware để tự động nén HTTP response
			app.UseResponseCompression();

			//Thêm middleware để chuyển hướng HTTP sang HTTPS
			app.UseHttpsRedirection();
			// Thêm middleware phục vụ các yêu cầu liên quan tới các tập tin dung tĩnh như hình ảnh, css,...
			app.UseStaticFiles();

			// Thêm middleware lựa chọn endpoint phù hợp nhất để xử lý một HTTP request
			app.UseRouting();
			return app;
		}

		// Thêm dữ liệu mẫu vào csdl
		public static IApplicationBuilder UseDataSeeder(
		  this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			try
			{
				scope.ServiceProvider
				  .GetRequiredService<IDataSeeder>()
				  .Initialize();
			}
			catch (Exception ex)
			{
				scope.ServiceProvider
					.GetRequiredService<ILogger<Program>>()
					.LogError(ex, "Could not insert data into database");
			}
			app.UseMiddleware<UserActivityMiddleware>();
			return app;
		}
	
	}
}
