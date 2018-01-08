
#if UNITY_ANDROID && !UNITY_EDITOR
#define DEVICE_ANDROID
#endif //UNITY_ANDROID && !UNITY_EDITOR

namespace Android.CpuFreqency
{
#if DEVICE_ANDROID
    using System.Runtime.InteropServices;
#endif //DEVICE_ANDROID

    /// <summary>
    /// Android CPU周波数
    /// </summary>
    public static class CpuFrequency
    {
        /// <summary>
        /// 現在アクティブなCPUの周波数を取得する
        /// </summary>
        /// <param name="freqs">結果を格納する配列。<paramref name="cpuNum"/>よりも配列長が小さい場合、ヒープを消費して拡張される。</param>
        /// <param name="cpuNum">CPUコア数の上限値</param>
        /// <returns>正常に取得できた場合、真を返す</returns>
        public static bool Read(ref int[] freqs, byte cpuNum = 8)
        {
#if DEVICE_ANDROID
            if (cpuNum <= 0) return false;
            if (freqs == null) freqs = new int[cpuNum];
            if (freqs.Length < cpuNum) System.Array.Resize(ref freqs, cpuNum);

            unsafe
            {
                fixed (int* ptr = &freqs[0])
                {
                    return (read_freqs(cpuNum, (System.IntPtr)ptr) == 0);
                }
            }
#else
            return false;
#endif //DEVICE_ANDROID
        }

#if DEVICE_ANDROID
        [DllImport("cpufreq")]
        private static extern byte read_freqs(byte in_cpu_num, System.IntPtr out_cpufreqs);
#endif //DEVICE_ANDROID
    }
}
