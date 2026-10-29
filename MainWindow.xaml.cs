using NAudio.CoreAudioApi;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Youtube_Music_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _volumeRefreshTimer = new()
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        private int _webViewVolumePercent = 100;

        public MainWindow()
        {
            InitializeComponent();

            WebViewVolumeSlider.ValueChanged += WebViewVolumeSlider_ValueChanged;
            _volumeRefreshTimer.Tick += (_, _) => ApplyWebViewVolume();
            Closed += (_, _) => _volumeRefreshTimer.Stop();

            // Initialize WebView in the background
            _ = InitializeWebViewAsync();
        }

        async Task InitializeWebViewAsync()
        {
            try
            {
                string userDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "YoutubeMusicProxy");

                // Set up command line arguments to customize browser process
                var options = new Microsoft.Web.WebView2.Core.CoreWebView2EnvironmentOptions(
                    "--disable-features=AudioServiceOutOfProcess --autoplay-policy=no-user-gesture-required --no-service-autorun");

                var environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(
                    null,
                    userDataFolder,
                    options);

                await webView.EnsureCoreWebView2Async(environment);

                webView.Source = new Uri("https://music.youtube.com");

                ApplyWebViewVolume();
                _volumeRefreshTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing WebView: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WebViewVolumeSlider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            _webViewVolumePercent = (int)Math.Round(e.NewValue);
            WebViewVolumeLabel.Text = $"{_webViewVolumePercent}%";
            ApplyWebViewVolume();
        }

        private void ApplyWebViewVolume()
        {
            if (webView.CoreWebView2 is not { } coreWebView2)
            {
                return;
            }

            try
            {
                var processIds = coreWebView2.Environment.GetProcessInfos()
                    .Select(process => (int)process.ProcessId)
                    .Concat(new[] { (int)coreWebView2.BrowserProcessId })
                    .ToHashSet();

                using var deviceEnumerator = new MMDeviceEnumerator();

                foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(
                    DataFlow.Render,
                    DeviceState.Active))
                {
                    using (device)
                    {
                        var sessions = device.AudioSessionManager.Sessions;

                        for (var index = 0; index < sessions.Count; index++)
                        {
                            try
                            {
                                using var session = sessions[index];

                                try
                                {
                                    var processId = (int)session.GetProcessID;
                                    if (processIds.Contains(processId))
                                    {
                                        session.SimpleAudioVolume.Volume = _webViewVolumePercent / 100f;
                                    }
                                }
                                catch (COMException)
                                {
                                    // Process ID retrieval failed, continue to next session
                                }
                            }
                            catch (COMException)
                            {
                                // A session can disappear while audio sessions are being enumerated.
                            }
                        }
                    }
                }
            }
            catch (COMException)
            {
                // The WebView2 process or an audio device can become unavailable during enumeration.
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}