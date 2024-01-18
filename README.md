# TunerAndMetronome

使用.NET Avalonia框架开发的跨平台调音器与节拍器应用（Windows和Android）

![](/doc/logo.png)

**界面预览图**

![](/doc/preview.png)

## 前言
本项目仅以个人学习为目的进行开发，无法保证功能在你的设备上正常运行。如果你正在寻找专业级的安卓应用，对于调音器我推荐`Boss Tuner`，对于节拍器我推荐`Stage Metronome`。

## 功能说明

### 调音器
- 音频输入为麦克风时，你需要保证信噪比足够高，本应用无法识别不插电的电吉他琴弦声，但接入音箱并提高到一定音量后则可以。

### 节拍器
- 自定义节拍表达式说明：前面为一小节内的音符时长占比，用`,`分隔，后面为音符时值。例如：

    | 节拍 | 表达式 |
    | - | - |
    | 4/4 | 1,1,1,1/4 |
    | 6/8 | 1,1,1,1,1,1/8 |
    | 四分音符 + 二分音符 + 四分音符 | 1,2,1/4 |

## 具体实现

- 跨平台框架：Avalonia + .NET 7
- Windows录制实现：NAudio中的WasapiCapture [WindowsAudioRecorder.cs](/TunerAndMetronome.Desktop/AudioRecorders/WindowsAudioRecorder.cs)
- Android录制实现：AudioRecord [AndroidAudioRecorder.cs](/TunerAndMetronome.Android/AudioRecorders/AndroidAudioRecorder.cs)
- 基频分析算法：自相关函数+峰值检测+二次插值 [FrequencyDetector.cs](/TunerAndMetronome/FrequencyDetector.cs)
- Windows播放实现：NAudio中的WasapiOut，使用MixingSampleProvider [WindowsAudioPlayer.cs](/TunerAndMetronome.Desktop/AudioPlayers/WindowsAudioPlayer.cs)
- Android播放实现：AudioTrack，也使用了NAudio的MixingSampleProvider [AndroidAudioPlayer.cs](/TunerAndMetronome.Android/AudioPlayers/AndroidAudioPlayer.cs)
- 精准节拍实现：使用C#的System.Timers.Timer.Elapsed事件 [Metronome.axaml.cs](/TunerAndMetronome/Views/Metronome.axaml.cs)

## 更多
我编写了一篇文章，里面是关于整个开发过程的总结，同时也是对这方面的资料进行一些补充（我查相关资料的时候查得十分困难，chatGPT都不管用），感兴趣的欢迎查看[使用C#开发跨平台调音器和节拍器(Windows&Android) - 知乎](https://zhuanlan.zhihu.com/p/678523647)
