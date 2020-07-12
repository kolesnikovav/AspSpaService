# AspSpaService
The Asp Net Core plugin for integrating SPA application with Asp Net Core.
This plugin can be used with any web framework in same manner.

# Usage
Change your Startup.cs configuration file as follows:
```cs
using AspSpaService;
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ---- Your code -----------//
            //this block starts vue spa application
            var p = Path.Combine(wd,"samples", "hello-vue"); // path to your vuejs project
            app.UseSpa(
                spa => {
                    spa.UseAspSpaDevelopmentServer("yarn", "serve", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), null);
                }
            );
        }
    }

```

## Sample configuratons
In folder sample/webapi has the web empty project that shows, how to use your favorite web framework with asp:
### Vue
```cs
    public class Startup
    {
            //this block starts vue spa application
            var p = Path.Combine(wd,"samples", "hello-vue"); // path to your vuejs project
            app.UseSpa(
                spa => {
                    spa.UseAspSpaDevelopmentServer("yarn", "serve", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), null);
                }
            );
    }

```
### Vite
```cs
    public class Startup
    {
            //this block starts vite spa application
            var p = Path.Combine(wd,"samples", "hello-vite"); // path to your vitejs project
            app.UseSpa(
                spa => {
                    spa.UseAspSpaDevelopmentServer("yarn", "dev", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), null);
                }
            );
    }

```
### Nuxt
```cs
    public class Startup
    {
            //this block starts nuxt spa application
            var p = Path.Combine(wd,"samples", "hello-nuxt"); // path to your nuxt project
            app.UseSpa(
                spa => {
                    spa.UseAspSpaDevelopmentServer("yarn", "dev", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), null);
                }
            );
    }

```