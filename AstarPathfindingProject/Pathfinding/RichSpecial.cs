using System;

namespace Pathfinding
{
	// Token: 0x02000021 RID: 33
	public class RichSpecial : RichPathPart
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000091AC File Offset: 0x000073AC
		public FakeTransform first
		{
			get
			{
				return new FakeTransform
				{
					position = this.nodeLink.relativeStart,
					rotation = (this.nodeLink.isReverse ? this.nodeLink.link.end.rotation : this.nodeLink.link.start.rotation)
				};
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00009214 File Offset: 0x00007414
		public FakeTransform second
		{
			get
			{
				return new FakeTransform
				{
					position = this.nodeLink.relativeEnd,
					rotation = (this.nodeLink.isReverse ? this.nodeLink.link.start.rotation : this.nodeLink.link.end.rotation)
				};
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000927C File Offset: 0x0000747C
		public bool reverse
		{
			get
			{
				return this.nodeLink.isReverse;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009289 File Offset: 0x00007489
		public override void OnEnterPool()
		{
			this.nodeLink = default(OffMeshLinks.OffMeshLinkTracer);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009297 File Offset: 0x00007497
		public RichSpecial Initialize(OffMeshLinks.OffMeshLinkTracer nodeLink)
		{
			this.nodeLink = nodeLink;
			return this;
		}

		// Token: 0x04000108 RID: 264
		public OffMeshLinks.OffMeshLinkTracer nodeLink;
	}
}
