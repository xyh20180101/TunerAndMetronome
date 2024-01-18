using System.Threading.Tasks;

namespace TunerAndMetronome.AudioPlayers;

public abstract class AudioPlayer
{
    public abstract Task Play(byte[] buffer);
}