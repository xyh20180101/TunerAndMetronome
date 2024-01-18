using System;
using System.Collections.Generic;

namespace TunerAndMetronome;

public class NoteData
{
    /// <summary>
    /// 音高频率数据
    /// <para>https://zh.wikipedia.org/wiki/%E9%9F%B3%E9%AB%98</para>
    /// </summary>
    public static readonly IReadOnlyList<(float Frequency, string NoteName, int Octave)> Data =
        new List<(float Frequency, string NoteName, int Octave)>
        {
            (16.352f, "C", 0),
            (17.324f, "C#", 0),
            (18.354f, "D", 0),
            (19.445f, "D#", 0),
            (20.602f, "E", 0),
            (21.827f, "F", 0),
            (23.125f, "F#", 0),
            (24.500f, "G", 0),
            (25.957f, "G#", 0),
            (27.500f, "A", 0),
            (29.135f, "A#", 0),
            (30.868f, "B", 0),

            (32.703f, "C", 1),
            (34.648f, "C#", 1),
            (36.708f, "D", 1),
            (38.891f, "D#", 1),
            (41.203f, "E", 1),
            (43.654f, "F", 1),
            (46.249f, "F#", 1),
            (48.999f, "G", 1),
            (51.913f, "G#", 1),
            (55.000f, "A", 1),
            (58.270f, "A#", 1),
            (61.735f, "B", 1),

            (65.406f, "C", 2),
            (69.296f, "C#", 2),
            (73.416f, "D", 2),
            (77.782f, "D#", 2),
            (82.407f, "E", 2),
            (87.307f, "F", 2),
            (92.499f, "F#", 2),
            (97.999f, "G", 2),
            (103.83f, "G#", 2),
            (110.00f, "A", 2),
            (116.54f, "A#", 2),
            (123.47f, "B", 2),

            (130.81f, "C", 3),
            (138.59f, "C#", 3),
            (146.83f, "D", 3),
            (155.56f, "D#", 3),
            (164.81f, "E", 3),
            (174.61f, "F", 3),
            (185.00f, "F#", 3),
            (196.00f, "G", 3),
            (207.65f, "G#", 3),
            (220.00f, "A", 3),
            (233.08f, "A#", 3),
            (246.94f, "B", 3),

            (261.63f, "C", 4),
            (277.18f, "C#", 4),
            (293.66f, "D", 4),
            (311.13f, "D#", 4),
            (329.63f, "E", 4),
            (349.23f, "F", 4),
            (369.99f, "F#", 4),
            (392.00f, "G", 4),
            (415.30f, "G#", 4),
            (440.00f, "A", 4),
            (466.16f, "A#", 4),
            (493.88f, "B", 4),

            (523.25f, "C", 5),
            (554.37f, "C#", 5),
            (587.33f, "D", 5),
            (622.25f, "D#", 5),
            (659.26f, "E", 5),
            (698.46f, "F", 5),
            (739.99f, "F#", 5),
            (783.99f, "G", 5),
            (830.61f, "G#", 5),
            (880.00f, "A", 5),
            (932.33f, "A#", 5),
            (987.77f, "B", 5),

            (1046.5f, "C", 6),
            (1108.7f, "C#", 6),
            (1174.7f, "D", 6),
            (1244.5f, "D#", 6),
            (1318.5f, "E", 6),
            (1396.9f, "F", 6),
            (1480.0f, "F#", 6),
            (1568.0f, "G", 6),
            (1661.2f, "G#", 6),
            (1760.0f, "A", 6),
            (1864.7f, "A#", 6),
            (1975.5f, "B", 6),

            (2093.0f, "C", 7),
            (2217.5f, "C#", 7),
            (2349.3f, "D", 7),
            (2489.0f, "D#", 7),
            (2637.0f, "E", 7),
            (2793.8f, "F", 7),
            (2960.0f, "F#", 7),
            (3136.0f, "G", 7),
            (3322.4f, "G#", 7),
            (3520.0f, "A", 7),
            (3729.3f, "A#", 7),
            (3951.1f, "B", 7),

            (4186.0f, "C", 8),
            (4434.9f, "C#", 8),
            (4698.6f, "D", 8),
            (4978.0f, "D#", 8),
            (5274.0f, "E", 8),
            (5587.7f, "F", 8),
            (5919.9f, "F#", 8),
            (6271.9f, "G", 8),
            (6644.9f, "G#", 8),
            (7040.0f, "A", 8),
            (7458.6f, "A#", 8),
            (7902.1f, "B", 8),

            (8372.0f, "C", 9),
            (8869.8f, "C#", 9),
            (9397.3f, "D", 9),
            (9956.1f, "D#", 9),
            (10548f, "E", 9),
            (11175f, "F", 9),
            (11840f, "F#", 9),
            (12544f, "G", 9),
            (13290f, "G#", 9),
            (14080f, "A", 9),
            (14917f, "A#", 9),
            (15804f, "B", 9)
        };

    /// <summary>
    /// 获取最接近的标准音下标
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static int FindClosestIndex(float value)
    {
        var left = 0;
        var right = Data.Count - 1;

        while (left <= right)
        {
            var mid = left + (right - left) / 2;

            if (Math.Abs(Data[mid].Frequency - value) < 1e-9)
                return mid;

            if (Data[mid].Frequency < value)
                left = mid + 1;
            else
                right = mid - 1;
        }

        // 在循环结束时，left 和 right 之间的差值可能为 1，选择离目标值更近的一侧
        if (left > 0 && left < Data.Count &&
            Math.Abs(Data[left - 1].Frequency - value) < Math.Abs(Data[left].Frequency - value))
            return left - 1;
        if (left < Data.Count)
            return left; // 目标值大于数组最大值，返回数组最后一个元素的下标
        return Data.Count - 1;
    }

    /// <summary>
    /// 获取输入频率和标准音之间的差距信息
    /// </summary>
    /// <param name="frequency"></param>
    public static ((float Frequency, string NoteName, int Octave) NoteData, float Delta, float NormalizeDelta,
        bool Acceptable) GetNoteInformation(float frequency)
    {
        var closestIndex = FindClosestIndex(frequency);

        var delta = frequency - Data[closestIndex].Frequency;
        float range;
        float normalizeDelta;

        if (delta >= 0) // 高于标准音
        {
            if (closestIndex + 1 >= Data.Count) // 超出范围
                range = (Data[closestIndex].Frequency - Data[closestIndex - 1].Frequency) / 2; // 取前一个间距
            else
                range = (Data[closestIndex + 1].Frequency - Data[closestIndex].Frequency) / 2; // 取下一个间距

            if (delta > range) // 高于B9
                return ((frequency, "", 9), 0, 0, false);

            normalizeDelta = delta / range;
        }
        else // 低于标准音
        {
            if (closestIndex - 1 < 0) // 超出范围
                range = (Data[closestIndex + 1].Frequency - Data[closestIndex].Frequency) / 2; // 取下一个间距
            else
                range = (Data[closestIndex].Frequency - Data[closestIndex - 1].Frequency) / 2; // 取前一个间距

            if (-delta > range) // 低于C0
                return ((frequency, "", 0), 0, 0, false);

            normalizeDelta = delta / range;
        }

        // 误差是否可接受
        var acceptable = Math.Abs(normalizeDelta) <= 0.25;

        return (Data[closestIndex], delta, normalizeDelta, acceptable);
    }
}