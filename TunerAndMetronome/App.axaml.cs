using System;
using System.Collections.Concurrent;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TunerAndMetronome.Enums;
using TunerAndMetronome.Views;

namespace TunerAndMetronome;

public class App : Application
{
    public readonly BlockingCollection<MainActivityEventType> MainActivityEventQueue = new();
    public new static App? Current => (App?)Application.Current;

    public static IServiceProvider ServiceProvider { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = new MainView();

        base.OnFrameworkInitializationCompleted();
    }
}