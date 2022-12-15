using System.Diagnostics;

namespace HttpKeyboardMouse
{
    internal class TextFileTraceListener: TextWriterTraceListener
    {

        public TextFileTraceListener(string? filename) : base(filename)
        {
            // we just want to add the DateTime to the output without needing extra config file
        }

        public override void Write(string? message)
        {
            base.Write($"[{DateTime.Now}] {message}");
        }

        public override void WriteLine(string? message)
        {
            Write(message + Environment.NewLine);
        }
    }
}
