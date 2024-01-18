using Android.App;
using Android.Content.PM;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Microsoft.Extensions.DependencyInjection;
using TunerAndMetronome.Android.AudioPlayers;
using TunerAndMetronome.Android.AudioRecorders;
using TunerAndMetronome.AudioPlayers;
using TunerAndMetronome.AudioRecorders;
using TunerAndMetronome.Enums;
using Xamarin.Essentials;

namespace TunerAndMetronome.Android;

[Activity(
    Label = "TunerAndMetronome.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        Platform.Init(this, new Bundle());

        var services = new ServiceCollection();
        services.AddSingleton<AudioPlayer, AndroidAudioPlayer>();
        services.AddSingleton<AudioRecorder, AndroidAudioRecorder>();
        App.ServiceProvider = services.BuildServiceProvider();

        return base.CustomizeAppBuilder(builder).WithInterFont();
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnPause()
    {
        App.Current?.MainActivityEventQueue.Add(MainActivityEventType.Pause);
        base.OnPause();
    }

    protected override void OnResume()
    {
        App.Current?.MainActivityEventQueue.Add(MainActivityEventType.Resume);
        base.OnResume();
    }
}