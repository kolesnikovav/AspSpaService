using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AspSpaService
{
    /// <summary>
    /// Wraps a <see cref="StreamReader"/> to expose an evented API, issuing notifications
    /// when the stream emits lines.
    /// </summary>
    internal class NodeStreamReader: IDisposable
    {
        public delegate void OnReceivedLineHandler(string line);
        public event OnReceivedLineHandler OnReceivedLine;
        private readonly StreamReader _streamReader;
        private readonly Task _taskReading;
        public NodeStreamReader(StreamReader streamReader)
        {
            _streamReader = streamReader ?? throw new ArgumentNullException(nameof(streamReader));
            _taskReading = Task.Factory.StartNew(Run);
        }
        private async Task Run()
        {
            while (true)
            {
                var recivedLine = await _streamReader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(recivedLine))
                {
                    OnReceivedLine(recivedLine);
                }
            }
        }
        public void Dispose()
        {
            _streamReader.Dispose();
            _taskReading?.Dispose();
        }
    }
}