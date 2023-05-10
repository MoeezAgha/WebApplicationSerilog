using Serilog;
using Serilog.Events;
using Serilog.Templates;

namespace WebApplicationSerilog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Configuration
            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
          // .Enrich.FromLogContext()
           .WriteTo.Console()
           .CreateBootstrapLogger();
            #endregion 



            #region Color using Temp
            //    Log.Logger = new LoggerConfiguration()
            //.WriteTo.Console(outputTemplate:
            //    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            //.CreateLogger();
            #endregion
            Log.Information("****************Hello, App", "Start*******************");

            var builder = WebApplication.CreateBuilder(args);

            #region serilog Setting
            // Full setup of serilog. We read log settings from appsettings.json
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());
            #endregion

            // Add services to the container.
            builder.Services.AddControllersWithViews();



            var app = builder.Build();

            #region We want to log all HTTP requests
            app.UseSerilogRequestLogging(configure =>
            {
                configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({Id}) responded {StatusCode} in {Elapsed:0.0000}ms";

                //configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({Id}) ({PowerApp}) responded {StatusCode} in {Elapsed:0.0000}ms";
            });
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}