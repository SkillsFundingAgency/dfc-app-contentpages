using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace DFC.App.Help
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                 .ConfigureLogging((webHostBuilderContext, loggingBuilder) =>
                 {
                     loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                 })
                .UseStartup<Startup>()
                .Build();

            webHost.Run();
        }
    }
}
