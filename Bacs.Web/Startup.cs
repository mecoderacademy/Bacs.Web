using Bacs.Data;
using Bacs.Data.Repository;
using Bacs.Models;
using Bacs.Services;
using Bacs.Services.Interfaces;
using Bacs.Services.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bacs.Web
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
            SqliteConnection sqliteConnection;
            sqliteConnection = new SqliteConnection("Filename=:memory:");
            sqliteConnection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var _contextOptions = new DbContextOptionsBuilder<Data.BacsContext>();
            services.AddScoped<DbContext,BacsContext>();
            services.AddScoped<BacsContext,BacsContext>();

            services.AddScoped(typeof(GenericRepository<>));
            services.AddScoped(typeof(FileTransactionRepository));
            services.AddScoped(typeof(TransactionRepository));

            services.AddScoped(typeof(ITransactionService),typeof(TransactionService));
            services.AddScoped(typeof(IFileTransactionService),typeof(FileTransactionService));
            services.AddScoped(typeof(IFileProccessor),typeof(FileProccessor));
            services.AddDbContext<BacsContext>(options =>
                options.UseSqlite(sqliteConnection));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bacs.Web", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bacs.Web v1"));
            }
            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                                                        //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
                    .AllowCredentials());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
