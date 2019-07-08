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
                     /*
                     var appInsightsInstrumentationKey = webHostBuilderContext.Configuration["ApplicationInsights:InstrumentationKey"];

                     //We cannot capture errors until we set the key, so we want to set this early on so we can catch errors in startup.cs/program.cs
                     loggingBuilder.AddApplicationInsights(appInsightsInstrumentationKey);

                     // Adding the filter below to ensure logs of all severity from Program.cs is sent to ApplicationInsights.
                     loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(typeof(Program).FullName, LogLevel.Trace);

                     // Adding the filter below to ensure logs of all severity from Startup.cs is sent to ApplicationInsights.
                     loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(typeof(Startup).FullName, LogLevel.Trace);
                     */

                     //This filter is for app insights only
                     loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                 })
                .UseStartup<Startup>()
                .Build();

            webHost.Run();
        }
    }
}
