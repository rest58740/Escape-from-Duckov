using System;
using System.Collections.Generic;
using Pathfinding.Jobs;
using Pathfinding.Serialization;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x020001FF RID: 511
	[JsonOptIn]
	public class GridGraphRules
	{
		// Token: 0x06000CC6 RID: 3270 RVA: 0x000501EB File Offset: 0x0004E3EB
		public void AddRule(GridGraphRule rule)
		{
			this.rules.Add(rule);
			this.lastHash = -1L;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00050201 File Offset: 0x0004E401
		public void RemoveRule(GridGraphRule rule)
		{
			this.rules.Remove(rule);
			this.lastHash = -1L;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00050218 File Offset: 0x0004E418
		public IReadOnlyList<GridGraphRule> GetRules()
		{
			if (this.rules == null)
			{
				this.rules = new List<GridGraphRule>();
			}
			return this.rules.AsReadOnly();
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00050238 File Offset: 0x0004E438
		private long Hash()
		{
			long num = 196613L;
			for (int i = 0; i < this.rules.Count; i++)
			{
				if (this.rules[i] != null && this.rules[i].enabled)
				{
					num = (num * 1572869L ^ (long)this.rules[i].Hash);
				}
			}
			return num;
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x000502A0 File Offset: 0x0004E4A0
		public void RebuildIfNecessary()
		{
			long num = this.Hash();
			if (num == this.lastHash && this.jobSystemCallbacks != null && this.mainThreadCallbacks != null)
			{
				return;
			}
			this.lastHash = num;
			this.Rebuild();
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x000502DC File Offset: 0x0004E4DC
		public void Rebuild()
		{
			this.rules = (this.rules ?? new List<GridGraphRule>());
			this.jobSystemCallbacks = (this.jobSystemCallbacks ?? new List<Action<GridGraphRules.Context>>[6]);
			for (int i = 0; i < this.jobSystemCallbacks.Length; i++)
			{
				if (this.jobSystemCallbacks[i] != null)
				{
					this.jobSystemCallbacks[i].Clear();
				}
			}
			this.mainThreadCallbacks = (this.mainThreadCallbacks ?? new List<Action<GridGraphRules.Context>>[6]);
			for (int j = 0; j < this.mainThreadCallbacks.Length; j++)
			{
				if (this.mainThreadCallbacks[j] != null)
				{
					this.mainThreadCallbacks[j].Clear();
				}
			}
			for (int k = 0; k < this.rules.Count; k++)
			{
				if (this.rules[k].enabled)
				{
					this.rules[k].Register(this);
				}
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x000503BC File Offset: 0x0004E5BC
		public void DisposeUnmanagedData()
		{
			if (this.rules != null)
			{
				for (int i = 0; i < this.rules.Count; i++)
				{
					if (this.rules[i] != null)
					{
						this.rules[i].DisposeUnmanagedData();
						this.rules[i].SetDirty();
					}
				}
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00050418 File Offset: 0x0004E618
		private static void CallActions(List<Action<GridGraphRules.Context>> actions, GridGraphRules.Context context)
		{
			if (actions != null)
			{
				try
				{
					for (int i = 0; i < actions.Count; i++)
					{
						actions[i](context);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00050460 File Offset: 0x0004E660
		public IEnumerator<JobHandle> ExecuteRule(GridGraphRule.Pass rule, GridGraphRules.Context context)
		{
			if (this.jobSystemCallbacks == null)
			{
				this.Rebuild();
			}
			GridGraphRules.CallActions(this.jobSystemCallbacks[(int)rule], context);
			if (this.mainThreadCallbacks[(int)rule] != null && this.mainThreadCallbacks[(int)rule].Count > 0)
			{
				if (!context.tracker.forceLinearDependencies)
				{
					yield return context.tracker.AllWritesDependency;
				}
				GridGraphRules.CallActions(this.mainThreadCallbacks[(int)rule], context);
			}
			yield break;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00050480 File Offset: 0x0004E680
		public void ExecuteRuleMainThread(GridGraphRule.Pass rule, GridGraphRules.Context context)
		{
			if (this.jobSystemCallbacks == null)
			{
				this.Rebuild();
			}
			if (this.jobSystemCallbacks[(int)rule] != null && this.jobSystemCallbacks[(int)rule].Count > 0)
			{
				throw new Exception(string.Concat(new string[]
				{
					"A job system pass has been added for the ",
					rule.ToString(),
					" pass. ",
					rule.ToString(),
					" only supports main thread callbacks."
				}));
			}
			if (context.tracker != null)
			{
				context.tracker.AllWritesDependency.Complete();
			}
			GridGraphRules.CallActions(this.mainThreadCallbacks[(int)rule], context);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00050528 File Offset: 0x0004E728
		public void AddJobSystemPass(GridGraphRule.Pass pass, Action<GridGraphRules.Context> action)
		{
			if (this.jobSystemCallbacks[(int)pass] == null)
			{
				this.jobSystemCallbacks[(int)pass] = new List<Action<GridGraphRules.Context>>();
			}
			this.jobSystemCallbacks[(int)pass].Add(action);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0005055C File Offset: 0x0004E75C
		public void AddMainThreadPass(GridGraphRule.Pass pass, Action<GridGraphRules.Context> action)
		{
			if (this.mainThreadCallbacks[(int)pass] == null)
			{
				this.mainThreadCallbacks[(int)pass] = new List<Action<GridGraphRules.Context>>();
			}
			this.mainThreadCallbacks[(int)pass].Add(action);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00050590 File Offset: 0x0004E790
		[Obsolete("Use AddJobSystemPass or AddMainThreadPass instead")]
		public void Add(GridGraphRule.Pass pass, Action<GridGraphRules.Context> action)
		{
			this.AddJobSystemPass(pass, action);
		}

		// Token: 0x0400095A RID: 2394
		private List<Action<GridGraphRules.Context>>[] jobSystemCallbacks;

		// Token: 0x0400095B RID: 2395
		private List<Action<GridGraphRules.Context>>[] mainThreadCallbacks;

		// Token: 0x0400095C RID: 2396
		[JsonMember]
		private List<GridGraphRule> rules = new List<GridGraphRule>();

		// Token: 0x0400095D RID: 2397
		private long lastHash;

		// Token: 0x02000200 RID: 512
		public class Context
		{
			// Token: 0x170001CA RID: 458
			// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x000505AD File Offset: 0x0004E7AD
			public JobDependencyTracker tracker
			{
				get
				{
					return this.data.dependencyTracker;
				}
			}

			// Token: 0x0400095E RID: 2398
			public GridGraph graph;

			// Token: 0x0400095F RID: 2399
			public GridGraphScanData data;
		}
	}
}
