using System;
using AspSpaService;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NodeRunner r = new NodeRunner();
            r.Command = "yarn";
            r.Arguments = "serve";
            r.WorkingDirectory = "hello-vue";
            r.Timeout = TimeSpan.FromSeconds(5);
            r.Launch(null);

            Console.WriteLine("Hello World!");
        }
    }
}
