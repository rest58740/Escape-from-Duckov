using System;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x02000011 RID: 17
	[DisallowMultipleComponent]
	public abstract class ProceduralImageModifier : MonoBehaviour
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000032E8 File Offset: 0x000014E8
		protected Graphic _Graphic
		{
			get
			{
				if (this.graphic == null)
				{
					this.graphic = base.GetComponent<Graphic>();
				}
				return this.graphic;
			}
		}

		// Token: 0x06000065 RID: 101
		public abstract Vector4 CalculateRadius(Rect imageRect);

		// Token: 0x04000021 RID: 33
		protected Graphic graphic;
	}
}
