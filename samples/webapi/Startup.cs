using AspSpaService;

namespace webapi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNodeRunner();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var wd = Directory.GetCurrentDirectory();
            // this block starts vue spa application
            // var p = Path.Combine(wd,"samples", "hello-vue");
            // app.UseSpa(
            //     spa => {
            //         spa.UseAspSpaDevelopmentServer("yarn", "serve", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(10), "Timeout has been exceeded");
            //     }
            // );
            // this block starts vite spa application
            var p = Path.Combine(wd,"spa-app", "hello-vite");
            app.UseSpa(
                spa => {
                    spa.UseAspSpaDevelopmentServer(hostApplicationLifetime, "yarn", "dev", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), null, true,true);
                }
            );
            // this block starts nuxt spa application
            // var p = Path.Combine(wd,"samples", "hello-nuxt");
            // app.UseSpa(
            //     spa => {
            //         spa.UseAspSpaDevelopmentServer("pnpm", "run dev -o", p, new Dictionary<string,string>(), TimeSpan.FromSeconds(10), null);
            //     }
            // );
            // this block starts next-react spa application
            // var pReact = Path.Combine(wd,"samples", "hello-next-react");
            // app.UseSpa(
            //     spa => {
            //         spa.UseAspSpaDevelopmentServer("npm", "run dev", pReact, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), null);
            //     }
            // );
            // this block starts svelte spa application
            // var pSvelte = Path.Combine(wd,"samples", "hello-svelte");
            // app.UseSpa(
            //     spa => {
            //         spa.UseAspSpaDevelopmentServer("npm", "run dev", pSvelte, new Dictionary<string,string>(), TimeSpan.FromSeconds(15), null);
            //     }
            // );
        }
    }
}
