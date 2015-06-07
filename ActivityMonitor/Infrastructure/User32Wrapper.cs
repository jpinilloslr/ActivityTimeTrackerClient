using System.Runtime.InteropServices;
using System.Text;

namespace ActivityMonitor.Infrastructure
{
    public class User32Wrapper
    {
        [DllImport("user32.dll")]
        public static extern int GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(int hWnd, out int processId);
    }
}
