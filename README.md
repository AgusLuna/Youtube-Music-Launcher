# YouTube Music Launcher

A lightweight Windows desktop application that lets you use **YouTube Music outside your web browser**.

Built with C# and Microsoft Edge WebView2, this project loads YouTube Music in its own standalone application window, so you can listen to music without keeping a browser tab open.

## Features

- **Standalone window** — Run YouTube Music separately from your regular browser.
- **OBS-friendly** — Makes it easier to identify and capture the YouTube Music window separately in OBS Studio and other recording or streaming tools.
- **Separate browsing environment** — Keep your music separate from your everyday browser tabs and activity.
- **WebView2 powered** — Uses Microsoft's WebView2 technology to display YouTube Music.
- **Simple and lightweight** — Focused on one purpose: launching YouTube Music as a desktop app.

## Why Use It?

When YouTube Music runs inside a regular browser, capturing its audio or window separately can be inconvenient.

Running it in its own application window provides a convenient target for tools such as:

- **OBS Studio** — Capture the YouTube Music window separately from your main browser.
- **Screen recording software** — Select the music player without capturing your entire browser.
- **Streaming setups** — Keep your music player separate from other browser windows and applications.
- **Window management tools** — Identify, switch to, and manage YouTube Music as a distinct application window.

**Note:** The launcher provides a separate application window, but does not guarantee complete process or audio isolation. Audio capture depends on your recording software and Windows configuration.

## Download and Installation

1. Go to the [Releases](../../releases) page.
2. Download the latest release package for Windows.
3. Extract the package if it is provided as a ZIP archive.
4. Run `YouTubeMusicLauncher.exe`.

If Windows asks for permission to run the application, verify that you trust the downloaded release before proceeding.

### Requirements

- Windows
- Microsoft Edge WebView2 Runtime

The .NET runtime may also be required depending on how the release package is published.

## How It Works

The application uses WebView2 to load the YouTube Music website inside a dedicated desktop window. This allows you to access YouTube Music without opening it in your regular browser.

Your YouTube account, playback, and website features are handled by YouTube Music itself.

## Disclaimer

This is an unofficial, independent project and is not affiliated with, endorsed by, or sponsored by Google or YouTube.

YouTube Music is a trademark of Google LLC.

## License

See the [LICENSE](LICENSE) file for license information, if provided.
