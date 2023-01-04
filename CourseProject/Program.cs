using CourseProject_DataAccess;
using CourseProject_DataAccess.Repository;
using CourseProject_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Npgsql;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using CourseProject_Utility;
using System.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;

namespace CourseProject
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            string connectionString = BuildConnectionString(builder.Configuration);
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                                                                options.UseNpgsql(connectionString));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddDefaultTokenProviders().AddDefaultUI()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<ICompositionRepository, CompositionRepository>();

            builder.Services.AddAuthentication().AddFacebook(Options =>
            {
                Options.AppId = "3295360184015573";
                Options.AppSecret = "27410a25aedc707e0fd07704b5a1a3aa";
            });

            builder.Services.AddAuthentication().AddGoogle(Options =>
            {
                Options.ClientId = "607176074381-pi3vqr95dpmkk7n2inlgojasn1gef2ai.apps.googleusercontent.com";
                Options.ClientSecret = "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";
            });

            builder.Services.AddControllersWithViews()
                            .AddDataAnnotationsLocalization(option =>
                            {
                                var type = typeof(SharedResource);
                                var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName!);
                                var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                                var localizer = factory!.Create("SharedResource", assemblyName!.Name!);
                                option.DataAnnotationLocalizerProvider = (t, f) => localizer;
                            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("fr-FR")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            NpgsqlConnectionStringBuilder builder = new()
            {
                Username = configuration["PG_Username"],
                Password = configuration["PG_Password"],
                Host = configuration["PG_Server"],
                Database = configuration["PG_Database"],
                Port = configuration.GetValue<int>("PG_Port")
            };
            return builder.ConnectionString;
        }
    }
}