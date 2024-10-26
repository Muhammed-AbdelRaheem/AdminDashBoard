using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Identity;
using Store.Core.Repositories.Contract;
using Store.DashBoard.Helpers;
using Store.Repository;
using Store.Repository.Data.Contexts;
using Store.Repository.Identity.Contexts;

namespace Store.DashBoard
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<StoreDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddDbContext<StoreIdentityDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

			builder.Services.AddIdentity<AppUser, IdentityRole>()
					 .AddEntityFrameworkStores<StoreIdentityDbContext>();

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddAutoMapper(typeof(MapsProfile));



            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Admin}/{action=Login}/{id?}");

			app.Run();
		}
	}
}
