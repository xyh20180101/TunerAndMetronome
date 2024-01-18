namespace TunerAndMetronome;

public class FrequencyDetector
{
    /// <summary>
    /// 归一化自相关
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static float[] NAC(float[] data)
    {
        var result = new float[data.Length];

        for (var w = 0; w < data.Length; w++)
        {
            var acf = 0f;
            var squareSum = 0f;

            for (var i = 0; i < data.Length - w; i++)
            {
                acf += data[i] * data[i + w];
                squareSum += data[i] * data[i] + data[i + w] * data[i + w];
            }

            result[w] = squareSum != 0 ? 2 * acf / squareSum : 0;
        }

        return result;
    }

    /// <summary>
    /// 获取周期峰值
    /// </summary>
    /// <param name="data"></param>
    /// <param name="useInterpolation"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    private static float Period(float[] data, bool useInterpolation, float threshold = 0.95f)
    {
        var max = 0f;
        var maxIndex = 0;
        for (var i = 1; i < data.Length; i++)
        {
            max = data[i] > max ? data[i] : max;
            maxIndex = i;
        }

        for (var i = 1; i < data.Length - 1; i++)
            if (data[i] > max * threshold && data[i] >= data[i - 1] && data[i] > data[i + 1])
                return useInterpolation ? QuadraticInterpolation(data, i) : i;

        return useInterpolation ? QuadraticInterpolation(data, maxIndex) : maxIndex;
    }

    /// <summary>
    /// 二次插值
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private static float QuadraticInterpolation(float[] data, int index)
    {
        if (index > 0 && index < data.Length - 1)
        {
            var a = data[index - 1];
            var b = data[index];
            var c = data[index + 1];

            var vertex = index + 0.5f * (a - c) / (a - 2 * b + c);
            return vertex;
        }

        return index;
    }

    /// <summary>
    /// 估计频率
    /// </summary>
    /// <param name="data">归一化音频数据</param>
    /// <param name="sampleRate">采样率</param>
    /// <param name="useInterpolation">使用二次插值</param>
    /// <returns></returns>
    public static float DetectFrequency(float[] data, int sampleRate, bool useInterpolation)
    {
        var d = NAC(data);
        var p = Period(d, useInterpolation);
        return sampleRate / p;
    }
}