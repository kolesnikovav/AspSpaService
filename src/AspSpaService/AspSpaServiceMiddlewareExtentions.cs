using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace AspSpaService
{
    /// <summary>
    /// Extension methods for enabling AspSpa development server middleware support.
    /// </summary>
    public static class AspSpaServiceMiddlewareExtensions
    {
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
            this IApplicationBuilder spaBuilder,
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
       }
    }
}
