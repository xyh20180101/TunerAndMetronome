using System.Threading;
using System.Threading.Tasks;
using Android.Media;
using NAudio.Wave;
using TunerAndMetronome.AudioRecorders;
using Xamarin.Essentials;

namespace TunerAndMetronome.Android.AudioRecorders;

public class AndroidAudioRecorder : AudioRecorder
{
    private static AudioRecord _audioRecord;
    private CancellationTokenSource _cancellationTokenSource;

    public override event BufferUpdatedHandler BufferUpdated;

    public override async Task Start()
    {
        if (IsRecording) return;

        var status = await Permissions.RequestAsync<Permissions.Microphone>();
        if (status != PermissionStatus.Granted)
            return;

        _cancellationTokenSource = new CancellationTokenSource();

        if (_audioRecord == null)
            _audioRecord = new AudioRecord(AudioSource.Mic, 44100, ChannelIn.Stereo, Encoding.Pcm16bit, 32000);

        _audioRecord.StartRecording();
        Task.Run(async () =>
        {
            var minSize = AudioRecord.GetMinBufferSize(44100, ChannelIn.Stereo, Encoding.Pcm16bit);
            var size = 16000;
            var buffer = new byte[size < minSize ? minSize : size];
            while (_audioRecord.RecordingState == RecordState.Recording &&
                   !_cancellationTokenSource.IsCancellationRequested)
            {
                await _audioRecord.ReadAsync(buffer, 0, buffer.Length);
                BufferUpdated?.Invoke(buffer,
                    new WaveFormat(_audioRecord.Format.SampleRate,
                        _audioRecord.Format.FrameSizeInBytes * 8 / _audioRecord.Format.ChannelCount,
                        _audioRecord.Format.ChannelCount));
            }
        }, _cancellationTokenSource.Token);

        IsRecording = true;
    }

    public override async Task Pause()
    {
        if (!IsRecording) return;

        _audioRecord.Stop();
        _cancellationTokenSource.Cancel();

        IsRecording = false;
    }
}