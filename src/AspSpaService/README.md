# AspSpaService

<a href="https://www.nuget.org/packages/AspSpaService">
    <img alt="Nuget (with prereleases)" src="https://img.shields.io/nuget/vpre/AspSpaService">
</a>
<a href="https://www.nuget.org/packages/AspSpaService">
    <img alt="Nuget" src="https://img.shields.io/nuget/dt/AspSpaService">
</a>

The Asp Net Core plugin for integrating SPA application with Asp Net Core.
Simplify SPA site development
This plugin can be used with any web framework in same manner.

# Usage
Install package via NuGet
```
dotnet add package AspSpaService
```
Install all the dependencies defined in Your SPA application
```
cd <SPA application path>
yarn // or npm install or pnpm install
```

Change your Startup.cs configuration file as follows:
```cs
using AspSpaService;
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // ---- Your code -----------//
            services.AddNodeRunner();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ---- Your code -----------//
            //this block starts vue spa application
            var wd = Directory.GetCurrentDirectory();
            var p = Path.Combine(wd,"samples", "hello-vite"); // path to your vuejs project
            app.UseSpa(
                spa => {
                    spa.UseAspSpaDevelopmentServer(
                        // command for nodejs process
                        // string
                        "yarn",
                        // argument for nodejs process
                        // string
                        "dev",
                        // working directory
                        // string
                        p,
                        // environment variables
                        new Dictionary<string,string>(),
                        // timeout for waiting node js process is ready to use
                         TimeSpan.FromSeconds(15),
                         // message when timeout has been exceeded
                         // has defaul value = "Timeout has been exceeded" (can be ommited!)
                         // string
                          "Timeout has been exceeded",
                          //logInformation for node js process
                          // bool (true by default)
                          true,
                          //logError for node js process
                          // some bundler emits many error strings during compilation
                          // bool (false by default)
                          false,
                          //unsubscribeWhenReady
                          // stop logging nodejs output when it ready to use
                          // bool (true by default)
                          true
                          );
                }
            );
        }
    }

```
This library starts NodeJS process, and waiting for it emits line with served valid Uri

## Sample configuratons
In folder sample/webapi has the web empty project that shows, how to use your favorite web framework with asp.
This code should be placed in Startup class in Configure method
### Vue
```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ---- Your code -----------//
    //this block starts vue spa application
    var wd = Directory.GetCurrentDirectory();
    var p = Path.Combine(wd,"samples", "hello-vue"); // path to your vuejs project
    app.UseSpa(
        spa => {
            spa.UseAspSpaDevelopmentServer("yarn", "serve", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), "Timeout has been exceeded");
        }
    );
}

```
### Vite
```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ---- Your code -----------//
    //this block starts vite spa application
    var wd = Directory.GetCurrentDirectory();
    var p = Path.Combine(wd,"samples", "hello-vite"); // path to your vitejs project
    app.UseSpa(
        spa => {
            spa.UseAspSpaDevelopmentServer("yarn", "dev", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), "Timeout has been exceeded");
        }
    );
}

```
### Nuxt
```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ---- Your code -----------//
    //this block starts nuxt spa application
    var wd = Directory.GetCurrentDirectory();
    var p = Path.Combine(wd,"samples", "hello-nuxt"); // path to your nuxt project
    app.UseSpa(
        spa => {
            spa.UseAspSpaDevelopmentServer("yarn", "dev", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), "Timeout has been exceeded");
        }
    );
}

```
### React
```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ---- Your code -----------//
    //this block starts react spa application
    var wd = Directory.GetCurrentDirectory();
    var p = Path.Combine(wd,"samples", "hello-react"); // path to your react project
    app.UseSpa(
        spa => {
            spa.UseAspSpaDevelopmentServer("yarn", "start", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), "Timeout has been exceeded");
        }
    );
}

```
### Svelte
Note. Because Svelte and AspNetCore, by default, use the same port (5000), the port of Svelte should be changed!!!
```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ---- Your code -----------//
    //this block starts svelte spa application
    var wd = Directory.GetCurrentDirectory();
    var p = Path.Combine(wd,"samples", "hello-svelte"); // path to your svelte project
    app.UseSpa(
        spa => {
            spa.UseAspSpaDevelopmentServer("yarn", "dev", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), "Timeout has been exceeded");
        }
    );
}

```
