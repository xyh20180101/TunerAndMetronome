using System.Threading.Tasks;
using Android.Media;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using TunerAndMetronome.AudioPlayers;

namespace TunerAndMetronome.Android.AudioPlayers;

public class AndroidAudioPlayer : AudioPlayer
{
    private readonly AudioTrack _audioTrack;
    private readonly MixingSampleProvider _mixingSampleProvider;

    public AndroidAudioPlayer()
    {
        _audioTrack = new AudioTrack.Builder()
            .SetAudioAttributes(new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Game)
                .SetContentType(AudioContentType.Sonification)
                .Build())
            .SetAudioFormat(new AudioFormat.Builder()
                .SetEncoding(Encoding.Pcm16bit)
                .SetSampleRate(44100)
                .SetChannelMask(ChannelOut.Stereo)
                .Build())
            .SetBufferSizeInBytes(1024)
            .SetPerformanceMode(AudioTrackPerformanceMode.LowLatency)
            .Build();
        _audioTrack.Play();

        _mixingSampleProvider = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        Task.Run(async () =>
        {
            var buffer = new byte[1024];
            var waveProvider = _mixingSampleProvider.ToWaveProvider16();
            while (true)
            {
                var count = waveProvider.Read(buffer, 0, buffer.Length);
                await _audioTrack.WriteAsync(buffer, 0, count);
            }
        });
    }

    public override async Task Play(byte[] buffer)
    {
        _mixingSampleProvider.AddMixerInput(new RawSourceWaveStream(buffer, 0, buffer.Length,
            new WaveFormat(44100, 2)));
    }
}