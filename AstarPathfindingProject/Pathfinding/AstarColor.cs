using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	public class AstarColor : ISerializationCallbackReceiver
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x00009898 File Offset: 0x00007A98
		public static int ColorHash()
		{
			int num = AstarColor.SolidColor.GetHashCode() ^ AstarColor.UnwalkableNode.GetHashCode() ^ AstarColor.BoundsHandles.GetHashCode() ^ AstarColor.ConnectionLowLerp.GetHashCode() ^ AstarColor.ConnectionHighLerp.GetHashCode() ^ AstarColor.MeshEdgeColor.GetHashCode();
			for (int i = 0; i < AstarColor.AreaColors.Length; i++)
			{
				num = (7 * num ^ AstarColor.AreaColors[i].GetHashCode());
			}
			return num;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009939 File Offset: 0x00007B39
		public static Color GetAreaColor(uint area)
		{
			if ((ulong)area >= (ulong)((long)AstarColor.AreaColors.Length))
			{
				return AstarMath.IntToColor((int)area, 1f);
			}
			return AstarColor.AreaColors[(int)area];
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009939 File Offset: 0x00007B39
		public static Color GetTagColor(uint tag)
		{
			if ((ulong)tag >= (ulong)((long)AstarColor.AreaColors.Length))
			{
				return AstarMath.IntToColor((int)tag, 1f);
			}
			return AstarColor.AreaColors[(int)tag];
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009960 File Offset: 0x00007B60
		public void PushToStatic()
		{
			this._AreaColors = (this._AreaColors ?? new Color[0]);
			AstarColor.SolidColor = this._SolidColor;
			AstarColor.UnwalkableNode = this._UnwalkableNode;
			AstarColor.BoundsHandles = this._BoundsHandles;
			AstarColor.ConnectionLowLerp = this._ConnectionLowLerp;
			AstarColor.ConnectionHighLerp = this._ConnectionHighLerp;
			AstarColor.MeshEdgeColor = this._MeshEdgeColor;
			AstarColor.AreaColors = this._AreaColors;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000035CE File Offset: 0x000017CE
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000099D0 File Offset: 0x00007BD0
		public void OnAfterDeserialize()
		{
			if (this._AreaColors != null && this._AreaColors.Length == 1 && this._AreaColors[0] == default(Color))
			{
				this._AreaColors = new Color[0];
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009A18 File Offset: 0x00007C18
		public AstarColor()
		{
			this._SolidColor = new Color(0.11764706f, 0.4f, 0.7882353f, 0.9f);
			this._UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);
			this._BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
			this._ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);
			this._ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);
			this._MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);
		}

		// Token: 0x04000121 RID: 289
		public Color _SolidColor;

		// Token: 0x04000122 RID: 290
		public Color _UnwalkableNode;

		// Token: 0x04000123 RID: 291
		public Color _BoundsHandles;

		// Token: 0x04000124 RID: 292
		public Color _ConnectionLowLerp;

		// Token: 0x04000125 RID: 293
		public Color _ConnectionHighLerp;

		// Token: 0x04000126 RID: 294
		public Color _MeshEdgeColor;

		// Token: 0x04000127 RID: 295
		public Color[] _AreaColors;

		// Token: 0x04000128 RID: 296
		public static Color SolidColor = new Color(0.11764706f, 0.4f, 0.7882353f, 0.9f);

		// Token: 0x04000129 RID: 297
		public static Color UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x0400012A RID: 298
		public static Color BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);

		// Token: 0x0400012B RID: 299
		public static Color ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);

		// Token: 0x0400012C RID: 300
		public static Color ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x0400012D RID: 301
		public static Color MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x0400012E RID: 302
		private static Color[] AreaColors = new Color[1];
	}
}
