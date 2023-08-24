using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AspSpaService;
/// <summary>
/// Wraps a <see cref="StreamReader"/> to expose an evented API, issuing notifications
/// when the stream emits lines.
/// </summary>
internal class NodeStreamReader : IDisposable
{
    public delegate void OnReceivedChunkHandler(ArraySegment<char> chunk);
    public delegate void OnReceivedLineHandler(string line);
    public delegate void OnStreamClosedHandler();
    public event OnReceivedLineHandler OnReceivedLine;
    #nullable enable
    public event OnReceivedChunkHandler? OnReceivedChunk;
    public event OnStreamClosedHandler? OnStreamClosed;
    #nullable disable
    private readonly StreamReader _streamReader;
    private readonly StringBuilder _linesBuffer;
    private readonly Task _taskReading;
    public NodeStreamReader(StreamReader streamReader)
    {
        _streamReader = streamReader ?? throw new ArgumentNullException(nameof(streamReader));
        _linesBuffer = new StringBuilder();
        _taskReading = Task.Factory.StartNew(Run);
    }
    private async Task Run()
    {
        var buf = new char[8 * 1024];
        while (true)
        {
            var chunkLength = await _streamReader.ReadAsync(buf, 0, buf.Length);
            if (chunkLength == 0)
            {
                if (_linesBuffer.Length > 0)
                {
                    OnCompleteLine(_linesBuffer.ToString());
                    _linesBuffer.Clear();
                }

                OnClosed();
                break;
            }

            OnChunk(new ArraySegment<char>(buf, 0, chunkLength));

            int lineBreakPos;
            var startPos = 0;

            // get all the newlines
            while ((lineBreakPos = Array.IndexOf(buf, '\n', startPos, chunkLength - startPos)) >= 0 && startPos < chunkLength)
            {
                var length = (lineBreakPos + 1) - startPos;
                _linesBuffer.Append(buf, startPos, length);
                OnCompleteLine(_linesBuffer.ToString());
                _linesBuffer.Clear();
                startPos = lineBreakPos + 1;
            }

            // get the rest
            if (lineBreakPos < 0 && startPos < chunkLength)
            {
                _linesBuffer.Append(buf, startPos, chunkLength - startPos);
            }
        }
    }
    private void OnChunk(ArraySegment<char> chunk)
    {
        var dlg = OnReceivedChunk;
        dlg?.Invoke(chunk);
    }

    private void OnCompleteLine(string line)
    {
        var dlg = OnReceivedLine;
        dlg?.Invoke(line);
    }

    private void OnClosed()
    {
        var dlg = OnStreamClosed;
        dlg?.Invoke();
    }
    public void Dispose()
    {
        _streamReader.Dispose();
        _taskReading?.Dispose();
    }
}