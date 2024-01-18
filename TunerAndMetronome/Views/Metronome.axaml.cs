using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using TunerAndMetronome.AudioPlayers;
using TunerAndMetronome.ViewModels;

namespace TunerAndMetronome.Views;

public partial class Metronome : UserControl
{
    private readonly byte[] _sample1;
    private readonly byte[] _sample2;
    private readonly AudioPlayer _audioPlayer;

    private readonly List<ToggleButton> _meterButtonList;
    private readonly MetronomeViewModel _viewModel;

    public Metronome()
    {
        InitializeComponent();
        _audioPlayer = App.ServiceProvider.GetRequiredService<AudioPlayer>();

        _viewModel = new MetronomeViewModel();
        DataContext = _viewModel;

        var wav1 = new WaveFileReader(AssetLoader.Open(new Uri("avares://TunerAndMetronome/Assets/01.wav")));
        _sample1 = new byte[wav1.Length];
        wav1.Read(_sample1, 0, (int)wav1.Length);
        wav1.Dispose();

        var wav2 = new WaveFileReader(AssetLoader.Open(new Uri("avares://TunerAndMetronome/Assets/02.wav")));
        _sample2 = new byte[wav2.Length];
        wav2.Read(_sample2, 0, (int)wav2.Length);
        wav2.Dispose();

        _meterButtonList = new List<ToggleButton>
        {
            MeterButton1, MeterButton2, MeterButton3, MeterButton4, MeterButton5, MeterButton6, MeterButton7,
            MeterButton8, MeterButton9, MeterButton10, MeterButton11, MeterButton12
        };

        IsPlaying = false;
    }

    #region Timer Method

    private (int[] Intervals, int BeatUnit) Meter = (new[] { 1, 1, 1, 1 }, 4);

    private readonly Timer _timer = new();
    private int _beatCount;
    private int _subBeatIndex;
    private int _subBeatCount;

    private void ResetTime()
    {
        _beatCount = 0;
        _subBeatIndex = 0;
        _subBeatCount = 0;
    }

    private void Play()
    {
        _timer.Stop();
        ResetTime();
        _timer.Interval = 4d * 60000d / _viewModel.Bpm / Meter.BeatUnit;
        _timer.Elapsed += OnTimerOnElapsed;
        _timer.Start();
        OnTimerOnElapsed(null, null);
    }

    private void Stop()
    {
        _timer.Elapsed -= OnTimerOnElapsed;
        _timer.Stop();
    }

    private void OnTimerOnElapsed(object? o, ElapsedEventArgs elapsedEventArgs)
    {
        if (_beatCount == 0)
        {
            _audioPlayer.Play(_sample1);
            _subBeatIndex = 0;
            _subBeatCount = 0;
        }
        else if (_subBeatCount == Meter.Intervals[_subBeatIndex])
        {
            _audioPlayer.Play(_sample2);
            _subBeatCount = 0;
            _subBeatIndex++;
        }

        _subBeatCount++;
        _beatCount = (_beatCount + 1) % Meter.Intervals.Sum();
    }

    #endregion

    #region Play Control

    private bool _isPlaying;

    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            if (value)
            {
                // start play
                Play();
                PlayButton.Content = this.FindResource("StopIcon");
            }
            else
            {
                // stop play
                Stop();
                PlayButton.Content = this.FindResource("PlayIcon");
            }

            _isPlaying = value;
        }
    }

    private async void PlayButton_OnClick(object? sender, RoutedEventArgs e)
    {
        IsPlaying = !IsPlaying;
    }

    private async void Control_OnUnloaded(object? sender, RoutedEventArgs e)
    {
        IsPlaying = false;
    }

    #endregion

    #region Bpm Setting

    private void MinusFiveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _viewModel.Bpm -= 5;
    }

    private void MinusOneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _viewModel.Bpm -= 1;
    }

    private void PlusOneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _viewModel.Bpm += 1;
    }

    private void PlusFiveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _viewModel.Bpm += 5;
    }

    private void BpmSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        _timer.Interval = 4d * 60000d / e.NewValue / Meter.BeatUnit;
        ResetTime();
    }

    #endregion

    #region Meter Setting

    private void MeterButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // 这里获取到的IsChecked已经切换后的值了
        if (((ToggleButton)sender).IsChecked == false)
        {
            ((ToggleButton)sender).IsChecked = true;
            return;
        }

        foreach (var meterButton in _meterButtonList) meterButton.IsChecked = false;
        ApplyButton.IsChecked = false;

        ((ToggleButton)sender).IsChecked = true;
        var meterButtonText = (string)((ToggleButton)sender).Content;
        Meter = MeterButtonTextToMeter(meterButtonText);

        if (IsPlaying)
        {
            Stop();
            Play();
        }

        (int[] Intervals, int BeatUnit) MeterButtonTextToMeter(string meterButtonText)
        {
            switch (meterButtonText)
            {
                case "1/2": return (new[] { 1 }, 2);
                case "2/2": return (new[] { 1, 1 }, 2);
                case "1/4": return (new[] { 1 }, 4);
                case "2/4": return (new[] { 1, 1 }, 4);
                case "3/4": return (new[] { 1, 1, 1 }, 4);
                case "4/4": return (new[] { 1, 1, 1, 1 }, 4);
                case "5/4": return (new[] { 1, 1, 1, 1, 1 }, 4);
                case "6/4": return (new[] { 1, 1, 1, 1, 1, 1 }, 4);
                case "1/8": return (new[] { 1 }, 8);
                case "2/8": return (new[] { 1, 1 }, 8);
                case "3/8": return (new[] { 1, 1, 1 }, 8);
                case "4/8": return (new[] { 1, 1, 1, 1 }, 8);
                case "5/8": return (new[] { 1, 1, 1, 1, 1 }, 8);
                case "6/8": return (new[] { 1, 1, 1, 1, 1, 1 }, 8);
                case "7/8": return (new[] { 1, 1, 1, 1, 1, 1, 1 }, 8);
                case "8/8": return (new[] { 1, 1, 1, 1, 1, 1, 1, 1 }, 8);
                default: throw new Exception("unknown meterButtonText");
            }
        }
    }

    private void ApplyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // 这里获取到的IsChecked已经切换后的值了
        if (ApplyButton.IsChecked == true)
            foreach (var meterButton in _meterButtonList)
                meterButton.IsChecked = false;

        ApplyButton.IsChecked = true;
        Meter = MeterTextToMeter(_viewModel.MeterText);

        if (IsPlaying)
        {
            Stop();
            Play();
        }

        (int[] Intervals, int BeatUnit) MeterTextToMeter(string meterText)
        {
            try
            {
                var array = meterText.Split('/');
                var beatUnit = int.Parse(array[1]);
                var intervals = array[0].Split(',').Select(int.Parse).ToArray();
                intervals = ReduceFractions(intervals); // 化简为最简形式
                return (intervals, beatUnit);
            }
            catch (Exception exception)
            {
                ApplyButton.IsChecked = false;
                MeterButton6.IsChecked = true;
                return (new[] { 1, 1, 1, 1 }, 4);
            }
        }

        int[] ReduceFractions(int[] numbers)
        {
            var gcd = numbers[0];

            for (var i = 0; i < numbers.Length; i++)
            for (var j = i + 1; j < numbers.Length; j++)
                gcd = CalculateGCD(gcd, numbers[j]);

            return numbers.Select(p => p / gcd).ToArray();
        }

        int CalculateGCD(int a, int b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }

    #endregion
}