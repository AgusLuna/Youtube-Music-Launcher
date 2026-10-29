using System.Runtime.InteropServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Diagnostics;
using System.Threading;

namespace Youtube_Music_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Mutex _instanceMutex = new Mutex(true, "Youtube.Music.Launcher.SingleInstance");

        [DllImport("shell32.dll")]
        static extern int SetCurrentProcessExplicitAppUserModelID(string AppID);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;
        private const int SW_SHOW = 5;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!_instanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                // Another instance is already running
                BringExistingInstanceToFront();
                Shutdown();
                return;
            }

            SetCurrentProcessExplicitAppUserModelID("Youtube.Music.Launcher");
            base.OnStartup(e);
        }

        private static void BringExistingInstanceToFront()
        {
            // Find the existing process
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

            foreach (Process process in processes)
            {
                if (process.Id != currentProcess.Id)
                {
                    IntPtr hWnd = process.MainWindowHandle;
                    if (hWnd != IntPtr.Zero)
                    {
                        // Restore the window if it's minimized
                        ShowWindow(hWnd, SW_RESTORE);
                        // Bring it to the foreground
                        SetForegroundWindow(hWnd);
                    }
                    break;
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _instanceMutex?.ReleaseMutex();
            _instanceMutex?.Dispose();
            base.OnExit(e);
        }
    }
}
