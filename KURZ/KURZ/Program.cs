using KURZ.Models;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<KurzContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KurzContext")));



builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddScoped<IRolesModel, RolesModel>();
builder.Services.AddScoped<IUsersModel, UsersModel>();
builder.Services.AddScoped<ITopicsModel, TopicsModel>();
builder.Services.AddScoped<ITeacherModel, TeacherModel>();
builder.Services.AddScoped<IStudentModel, StudentModel>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.ReturnUrlParameter = "returnUrl";
                    options.LoginPath = "/Authentication/Login"; //Login es el nombre del controlador
                });

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

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
