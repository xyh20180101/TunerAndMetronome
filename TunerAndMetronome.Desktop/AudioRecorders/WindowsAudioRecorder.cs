using System.Linq;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using TunerAndMetronome.AudioRecorders;

namespace TunerAndMetronome.Desktop.AudioRecorders;

public class WindowsAudioRecorder : AudioRecorder
{
    private readonly WasapiCapture _device;

    public WindowsAudioRecorder()
    {
        _device = new WasapiCapture();
        _device.DataAvailable += (s, a) =>
        {
            BufferUpdated?.Invoke(a.Buffer.Take(a.BytesRecorded).ToArray(), _device.WaveFormat);
        };
    }

    public override event BufferUpdatedHandler BufferUpdated;

    public override async Task Start()
    {
        if (!IsRecording)
            _device.StartRecording();
        IsRecording = true;
    }

    public override async Task Pause()
    {
        if (IsRecording)
            _device.StopRecording();
        IsRecording = false;
    }
}