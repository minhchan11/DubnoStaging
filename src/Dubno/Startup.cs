using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Dubno.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Dubno
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<SampleData>();
            //makes this app a Model View Controller app
            services.AddMvc();
            //add entityframework to the app, using an extension of DB context that connects to our own DubnoDbContext
            services.AddEntityFramework()
               .AddDbContext<DubnoDbContext>(options =>
                   options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            //adds Identity to app which makes log in secure
            services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<DubnoDbContext>()
             .AddDefaultTokenProviders();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SampleData seeder)
        {

            seeder.SeedAdminUser();

            //seeds our database with posts on startup

            var context = app.ApplicationServices.GetService<DubnoDbContext>();
            AddTestData(context);

            //tells app to use Identity on configuration
            app.UseIdentity();

            //tells app to use our wwwroot folder for css/js/images
            app.UseStaticFiles();

            loggerFactory.AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    //will determine where the app directs to first
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
            app.Run(async (error) =>
            {
                await error.Response.WriteAsync("You should not see this message. An error has occured.");
            });
        }


        //our method for seeding the database

        private static void AddTestData(DubnoDbContext context)
        {
            //context.Database.ExecuteSqlCommand("Delete From Posts");


            context.SaveChanges();
        }
    }
}