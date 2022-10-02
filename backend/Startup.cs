using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using prid_tuto.Models;
using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace prid_tuto
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
            services.AddDbContext<MsnContext>(opt => opt.UseSqlite("data source=msn.db"));
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "prid_tuto", Version = "v1" }));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "frontend/dist";
            });

            //Mapper
            services.AddScoped(provider => new MapperConfiguration(cfg => {
                cfg.AddProfile(new MappingProfile(provider.GetService<MsnContext>()));
                // see: https://github.com/AutoMapper/AutoMapper.Collection
                cfg.AddCollectionMappers();
                cfg.UseEntityFrameworkCoreModel<MsnContext>(services);
            }).CreateMapper());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env , MsnContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "prid_tuto v1"));
            }

            if (context.Database.IsSqlite())
            {
                /*
                La suppression complète de la base de données n'est pas possible si celle-ci est ouverte par un autre programme,
                comme par exemple "DB Browser for SQLite" car les fichiers correspondants sont verrouillés.
                Pour contourner ce problème, on exécute cet ensemble de commandes qui vont supprimer tout le contenu de la DB.
                La dernière commande permet de réduire la taille du fichier au minimum.
                (voir https://stackoverflow.com/a/548297)
                */
                context.Database.ExecuteSqlRaw(
                    @"PRAGMA writable_schema = 1;
                    delete from sqlite_master where type in ('table', 'index', 'trigger', 'view');
                    PRAGMA writable_schema = 0;
                    VACUUM;");
            }
            else
            {
                context.Database.EnsureDeleted();        
            }
            context.Database.EnsureCreated();

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa => {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "frontend/dist";
                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
