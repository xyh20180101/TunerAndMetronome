using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using TunerAndMetronome.AudioPlayers;

namespace TunerAndMetronome.Desktop.AudioPlayers;

public class WindowsAudioPlayer : AudioPlayer
{
    private readonly MixingSampleProvider _mixingSampleProvider;
    private readonly WasapiOut _wasapiOut;

    public WindowsAudioPlayer()
    {
        _mixingSampleProvider = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            { ReadFully = true };
        _wasapiOut = new WasapiOut(AudioClientShareMode.Shared, 100);
        _wasapiOut.Init(_mixingSampleProvider);
        _wasapiOut.Play();
    }

    public override async Task Play(byte[] buffer)
    {
        _mixingSampleProvider.AddMixerInput(new RawSourceWaveStream(buffer, 0, buffer.Length,
            new WaveFormat(44100, 2)));
    }
}