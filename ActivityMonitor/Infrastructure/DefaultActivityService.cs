using System.Diagnostics;
using System.Text;
using ActivityMonitor.Interfaces;

namespace ActivityMonitor.Infrastructure
{
    class DefaultActivityService : IActivityService
    {

        public string GetWindowText(int hwnd)
        {
            var result = "";
            const int nChars = 256;
            var buffer = new StringBuilder(nChars);

            if (User32Wrapper.GetWindowText(hwnd, buffer, nChars) > 0)
            {
                result = buffer.ToString();
            }
            return result;
        }

        public string GetProcessName(int hwnd)
        {
            int pid;
            User32Wrapper.GetWindowThreadProcessId(hwnd, out pid);
            var process = Process.GetProcessById(pid);
            return process.ProcessName;
        }

        public string GetExecutableFilename(int hwnd)
        {
            var result = "";
            int pid;
            User32Wrapper.GetWindowThreadProcessId(hwnd, out pid);
            var process = Process.GetProcessById(pid);

            try
            {
                result = process.MainModule.FileName;
            }
            catch { }

            return result;
        }

    }
}
