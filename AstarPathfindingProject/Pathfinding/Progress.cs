using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200002C RID: 44
	public readonly struct Progress
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00009EB5 File Offset: 0x000080B5
		public Progress(float progress, ScanningStage stage, int graphIndex = 0, int graphCount = 0)
		{
			this.progress = progress;
			this.stage = stage;
			this.graphIndex = graphIndex;
			this.graphCount = graphCount;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00009ED4 File Offset: 0x000080D4
		public Progress MapTo(float min, float max)
		{
			return new Progress(Mathf.Lerp(min, max, this.progress), this.stage, this.graphIndex, this.graphCount);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009EFC File Offset: 0x000080FC
		public override string ToString()
		{
			string text = this.progress.ToString("0%") + " ";
			switch (this.stage)
			{
			case ScanningStage.PreProcessingGraphs:
				text += "Pre-processing graphs";
				break;
			case ScanningStage.PreProcessingGraph:
				text = string.Concat(new string[]
				{
					text,
					"Pre-processing graph ",
					(this.graphIndex + 1).ToString(),
					" of ",
					this.graphCount.ToString()
				});
				break;
			case ScanningStage.ScanningGraph:
				text = string.Concat(new string[]
				{
					text,
					"Scanning graph ",
					(this.graphIndex + 1).ToString(),
					" of ",
					this.graphCount.ToString()
				});
				break;
			case ScanningStage.PostProcessingGraph:
				text = string.Concat(new string[]
				{
					text,
					"Post-processing graph ",
					(this.graphIndex + 1).ToString(),
					" of ",
					this.graphCount.ToString()
				});
				break;
			case ScanningStage.FinishingScans:
				text += "Finalizing graph scans";
				break;
			}
			return text;
		}

		// Token: 0x0400014A RID: 330
		public readonly float progress;

		// Token: 0x0400014B RID: 331
		internal readonly ScanningStage stage;

		// Token: 0x0400014C RID: 332
		internal readonly int graphIndex;

		// Token: 0x0400014D RID: 333
		internal readonly int graphCount;
	}
}
