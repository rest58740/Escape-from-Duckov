using System;

namespace Shapes
{
	// Token: 0x02000041 RID: 65
	public class TextElement : IDisposable
	{
		// Token: 0x06000C2D RID: 3117 RVA: 0x00018774 File Offset: 0x00016974
		public static int GetNextId()
		{
			return TextElement.idCounter++;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00018783 File Offset: 0x00016983
		public TextMeshProShapes Tmp
		{
			get
			{
				return ShapesObjPool<TextMeshProShapes, ShapesTextPool>.Instance.GetElement(this.id);
			}
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00018795 File Offset: 0x00016995
		public TextElement()
		{
			this.id = TextElement.GetNextId();
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000187A8 File Offset: 0x000169A8
		public void Dispose()
		{
			ShapesObjPool<TextMeshProShapes, ShapesTextPool>.Instance.ReleaseElement(this.id);
		}

		// Token: 0x040001AA RID: 426
		private static int idCounter;

		// Token: 0x040001AB RID: 427
		public readonly int id;
	}
}
