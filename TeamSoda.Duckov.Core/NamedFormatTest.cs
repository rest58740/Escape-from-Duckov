using System;
using System.Diagnostics;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class NamedFormatTest : MonoBehaviour
{
	// Token: 0x0600099C RID: 2460 RVA: 0x0002AF7C File Offset: 0x0002917C
	private void Test()
	{
		string text = "";
		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < this.loopCount; i++)
		{
			text = StringExtensions.Format(this.format, this.content);
		}
		stopwatch.Stop();
		Debug.Log("Time Consumed 1:" + stopwatch.ElapsedMilliseconds.ToString());
		stopwatch = Stopwatch.StartNew();
		for (int j = 0; j < this.loopCount; j++)
		{
			text = string.Format(this.format2, this.content.textA, this.content.textB);
		}
		stopwatch.Stop();
		Debug.Log("Time Consumed 2:" + stopwatch.ElapsedMilliseconds.ToString());
		Debug.Log(text);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0002B048 File Offset: 0x00029248
	private void Test2()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		string text = StringExtensions.Format(this.format, new
		{
			this.content.textA,
			this.content.textB
		});
		stopwatch.Stop();
		Debug.Log("Time Consumed:" + stopwatch.ElapsedMilliseconds.ToString());
		Debug.Log(text);
	}

	// Token: 0x040008A6 RID: 2214
	public string format = "Displaying {textA} {textB}";

	// Token: 0x040008A7 RID: 2215
	public string format2 = "Displaying {0} {1}";

	// Token: 0x040008A8 RID: 2216
	public NamedFormatTest.Content content;

	// Token: 0x040008A9 RID: 2217
	[SerializeField]
	private int loopCount = 100;

	// Token: 0x020004B4 RID: 1204
	[Serializable]
	public struct Content
	{
		// Token: 0x04001CBA RID: 7354
		public string textA;

		// Token: 0x04001CBB RID: 7355
		public string textB;
	}
}
