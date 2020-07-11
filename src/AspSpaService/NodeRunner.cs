using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Extensions.Logging;

namespace AspSpaService
{
    /// <summary>
    /// Node JS Process starter
    /// </summary>
    public class NodeRunner: IDisposable
    {
        private Process _nodeProcess;
        private Uri _uri;
        private Regex _regexUri = new Regex(@"http:\/\/localhost:[0-9]+");
        private EventWaitHandle _awaiter = new EventWaitHandle(false, EventResetMode.AutoReset);
        private NodeStreamReader streamOutputReader;
        private NodeStreamReader streamErrorReader;
    /// <summary>
    /// Message when timeout exceed
    /// </summary>
        public string TimeoutExceedMessage {get;set;}
    /// <summary>
    /// Timeout for wait node process response
    /// </summary>
        public TimeSpan Timeout {get;set;}
    /// <summary>
    /// Command or filename to be executed to launch node process
    /// </summary>
        public string Command {get;set;}
    /// <summary>
    /// Arguments for node process
    /// </summary>
        public string Arguments {get;set;}
    /// <summary>
    /// Working directory for node process
    /// </summary>
        public string WorkingDirectory {get;set;}
    /// <summary>
    /// Environment variables for node process
    /// </summary>
        public Dictionary<string,string> EnvVars {get;set;} = new Dictionary<string, string>();
    /// <summary>
    /// Indicates where node process is being served
    /// </summary>
        public Uri Uri {
            get => this._uri;
        }

        private ProcessStartInfo GetProcessStartInfo()
        {
            ProcessStartInfo p = new ProcessStartInfo();
            p.Arguments = this.Arguments;
            p.UseShellExecute = false;
            p.WorkingDirectory = this.WorkingDirectory;
            p.FileName = this.Command;
            p.RedirectStandardInput = true;
            p.RedirectStandardOutput = true;
            p.RedirectStandardError = true;
            foreach (var keyValuePair in this.EnvVars)
            {
                p.Environment[keyValuePair.Key] = keyValuePair.Value;
            }
            return p;
        }
        private void onResiveLineResult(string line)
        {
            var u = this._regexUri.Match(line);
            if (u.Success)
            {
                this._uri = new Uri(u.Value);
                this._awaiter.Set();
            }
        }
    /// <summary>
    /// Launch node process and wait untill it emits line with URL or timeout exceeds
    /// </summary>
        public void Launch(ILogger logger)
        {
            this._uri = null;
            var p = this.GetProcessStartInfo();
            this._awaiter.Reset();
            try
            {
                this._nodeProcess = Process.Start(p);
                this.streamOutputReader = new NodeStreamReader(this._nodeProcess.StandardOutput);
                this.streamErrorReader = new NodeStreamReader(this._nodeProcess.StandardError);
                if (logger != null)
                {
                    this.AttachToLogger(logger);
                }
                this.streamOutputReader.OnReceivedLine += this.onResiveLineResult;
                this.streamErrorReader.OnReceivedLine += this.onResiveLineResult;
                this._awaiter.WaitOne(this.Timeout);
            }
            catch(Exception ex)
            {
                var message = $"Failed to start '{Command} {Arguments}'";
                throw new InvalidOperationException(message, ex);
            }
        }
        /// <summary>
        /// Attach to logger.
        /// </summary>
        /// <param name="logger">The logger to attach </param>
        private void AttachToLogger(ILogger logger)
        {
            // When the node task emits complete lines, pass them through to the real logger
            this.streamOutputReader.OnReceivedLine += line =>
            {
                logger.LogInformation(line);
            };

            this.streamErrorReader.OnReceivedLine += line =>
            {
                logger.LogError(line);
            };
        }
        /// <summary>
        /// Stop node process and disposes resources
        /// </summary>
        public void Dispose()
        {
            if (_nodeProcess != null && !_nodeProcess.HasExited)
            {
                this._nodeProcess.Kill(true);
                this._nodeProcess = null;
            }
            this.streamOutputReader.Dispose();
            this.streamErrorReader.Dispose();
        }
    }
}
