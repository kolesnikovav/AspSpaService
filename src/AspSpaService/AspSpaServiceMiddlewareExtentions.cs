using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.SpaServices;

namespace AspSpaService
{
    /// <summary>
    /// Extension methods for enabling AspSpa development server middleware support.
    /// </summary>
    public static class AspSpaServiceMiddlewareExtensions
    {
        private const string LogCategoryName = "AspSpaService";
        /// <summary>
        /// Adds NodeRunner as singletone service and register it in Dependency Injection
        /// This dispose node js process when application is shutdown
        ///
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public static void AddNodeRunner(this IServiceCollection services)
        {
            services.AddSingleton(typeof(NodeRunner));
        }
        private static NodeRunner GetNodeRunner(IApplicationBuilder builder)
        {
            return (NodeRunner)builder.ApplicationServices.GetService(typeof(NodeRunner));
        }
        /// <summary>
        /// Handles requests by passing them through to an instance of the node dev server.
        /// This means you don't need to start node dev server manually.
        ///
        /// </summary>
        /// <param name="spaBuilder">The <see cref="ApplicationBuilder"/>.</param>
        /// <param name="command">The command or file name to start dev server.</param>
        /// <param name="arguments">Arguments to start dev server.</param>
        /// <param name="workingDirectory">WorkingDirectory for node dev  server</param>
        /// <param name="envVars">Environment variables for node dev  server</param>
        /// <param name="timeout">Timeout for node process waiting</param>
        /// <param name="timeoutExceedMessage">Message when timeout is exceeded</param>
        public static void UseAspSpaDevelopmentServer(
            this ISpaBuilder spaBuilder,
            string command,
            string arguments,
            string workingDirectory,
            Dictionary<string,string> envVars,
            TimeSpan timeout,
            string timeoutExceedMessage)
        {
            if (spaBuilder == null)
            {
                throw new ArgumentNullException(nameof(spaBuilder));
            }
            var logger = GetOrCreateLogger(spaBuilder.ApplicationBuilder, LogCategoryName);
            NodeRunner runner = GetNodeRunner(spaBuilder.ApplicationBuilder);
            if (runner == null)
            {
                runner = new NodeRunner();
            }
            runner.Command = command;
            runner.Arguments = arguments;
            runner.WorkingDirectory = workingDirectory;
            runner.EnvVars = envVars;
            runner.Timeout = timeout;
            runner.Launch(logger);
            if (runner.Uri != null)
            {
                spaBuilder.UseProxyToSpaDevelopmentServer(runner.Uri);
            }
       }
        private static ILogger GetOrCreateLogger(
            IApplicationBuilder appBuilder,
            string logCategoryName)
        {
            // If the DI system gives us a logger, use it. Otherwise, set up a default one
            var loggerFactory = appBuilder.ApplicationServices.GetService<ILoggerFactory>();
            var logger = loggerFactory != null
                ? loggerFactory.CreateLogger(logCategoryName)
                : NullLogger.Instance;
            return logger;
        }
    }
}
