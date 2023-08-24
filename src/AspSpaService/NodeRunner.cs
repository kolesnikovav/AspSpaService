using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
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
        private readonly Regex _regexUri = new (@"(http|https):\/\/(localhost|127\.0\.0\.1):[0-9]+");
        private readonly EventWaitHandle _awaiter = new (false, EventResetMode.AutoReset);
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
        /// Log Node JS Process messages
        /// </summary>
        public bool LogResult { get; set; } = true;
        /// <summary>
        /// Log Node JS Process error messages
        /// </summary>
        public bool LogError { get; set; } = false;
        /// <summary>
        /// Indicates where node process is being served
        /// </summary>
        public Uri Uri {
            get => _uri;
        }

        private System.Security.SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new System.Security.SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        private ProcessStartInfo GetProcessStartInfo()
        {
            var exeCmd = OperatingSystem.IsWindows() ? "cmd" : Command;

            var argumentsLaunch = OperatingSystem.IsWindows() ? $"/c {Command} {Arguments}" : Arguments;
            ProcessStartInfo p;
            if (OperatingSystem.IsWindows())
            {
                p = new(exeCmd)
                {
                    Arguments = argumentsLaunch,
                    UseShellExecute = false,
                    WorkingDirectory = WorkingDirectory,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Domain = Environment.UserDomainName,
                    UserName = Environment.UserName,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
            }
            else
            {
                p = new(exeCmd)
                {
                    Arguments = argumentsLaunch,
                    UseShellExecute = false,
                    WorkingDirectory = WorkingDirectory,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
            }

            if (EnvVars != null)
            {
                foreach (var keyValuePair in EnvVars)
                {
                    p.Environment[keyValuePair.Key] = keyValuePair.Value;
                }
            }
            return p;
        }
        private void OnResiveLineResult(string line)
        {
            var u = _regexUri.Match(line);
            if (u.Success)
            {
                _uri = new Uri(u.Value);
                _awaiter.Set();
            }
        }
    /// <summary>
    /// Launch node process and wait untill it emits line with URL or timeout exceeds
    /// </summary>
        public void Launch(ILogger logger)
        {
            _uri = null;
            var p = GetProcessStartInfo();
            _awaiter.Reset();
            try
            {
                _nodeProcess = Process.Start(p);
                _nodeProcess.EnableRaisingEvents = true;
                streamOutputReader = new NodeStreamReader(_nodeProcess.StandardOutput);
                streamErrorReader = new NodeStreamReader(_nodeProcess.StandardError);
                streamOutputReader.OnReceivedLine += OnResiveLineResult;
                if (LogError)
                {
                    streamErrorReader.OnReceivedLine += OnResiveLineResult;
                }
                _nodeProcess.Exited += (a, b) =>
                {
                    logger?.LogError("Node JS Process has been exited with code " + _nodeProcess.ExitCode.ToString());
                };
                var cStart = DateTime.Now;
                _awaiter.WaitOne(Timeout);
                var cExit = DateTime.Now;
                bool timeoutHasBeenExceeded = TimeSpan.Compare(cExit - cStart, Timeout) > 0;
                if (Uri == null)
                {
                    //unsubscribe events
                    streamOutputReader.OnReceivedLine -= OnResiveLineResult;
                    if (LogError)
                    {
                        streamOutputReader.OnReceivedLine -= OnResiveLineResult;
                    }
                    if (_nodeProcess != null)
                    {
                        logger.LogError("Disposing Node JS Process");
                        if (_nodeProcess != null && !_nodeProcess.HasExited)
                        {
                            _nodeProcess.Kill(true);
                            _nodeProcess = null;
                        }
                        _uri = null;
                        logger.LogError("Disposing Node JS Process has been disposed");
                    }
                    if (logger != null)
                    {
                        if (timeoutHasBeenExceeded)
                        {
                            logger.LogError(TimeoutExceedMessage + Timeout.ToString());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                var message = $"Failed to start '{Command} {Arguments}'";
                throw new InvalidOperationException(message, ex);
            }
        }
    /// <summary>
    /// Stop logging node process output
    /// </summary>
        public void UnsubscribeLog(ILogger logger)
        {
            streamOutputReader.OnReceivedLine += OnResiveLineResult;
            if (LogError)
            {
                streamErrorReader.OnReceivedLine += OnResiveLineResult;
            }
        }
        /// <summary>
        /// Stop node process and disposes resources
        /// </summary>
        public void Dispose()
        {
            if (_nodeProcess != null && !_nodeProcess.HasExited)
            {
                _nodeProcess.Kill(true);
                _nodeProcess = null;
            }
            streamOutputReader.Dispose();
            streamErrorReader.Dispose();
        }
    }
}
