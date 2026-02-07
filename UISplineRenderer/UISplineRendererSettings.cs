using System;
using UnityEngine;

namespace UI_Spline_Renderer
{
	// Token: 0x02000017 RID: 23
	public class UISplineRendererSettings : ScriptableObject
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000051BA File Offset: 0x000033BA
		public static UISplineRendererSettings Instance
		{
			get
			{
				if (UISplineRendererSettings._instance == null)
				{
					UISplineRendererSettings._instance = Resources.Load<UISplineRendererSettings>("UISplineRenderer Settings");
				}
				return UISplineRendererSettings._instance;
			}
		}

		// Token: 0x04000066 RID: 102
		private static UISplineRendererSettings _instance;

		// Token: 0x04000067 RID: 103
		public Texture defaultLineTexture;

		// Token: 0x04000068 RID: 104
		public Texture uvTestLineTexture;

		// Token: 0x04000069 RID: 105
		public Sprite triangleHead;

		// Token: 0x0400006A RID: 106
		public Sprite arrowHead;

		// Token: 0x0400006B RID: 107
		public Sprite emptyCircleHead;

		// Token: 0x0400006C RID: 108
		public Sprite filledCircleHead;
	}
}
