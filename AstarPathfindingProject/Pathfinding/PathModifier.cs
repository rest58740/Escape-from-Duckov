using System;

namespace Pathfinding
{
	// Token: 0x02000115 RID: 277
	[Serializable]
	public abstract class PathModifier : IPathModifier
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060008D2 RID: 2258
		public abstract int Order { get; }

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002E933 File Offset: 0x0002CB33
		public void Awake(Seeker seeker)
		{
			this.seeker = seeker;
			if (seeker != null)
			{
				seeker.RegisterModifier(this);
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002E94C File Offset: 0x0002CB4C
		public void OnDestroy(Seeker seeker)
		{
			if (seeker != null)
			{
				seeker.DeregisterModifier(this);
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void PreProcess(Path path)
		{
		}

		// Token: 0x060008D6 RID: 2262
		public abstract void Apply(Path path);

		// Token: 0x040005CC RID: 1484
		[NonSerialized]
		public Seeker seeker;
	}
}
