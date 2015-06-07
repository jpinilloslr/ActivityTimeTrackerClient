
namespace ActivityMonitor.Interfaces
{
    interface IActivityService
    {
        string GetWindowText(int hwnd);

        string GetProcessName(int hwnd);

        string GetExecutableFilename(int hwnd);
    }
}
