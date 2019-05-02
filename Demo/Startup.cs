using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            Action<InitialData> list = (opt =>
            {
                opt.InitialDataList = GetData();
            });
            services.Configure(list);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<InitialData>>().Value);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public List<DemoClass> GetData()
        {
            Random rand = new Random();
            List<DemoClass> initialDataList = new List<DemoClass>();
            for (int i = 0; i < 10; i++)
            {
                DemoClass demoClass = new DemoClass
                {
                    Id = i + 1,
                    Name = "Name_" + (i + 1),
                    ParentId = rand.Next(0, initialDataList.Count),
                    Details = "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum "
                };
                demoClass.ParentId = demoClass.ParentId == 0 ? null : demoClass.ParentId;
                initialDataList.Add(demoClass);
            }
            foreach (DemoClass item in initialDataList)
            {
                item.HasChildren = initialDataList.Any(x => x.ParentId == item.Id);
            }

            return initialDataList;
        }
    }
}
