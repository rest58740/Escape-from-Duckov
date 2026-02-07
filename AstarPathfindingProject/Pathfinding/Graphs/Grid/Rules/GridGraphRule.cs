using System;
using Pathfinding.Serialization;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x02000202 RID: 514
	[JsonDynamicType]
	[JsonDynamicTypeAlias("Pathfinding.RuleTexture", typeof(RuleTexture))]
	[JsonDynamicTypeAlias("Pathfinding.RuleAnglePenalty", typeof(RuleAnglePenalty))]
	[JsonDynamicTypeAlias("Pathfinding.RuleElevationPenalty", typeof(RuleElevationPenalty))]
	[JsonDynamicTypeAlias("Pathfinding.RulePerLayerModifications", typeof(RulePerLayerModifications))]
	public abstract class GridGraphRule
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000506AA File Offset: 0x0004E8AA
		public virtual int Hash
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x000506B2 File Offset: 0x0004E8B2
		public virtual void SetDirty()
		{
			this.dirty++;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void DisposeUnmanagedData()
		{
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void Register(GridGraphRules rules)
		{
		}

		// Token: 0x04000965 RID: 2405
		[JsonMember]
		public bool enabled = true;

		// Token: 0x04000966 RID: 2406
		private int dirty = 1;

		// Token: 0x02000203 RID: 515
		public enum Pass
		{
			// Token: 0x04000968 RID: 2408
			BeforeCollision,
			// Token: 0x04000969 RID: 2409
			BeforeConnections,
			// Token: 0x0400096A RID: 2410
			AfterConnections,
			// Token: 0x0400096B RID: 2411
			AfterErosion,
			// Token: 0x0400096C RID: 2412
			PostProcess,
			// Token: 0x0400096D RID: 2413
			AfterApplied
		}
	}
}
