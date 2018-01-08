
public class Main : UnityEngine.MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(PrintCpuFreq());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    System.Collections.IEnumerator PrintCpuFreq()
    {
        var data = new int[8];
        var builder = new System.Text.StringBuilder();
        var wait = new UnityEngine.WaitForSeconds(2f);

        while (true)
        {
            if (Android.CpuFreqency.CpuFrequency.Read(ref data, 8))
            {
                builder.Length = 0;
                builder.Append("CPU周波数の取得に成功しました\n\n");
                for (var i = 0; i < data.Length; i++)
                {
                    if (data[i] < 0) continue;
                    builder.AppendFormat("{0}: {1}KHz\n", i, data[i]);
                }
                UnityEngine.Debug.Log(builder.ToString());
            }
            else
            {
                UnityEngine.Debug.Log("CPU周波数の取得に失敗しました");
            }

            yield return wait;
        }
    }
}
