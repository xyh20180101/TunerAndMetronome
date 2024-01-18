using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using TunerAndMetronome.AudioRecorders;
using TunerAndMetronome.Enums;
using TunerAndMetronome.ViewModels;

namespace TunerAndMetronome.Views;

public partial class Tuner : UserControl
{
    private static readonly SolidColorBrush GreenBrush = new(new Color(0xff, 0, 0xff, 0));

    private static readonly SolidColorBrush YellowBrush = new(new Color(0xff, 0xff, 0xb9, 0));
    private readonly AudioRecorder _audioRecorder;
    private readonly TunerViewModel _viewModel;

    public Tuner()
    {
        InitializeComponent();
        _audioRecorder = App.ServiceProvider.GetRequiredService<AudioRecorder>();

        _viewModel = new TunerViewModel();
        DataContext = _viewModel;

        // 添加控件动画
        GreenPoint.Transitions = new Transitions
        {
            new ThicknessTransition
            {
                Duration = TimeSpan.FromSeconds(0.5),
                Easing = new QuinticEaseOut(),
                Property = MarginProperty
            }
        };
        VolumeProgressBar.Transitions = new Transitions
        {
            new DoubleTransition
            {
                Duration = TimeSpan.FromSeconds(0.5),
                Easing = new QuinticEaseOut(),
                Property = RangeBase.ValueProperty
            }
        };

        // 处理事件队列
        Task.Run(async () =>
        {
            while (true)
            {
                var eventType = App.Current?.MainActivityEventQueue.Take();
                if (!IsLoaded) continue;
                switch (eventType)
                {
                    case MainActivityEventType.Pause:
                        await PauseRecord();
                        break;
                    case MainActivityEventType.Resume:
                        await StartRecord();
                        break;
                }
            }
        });
    }

    private async Task StartRecord()
    {
        if (!_audioRecorder.IsRecording)
        {
            _audioRecorder.BufferUpdated += UpdateTone;
            await _audioRecorder.Start();
        }
    }

    private async Task PauseRecord()
    {
        if (_audioRecorder.IsRecording)
        {
            _audioRecorder.BufferUpdated -= UpdateTone;
            await _audioRecorder.Pause();
        }
    }

    private async void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        await StartRecord();
    }

    private async void Control_OnUnloaded(object? sender, RoutedEventArgs e)
    {
        await PauseRecord();
    }

    private float[] ToFloatArray(byte[] bytes, WaveFormat waveFormat)
    {
        var dataLength = waveFormat.BitsPerSample / 8;
        var floatArray = new float[bytes.Length / dataLength / waveFormat.Channels];
        switch (dataLength)
        {
            case 1:
                for (var i = 0; i < bytes.Length; i += waveFormat.Channels * dataLength)
                    floatArray[i / dataLength / waveFormat.Channels] = bytes[i] / 128f;
                break;
            case 2:
                for (var i = 0; i < bytes.Length; i += waveFormat.Channels * dataLength)
                    floatArray[i / dataLength / waveFormat.Channels] = BitConverter.ToInt16(bytes, i) / 32768f;
                break;
            case 4:
                for (var i = 0; i < bytes.Length; i += waveFormat.Channels * dataLength)
                    floatArray[i / dataLength / waveFormat.Channels] = BitConverter.ToSingle(bytes, i);
                break;
        }

        return floatArray;
    }

    public void UpdateTone(byte[] bytes, WaveFormat waveFormat)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            if (bytes.Length == 0) return;

            var floats = ToFloatArray(bytes, waveFormat);

            for (var i = 0; i < floats.Length; i++)
            {
                floats[i] *= _viewModel.Amp;

                if (Math.Abs(floats[i]) < _viewModel.NoiseGate)
                    floats[i] = 0;
            }

            var max = floats.Select(Math.Abs).Max();
            _viewModel.Volume = max;

            var frequency = FrequencyDetector.DetectFrequency(floats, waveFormat.SampleRate,
                InterpolationCheckBox.IsChecked ?? false);
            var (noteData, delta, normalizeDelta, acceptable) = NoteData.GetNoteInformation(frequency);

            var isShow = noteData.NoteName != string.Empty && (HideTooHighOrTooLowCheckBox.IsChecked == false || noteData.Octave is >= 1 and <= 8);

            _viewModel.GreenPointMargin = new Thickness(isShow ? normalizeDelta * ScaleGrid.Bounds.Width * 2 / 3 : 0, 0, 0, 0);
            _viewModel.GreenPointFill = acceptable ? GreenBrush : YellowBrush;
            _viewModel.NoteName = isShow ? $"{noteData.NoteName}{noteData.Octave}" : string.Empty;
            _viewModel.StandardFrequency = isShow ? $"Standard: {noteData.Frequency} Hz" : string.Empty;
            _viewModel.Frequency = $"{frequency} Hz";

            _viewModel.RecordingParams = $@"SampleRate: {waveFormat.SampleRate}
BitDepth: {waveFormat.BitsPerSample}
Channels: {waveFormat.Channels}
FloatBufferLength: {floats.Length}";
        });
    }
}