using System;
using AspSpaService;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var wd = Directory.GetCurrentDirectory();
            // this block starts vue spa application
            var p = Path.Combine(wd,"samples", "hello-vue");
            NodeRunner r = new NodeRunner();
            r.Command = "yarn";
            r.Arguments = "serve";
            r.WorkingDirectory = p;
            r.Timeout = TimeSpan.FromSeconds(10);
            r.Launch(null);
            if (r.Uri != null) {
                Console.WriteLine("vue spa project is served " + r.Uri);
            }
            r.Dispose();

            // this block starts vite spa application
            var pVite = Path.Combine(wd,"samples", "hello-vite");
            NodeRunner rVite = new NodeRunner();
            rVite.Command = "yarn";
            rVite.Arguments = "dev";
            rVite.WorkingDirectory = pVite;
            rVite.Timeout = TimeSpan.FromSeconds(10);
            rVite.Launch(null);
            if (rVite.Uri != null) {
                Console.WriteLine("vite spa project is served " + rVite.Uri);
            }
            rVite.Dispose();

            // this block starts nuxt spa application
            var pNuxt = Path.Combine(wd,"samples", "hello-nuxt");
            NodeRunner rNuxt = new NodeRunner();
            rNuxt.Command = "yarn";
            rNuxt.Arguments = "dev";
            rNuxt.WorkingDirectory = pNuxt;
            rNuxt.Timeout = TimeSpan.FromSeconds(10);
            rNuxt.Launch(null);
            if (rNuxt.Uri != null) {
                Console.WriteLine("nuxt spa project is served " + rNuxt.Uri);
            }
            rNuxt.Dispose();

            Console.WriteLine("Hello World!");
        }
    }
}
