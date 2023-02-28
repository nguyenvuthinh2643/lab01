using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
var builder = WebApplication.CreateBuilder(args);
{
	// Thêm các dịch vụ được yêu cầu bởi MVC Framework
	builder.Services.AddControllersWithViews();
// Đăng ký các dịch vụ với DI Container
builder.Services.AddDbContext<BlogDbContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection")));
	builder.Services.AddScoped<IBlogRepository, BlogRepository>();
	builder.Services.AddScoped<IDataSeeder, DataSeeder>();
}
var app = builder.Build();
{
	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}
	else
	{
		app.UseExceptionHandler("/Blog/Error");
		app.UseHsts();
	}
	app.UseHttpsRedirection();
	app.UseStaticFiles();
	app.UseRouting();
	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Blog}/{action=Index}/{id?}"
		);
	// Thêm dữ liệu mẫu vào CSDL
	using (var scope = app.Services.CreateScope())
	{
		var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
		seeder.Initialize();
	}
}

app.Run();
