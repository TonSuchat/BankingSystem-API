using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Entity.DBModels;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Logger;
using Service;
using Service.Interfaces;
using Service.Services;

namespace API
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
            // init log4net configuration
            Log.Init();

            // add db context dependency
            services.AddDbContext<BankingSystemContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // add dependency injections
            AddServicesScope(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BankingSystemContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // create db in case not exist, Also seed IBAN master data
            DbInitializer.Initialize(dbContext);

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AddServicesScope(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
        }
        
    }
}
