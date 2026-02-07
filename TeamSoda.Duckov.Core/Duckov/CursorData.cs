using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023C RID: 572
	[Serializable]
	public class CursorData
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x000466D0 File Offset: 0x000448D0
		public Texture2D texture
		{
			get
			{
				if (this.textures.Length == 0)
				{
					return null;
				}
				return this.textures[0];
			}
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x000466E8 File Offset: 0x000448E8
		internal void Apply(int frame)
		{
			if (this.textures == null || this.textures.Length < 1)
			{
				Cursor.SetCursor(null, default(Vector2), 0);
				return;
			}
			if (frame < 0)
			{
				int num = this.textures.Length;
				frame = (-frame / this.textures.Length + 1) * num + frame;
			}
			frame %= this.textures.Length;
			Cursor.SetCursor(this.textures[frame], this.hotspot, 0);
		}

		// Token: 0x04000E01 RID: 3585
		public Texture2D[] textures;

		// Token: 0x04000E02 RID: 3586
		public Vector2 hotspot;

		// Token: 0x04000E03 RID: 3587
		public float fps;
	}
}
