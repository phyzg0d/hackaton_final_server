using CoreServer.Replication.Replication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using ServerAspNetCoreLinux.Core;

namespace ServerAspNetCoreLinux.ServerCore
{
    public class Startup
    {
        public HttpContext HttpContext;
        public ServerContext Context;
        public StartController StartController;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                allowsites => { allowsites.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()); });
            services.AddRouting();
            services.AddResponseCompression();
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            Context = new ServerContext();
            StartController = new StartController(Context, HttpContext);

            app.UseDefaultFiles();

            app.UseRouting();

            app.UseResponseCompression();

            app.UseCors(options => options.AllowAnyOrigin());    
            
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "0");
                if (context.Request.Path == "/game_request")
                {
                    if (!context.Request.HasFormContentType)
                    {
                        context.Response.Redirect("/");
                    }
                    else
                    {
                        Context.CommandModel.AddCommand(
                            Context.Factory.CommandFactory[context.Request.Form["Command"]](context.Request.Form, context));
                    }
                }
                else
                {
                    await next();
                }
            });
        }
    }
}