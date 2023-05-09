using AuctionSite.Data;
// using AuctionSite.Services;
using AuctionSite.Areas.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using MudBlazor.Services;
using AuctionSite.Services;
using AuctionSite.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<ApplicationDbContext>();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsPrincipalFactory>();
builder.Services.AddSignalR();

builder.Services.AddSingleton<AuctionService>();
builder.Services.AddSingleton<BiddingService>();
builder.Services.AddSingleton<WatchingService>();

// MudBlazor
builder.Services.AddMudServices();

builder.Services.AddResponseCompression(options =>
{
	options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
		new[] { "application/octet-stream" }
	);
});

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapHub<BiddingHub>("/BiddingHub");
app.MapFallbackToPage("/_Host");

//using (var scope = app.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;

//	RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

//	await CreateRolesAsync(roleManager);
//	await CreateInitialUsersAsync(userManager);
//}

app.Run();

// 

async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
{
	try
	{
		if(!await roleManager.RoleExistsAsync(Roles.Admin))
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
		}

		if (!await roleManager.RoleExistsAsync(Roles.Student))
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.Student));
		}

	}
	catch (Exception ex)
	{
		throw new Exception(ex.Message);
	}
}

async Task CreateInitialUsersAsync(UserManager<ApplicationUser> userManager)
{
	try
	{
		string password = "Pa$$w0rd!"; // evil

		// Base Admin
		var adminUser = new ApplicationUser();
		adminUser.UserName = "admin@auction.site";
		adminUser.Email = adminUser.UserName;
		adminUser.FirstName = "Josh";
		adminUser.LastName = "Price";

		if(await userManager.FindByNameAsync(adminUser.UserName) == null)
		{
			var createResult = await userManager.CreateAsync(adminUser, password);

			if (createResult.Succeeded)
			{
				await userManager.AddToRoleAsync(adminUser, Roles.Admin);
				await userManager.SetLockoutEnabledAsync(adminUser, false);
			}
			else
			{
				throw new Exception(createResult.Errors.FirstOrDefault()?.ToString());
			}
		}

		// Test Student
		var studentUser = new ApplicationUser();
		studentUser.UserName = "student@auction.site";
		studentUser.Email = studentUser.UserName;
		studentUser.FirstName = "Scringlo";
		studentUser.LastName = "Beempis";

		if(await userManager.FindByNameAsync(studentUser.UserName) == null)
		{
			var createResult = await userManager.CreateAsync(studentUser, password);

			if (createResult.Succeeded)
			{
				await userManager.AddToRoleAsync(studentUser, Roles.Student);
				await userManager.SetLockoutEnabledAsync(studentUser, false);
			}
			else
			{
				throw new Exception(createResult.Errors.FirstOrDefault()?.ToString());
			}
		}

	}
	catch (Exception ex)
	{
		throw new Exception(ex.Message);
	}
}
