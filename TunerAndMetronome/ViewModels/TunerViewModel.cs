using Avalonia;
using Avalonia.Media;

namespace TunerAndMetronome.ViewModels;

public class TunerViewModel : ViewModelBase
{
    private float _amp;
    private string _frequency;
    private SolidColorBrush _greenPointFill;
    private Thickness _greenPointMargin;
    private string _noteName;
    private string _recordingParams;
    private string _standardFrequency;
    private float _volume;
    private float _noiseGate;

    public TunerViewModel()
    {
        Amp = 1;
        NoiseGate = 0.01f;
    }

    public string NoteName
    {
        get => _noteName;
        set
        {
            if (value == _noteName) return;
            _noteName = value;
            OnPropertyChanged();
        }
    }

    public Thickness GreenPointMargin
    {
        get => _greenPointMargin;
        set
        {
            if (value.Equals(_greenPointMargin)) return;
            _greenPointMargin = value;
            OnPropertyChanged();
        }
    }

    public SolidColorBrush GreenPointFill
    {
        get => _greenPointFill;
        set
        {
            if (Equals(value, _greenPointFill)) return;
            _greenPointFill = value;
            OnPropertyChanged();
        }
    }

    public string Frequency
    {
        get => _frequency;
        set
        {
            if (value == _frequency) return;
            _frequency = value;
            OnPropertyChanged();
        }
    }

    public string StandardFrequency
    {
        get => _standardFrequency;
        set
        {
            if (value == _standardFrequency) return;
            _standardFrequency = value;
            OnPropertyChanged();
        }
    }

    public string RecordingParams
    {
        get => _recordingParams;
        set
        {
            if (value == _recordingParams) return;
            _recordingParams = value;
            OnPropertyChanged();
        }
    }

    public float Volume
    {
        get => _volume;
        set
        {
            if (value.Equals(_volume)) return;
            _volume = value;
            OnPropertyChanged();
        }
    }

    public float Amp
    {
        get => _amp;
        set
        {
            if (value.Equals(_amp)) return;
            _amp = value;
            OnPropertyChanged();
        }
    }

    public float NoiseGate
    {
        get => _noiseGate;
        set
        {
            if (value.Equals(_noiseGate)) return;
            _noiseGate = value;
            OnPropertyChanged();
        }
    }
}