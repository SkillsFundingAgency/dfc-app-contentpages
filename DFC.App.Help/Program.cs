using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DFC.App.Help
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();

            webHost.Run();
        }
    }
}
