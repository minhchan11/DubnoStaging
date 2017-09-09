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
using ReflectionIT.Mvc.Paging;
using System;
using System.IO;


namespace Dubno
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment environment)
        {

   

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<SampleData>();
            //makes this app a Model View Controller app
            services.AddMvc();

            services.AddPaging(options => {
                options.ViewName = "Bootstrap4";
                options.HtmlIndicatorDown = " <span>&darr;</span>";
                options.HtmlIndicatorUp = " <span>&uarr;</span>";
            });

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


            //tells app to use Identity on configuration
            app.UseIdentity();

            //tells app to use our wwwroot folder for css/js/images
            app.UseStaticFiles();

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseBrowserLink();

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


    }
}