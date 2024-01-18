using System.Threading.Tasks;
using NAudio.Wave;

namespace TunerAndMetronome.AudioRecorders;

public abstract class AudioRecorder
{
    public delegate void BufferUpdatedHandler(byte[] buffer, WaveFormat waveFormat);

    public bool IsRecording { get; protected set; }

    public abstract event BufferUpdatedHandler BufferUpdated;

    public abstract Task Start();

    public abstract Task Pause();
}