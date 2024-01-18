namespace TunerAndMetronome.ViewModels;

public class MetronomeViewModel : ViewModelBase
{
    private int _bpm;
    private string _meterText;

    public MetronomeViewModel()
    {
        Bpm = 120;
        MeterText = "1,1,1,1/4";
    }

    public int Bpm
    {
        get => _bpm;
        set
        {
            if (value == _bpm || value < 10 || value > 400) return;
            _bpm = value;
            OnPropertyChanged();
        }
    }

    public string MeterText
    {
        get => _meterText;
        set
        {
            if (value == _meterText) return;
            _meterText = value;
            OnPropertyChanged();
        }
    }
}