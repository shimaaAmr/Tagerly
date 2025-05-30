﻿using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Mapping;
using Tagerly.Mapping.Admin;
using Tagerly.Models;
using Tagerly.Repositories.Implementations;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services;
using Tagerly.Services.Implementations;
using Tagerly.Services.Implementations.Admin;
using Tagerly.Services.Interfaces;
using Tagerly.Services.Interfaces.Admin;
using Tagerly.ViewModels.Configurations;

namespace Tagerly
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<TagerlyDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
			});
			builder.Services.AddScoped<IOrderRepo, OrderRepo>();
			builder.Services.AddScoped<ICartRepo, CartRepo>();
			builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
			builder.Services.AddScoped<IOrderRepo, OrderRepo>();
			builder.Services.AddScoped<IUserRepo, UserRepo>();
			builder.Services.AddScoped<IProductRepo, ProductRepo>();
			// Add Services
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IAdminProductService, AdminProductService>();
			builder.Services.AddScoped<ICartService, CartService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();
			builder.Services.AddScoped<IDashboardService, DashboardService>();

			builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

			builder.Services.AddControllersWithViews()
				.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SignUpViewModelValidator>());
			// AutoMapper
			builder.Services.AddAutoMapper(typeof(ProductProfile));
			builder.Services.AddAutoMapper(typeof(AdminProductProfile));

			_ = builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireNonAlphanumeric = false;
				options.User.RequireUniqueEmail = true;
				options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;


			})
				.AddEntityFrameworkStores<TagerlyDbContext>().
				AddDefaultTokenProviders();


			#region Add Roles
			async Task SeedRolesAsync(IServiceProvider serviceProvider)
			{
				var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				string[] roleNames = { "Seller", "Buyer", "Admin" };
				foreach (var roleName in roleNames)
				{
					if (!await roleManager.RoleExistsAsync(roleName))
					{
						await roleManager.CreateAsync(new IdentityRole(roleName));
					}
				}
			}
			#endregion

			#region Add Admin
			async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
			{
				var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				string adminEmail = "tagerly588@gmail.com";
				string adminPassword = "Admin@123";

				if (await userManager.FindByEmailAsync(adminEmail) == null)
				{
					var adminUser = new ApplicationUser
					{
						UserName = "AdminTagerly",
						Email = adminEmail,
						Address = "Admin Office",
						PhoneNumber = "123-456-7890",
						EmailConfirmed = true,
						IsActive=true
					};

					var result = await userManager.CreateAsync(adminUser, adminPassword);
					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(adminUser, "Admin");
					}
				}
			}
			#endregion

			builder.Services.AddLogging(logging =>
			{
				logging.AddConsole();
				logging.SetMinimumLevel(LogLevel.Debug);
			});

			var app = builder.Build();
			// Call the seeding methods
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				await SeedRolesAsync(services);
				await SeedAdminUserAsync(services);
			}

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			#region Custom routes
			app.MapControllerRoute(
			name: "sellerProducts",
			pattern: "Seller/Products/{action=Index}/{id?}",
			defaults: new { controller = "Product" });

			app.MapControllerRoute(
				name: "buyerProducts",
				pattern: "Products/{action=Index}/{id?}",
				defaults: new { controller = "Buyer" }); 
			#endregion

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}