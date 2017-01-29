using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Yamac.LineMessagingApi.AspNetCore.Middleware;

namespace LinebotEcho
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure echo handler and options.
            services.AddSingleton<ILineMessagingRequestHandler, LinebotEchoHandler>();
            services.Configure<LineMessagingMiddlewareOptions>(Configuration.GetSection("Line"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Configure the webook of the LINE Messaging API.
            app.UseLineMessaging(new LineMessagingMiddlewareOptions
            {
                ChannelId = Configuration.GetSection("Line")["ChannelId"],
                ChannelSecret = Configuration.GetSection("Line")["ChannelSecret"],
                ChannelAccessToken = Configuration.GetSection("Line")["ChannelAccessToken"],
                WebhookPath = Configuration.GetSection("Line")["WebhookPath"],
            });
        }
    }
}
