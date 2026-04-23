using BusinessLayer.Mapping;
using DataAccessLayer;
using BusinessLayer;
using DataAccessLayer.Concrete.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using BusinessLayer.ValidationRules.Teacher;
using PathojaPilatesProject.ValidationRules.Teacher;
using PathojaPilatesProject.ValidationRules.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using CoreLayer.Utilities.Mail;
using PathojaPilatesProject.Services.Mail;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDataAccessDependencies();
builder.Services.AddBusinessLayerDependencies();
builder.Services.AddAutoMapper(typeof(BusinessLayer.Mapping.MappingProfile),
                               typeof(PathojaPilatesProject.Mapping.UiMappingProfile)); //Assembly => Tüm Profile sınıflarını yükler.
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<TeacherAddDtoValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<TeacherAddVMValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<TeacherUpdateDtoValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<TeacherUpdateVMValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<SignUpVMValidator>();
        fv.DisableDataAnnotationsValidation = true; // ⚡ DataAnnotations ile çakışmayı engeller
    });

builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IMailService, SmtpMailService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Login/Index";          // yetkisiz yakalanırsa buraya
        opt.AccessDeniedPath = "/Login/Denied";  // yetkisi yoksa buraya
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS
        opt.SlidingExpiration = true;
        opt.ExpireTimeSpan = TimeSpan.FromDays(14);
    });

builder.Services.AddMemoryCache();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
