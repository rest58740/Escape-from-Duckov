using System;
using System.Diagnostics;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000026 RID: 38
	public struct SimpleTimer : IDisposable
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00009014 File Offset: 0x00007214
		public static double CurrentTime
		{
			get
			{
				return SimpleTimer.Stopwatch.Elapsed.TotalSeconds;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00009033 File Offset: 0x00007233
		public bool IsStarted
		{
			get
			{
				return this.startTime != 0.0;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00009049 File Offset: 0x00007249
		public SimpleTimer(string name)
		{
			this.name = name;
			this.startTime = 0.0;
			this.total = 0.0;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00009070 File Offset: 0x00007270
		public static SimpleTimer Start(string name = null)
		{
			return new SimpleTimer
			{
				name = name,
				startTime = SimpleTimer.CurrentTime
			};
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000909A File Offset: 0x0000729A
		public bool Start()
		{
			if (this.startTime != 0.0)
			{
				return false;
			}
			this.startTime = SimpleTimer.CurrentTime;
			return true;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000090BC File Offset: 0x000072BC
		public bool Stop()
		{
			if (this.startTime == 0.0)
			{
				return false;
			}
			double currentTime = SimpleTimer.CurrentTime;
			this.total += currentTime - this.startTime;
			this.startTime = 0.0;
			return true;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00009108 File Offset: 0x00007308
		public override string ToString()
		{
			this.Stop();
			if (!string.IsNullOrEmpty(this.name))
			{
				return this.name + ": " + this.total.ToString("0.000");
			}
			return this.total.ToString("0.000");
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000915A File Offset: 0x0000735A
		public void Dispose()
		{
			UnityEngine.Debug.Log(this.ToString());
		}

		// Token: 0x04000084 RID: 132
		public static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

		// Token: 0x04000085 RID: 133
		public string name;

		// Token: 0x04000086 RID: 134
		public double startTime;

		// Token: 0x04000087 RID: 135
		public double total;

		// Token: 0x04000088 RID: 136
		private const string Format = "0.000";
	}
}
