using System;
using AspSpaService;
using System.IO;

var wd = Directory.GetCurrentDirectory();
// this block starts vue spa application
var p = Path.Combine(wd, "samples", "hello-vue");
NodeRunner r = new()
{
    Command = "yarn",
    Arguments = "serve",
    WorkingDirectory = p,
    Timeout = TimeSpan.FromSeconds(10)
};
r.Launch(null);
if (r.Uri != null)
{
    Console.WriteLine("vue spa project is served " + r.Uri);
}
r.Dispose();

// this block starts vite spa application
var pVite = Path.Combine(wd, "samples", "hello-vite");
NodeRunner rVite = new()
{
    Command = "yarn",
    Arguments = "dev",
    WorkingDirectory = pVite,
    Timeout = TimeSpan.FromSeconds(10)
};
rVite.Launch(null);
if (rVite.Uri != null)
{
    Console.WriteLine("vite spa project is served " + rVite.Uri);
}
rVite.Dispose();

// this block starts nuxt spa application
var pNuxt = Path.Combine(wd, "samples", "hello-nuxt");
NodeRunner rNuxt = new()
{
    Command = "yarn",
    Arguments = "dev",
    WorkingDirectory = pNuxt,
    Timeout = TimeSpan.FromSeconds(10)
};
rNuxt.Launch(null);
if (rNuxt.Uri != null)
{
    Console.WriteLine("nuxt spa project is served " + rNuxt.Uri);
}
rNuxt.Dispose();

Console.WriteLine("Hello World!");
