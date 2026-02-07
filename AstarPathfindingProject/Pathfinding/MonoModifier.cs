using System;

namespace Pathfinding
{
	// Token: 0x02000116 RID: 278
	[Serializable]
	public abstract class MonoModifier : VersionedMonoBehaviour, IPathModifier
	{
		// Token: 0x060008D8 RID: 2264 RVA: 0x0002E95E File Offset: 0x0002CB5E
		protected virtual void OnEnable()
		{
			this.seeker = base.GetComponent<Seeker>();
			if (this.seeker != null)
			{
				this.seeker.RegisterModifier(this);
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002E986 File Offset: 0x0002CB86
		protected virtual void OnDisable()
		{
			if (this.seeker != null)
			{
				this.seeker.DeregisterModifier(this);
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060008DA RID: 2266
		public abstract int Order { get; }

		// Token: 0x060008DB RID: 2267 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void PreProcess(Path path)
		{
		}

		// Token: 0x060008DC RID: 2268
		public abstract void Apply(Path path);

		// Token: 0x040005CD RID: 1485
		[NonSerialized]
		public Seeker seeker;
	}
}
