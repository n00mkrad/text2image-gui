using System.Diagnostics;
using System.IO;

namespace StableDiffusionGui.Io
{
    public class NmkdStreamWriter
    {
        private StreamWriter _writer;
        public StreamWriter Writer { get { return _writer; } }

        private Process _process;
        public Process Process { get { return _process; } }

        public bool IsRunning { get { return _process != null && !_process.HasExited; } }

        public NmkdStreamWriter (StreamWriter writer, Process associatedProcess) 
        {
            _writer = writer;
            _process = associatedProcess;
        }

        public NmkdStreamWriter(Process associatedProcess)
        {
            _writer = associatedProcess.StandardInput;
            _process = associatedProcess;
        }
    }
}
