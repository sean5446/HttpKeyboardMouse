﻿using System.Diagnostics;

namespace HttpKeyboardMouse
{
    internal class TextBoxTraceListener : TraceListener
    {
        private readonly TextBoxBase _output;

        public TextBoxTraceListener(TextBoxBase output)
        {
            Name = "Trace";
            _output = output;
        }

        public override void Write(string? message)
        {
            void append()
            {
                _output.AppendText($"[{DateTime.Now}] {message}");
            }

            if (_output.InvokeRequired)
            {
                _output.BeginInvoke(append);
            }
            else
            {
                append();
            }
        }

        public override void WriteLine(string? message)
        {
            Write(message + Environment.NewLine);
        }
    }
}
