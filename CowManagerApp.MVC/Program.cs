using CowManager.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using CowManager.S;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;
using CowManager.Models.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CowManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<CowManagerContext>();
builder.Services.ConfigureApplicationCookie(Option => { Option.LoginPath=$"/Identity/Account/Login";Option.LogoutPath=$"/Identity/Account/Logout"; Option.AccessDeniedPath = $"/Identity/Account/AccessDeniedPath"; });

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CostumerOnly", policy => policy.RequireRole("Costumer"));
});

var app = builder.Build();

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

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name:"Costumer",
    areaName:"Costumer",
    pattern: "Costumer/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
