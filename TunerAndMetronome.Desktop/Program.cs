using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using TunerAndMetronome.AudioPlayers;
using TunerAndMetronome.AudioRecorders;
using TunerAndMetronome.Desktop.AudioPlayers;
using TunerAndMetronome.Desktop.AudioRecorders;

namespace TunerAndMetronome.Desktop;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        var services = new ServiceCollection();
        services.AddSingleton<AudioPlayer, WindowsAudioPlayer>();
        services.AddSingleton<AudioRecorder, WindowsAudioRecorder>();
        App.ServiceProvider = services.BuildServiceProvider();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}