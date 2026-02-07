using System;
using UnityEngine;

namespace LeTai.TrueShadow
{
	// Token: 0x02000016 RID: 22
	internal class ShadowContainer
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004E99 File Offset: 0x00003099
		public RenderTexture Texture { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004EA1 File Offset: 0x000030A1
		public ShadowSettingSnapshot Snapshot { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004EA9 File Offset: 0x000030A9
		public int Padding { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004EB1 File Offset: 0x000030B1
		public Vector2Int ImprintSize { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004EB9 File Offset: 0x000030B9
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00004EC1 File Offset: 0x000030C1
		public int RefCount { get; internal set; }

		// Token: 0x06000096 RID: 150 RVA: 0x00004ECA File Offset: 0x000030CA
		internal ShadowContainer(RenderTexture texture, ShadowSettingSnapshot snapshot, int padding, Vector2Int imprintSize)
		{
			this.Texture = texture;
			this.Snapshot = snapshot;
			this.Padding = padding;
			this.ImprintSize = imprintSize;
			this.RefCount = 1;
			this.requestHash = snapshot.GetHashCode();
		}

		// Token: 0x0400007D RID: 125
		public readonly int requestHash;
	}
}
