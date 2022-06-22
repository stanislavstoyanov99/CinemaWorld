namespace CinemaWorld.Web
{
    using System;
    using System.Reflection;

    using CinemaWorld.Data;
    using CinemaWorld.Data.Common;
    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Repositories;
    using CinemaWorld.Data.Seeding;
    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Services.Data;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;
    using CinemaWorld.Services.Messaging;
    using CinemaWorld.Web.Middlewares;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CinemaWorldDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = this.configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "CacheData";
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = new TimeSpan(0, 6, 0, 0);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddDefaultIdentity<CinemaWorldUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<CinemaWorldDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            // Secure for CSRF
            services.AddControllersWithViews(configure =>
            {
                configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddRazorPages();

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(
                serviceProvider => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IMoviesService, MoviesService>();
            services.AddTransient<IDirectorsService, DirectorsService>();
            services.AddTransient<IGenresService, GenresService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<ICinemasService, CinemasService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IContactsService, ContactsService>();
            services.AddTransient<IAboutService, AboutService>();
            services.AddTransient<IRatingsService, RatingsService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IHallsService, HallsService>();
            services.AddTransient<ISeatsService, SeatsService>();
            services.AddTransient<IMovieProjectionsService, MovieProjectionsService>();
            services.AddTransient<ITicketsService, TicketsService>();
            services.AddTransient<IPrivacyService, PrivacyService>();
            services.AddTransient<IMovieCommentsService, MovieCommentsService>();
            services.AddTransient<INewsCommentsService, NewsCommentsService>();

            // External login providers
            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = this.configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = this.configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.Fields.Add("name");
                });

            Account account = new Account(
                this.configuration["Cloudinary:AppName"],
                this.configuration["Cloudinary:AppKey"],
                this.configuration["Cloudinary:AppSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<CinemaWorldDbContext>();

                if (!env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseStatusCodePagesWithRedirects("/Home/HttpError?statusCode={0}"); // Midleware for missing page
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAdminMiddleware();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute(
                            "genreName",
                            "genre/{name:minlength(3)}",
                            new { controller = "Genres", action = "ByName" });
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Contacts}/{action=SuccessfullySend}/{userEmail?}");
                        endpoints.MapControllerRoute("subscription", "{controller=Home}/{action=ThankYouSubscription}/{email?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
