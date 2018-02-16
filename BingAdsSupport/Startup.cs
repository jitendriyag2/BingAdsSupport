using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BingAdsSupport.Data;
using BingAdsSupport.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BingAdsSupport
{
    public class Startup
    {
        IHostingEnvironment env;
        public Startup(IConfiguration configuration, IHostingEnvironment _env)
        {
            Configuration = configuration;
            env = _env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("BingAdConnection");

            if(env.IsDevelopment())
            {
                var pwd = Configuration["pwd"];
                var connectioString = connectionString.Insert(connectionString.Length - 1, $"; pwd={pwd} ; MultipleActiveResultSets = True;");
                connectionString = connectioString;
            }
            
            services.AddDbContext<AppDbContext>(options =>
                     options.UseSqlServer(connectionString));

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", template: "{controller=Ticket}/{action=AddFeedback}/{id?}");
            });
        }
    }
}
