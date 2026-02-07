using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Drawing.Text
{
	// Token: 0x0200005C RID: 92
	internal struct SDFLookupData
	{
		// Token: 0x060003BA RID: 954 RVA: 0x000113BC File Offset: 0x0000F5BC
		public SDFLookupData(SDFFont font)
		{
			int num = 0;
			SDFCharacter value = font.characters[0];
			for (int i = 0; i < font.characters.Length; i++)
			{
				if (font.characters[i].codePoint == '?')
				{
					value = font.characters[i];
				}
				if (font.characters[i].codePoint >= '\u0080')
				{
					num++;
				}
			}
			this.characters = new NativeArray<SDFCharacter>(128 + num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < this.characters.Length; j++)
			{
				this.characters[j] = value;
			}
			this.lookup = new Dictionary<char, int>();
			this.material = font.material;
			num = 0;
			for (int k = 0; k < font.characters.Length; k++)
			{
				SDFCharacter sdfcharacter = font.characters[k];
				int num2 = (int)sdfcharacter.codePoint;
				if (sdfcharacter.codePoint >= '\u0080')
				{
					num2 = 128 + num;
					num++;
				}
				this.characters[num2] = sdfcharacter;
				this.lookup[sdfcharacter.codePoint] = num2;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000114E8 File Offset: 0x0000F6E8
		public int GetIndex(char c)
		{
			int result;
			if (this.lookup.TryGetValue(c, out result))
			{
				return result;
			}
			if (c == '\n')
			{
				return 65535;
			}
			return this.lookup['?'];
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001151F File Offset: 0x0000F71F
		public void Dispose()
		{
			if (this.characters.IsCreated)
			{
				this.characters.Dispose();
			}
		}

		// Token: 0x0400017A RID: 378
		public NativeArray<SDFCharacter> characters;

		// Token: 0x0400017B RID: 379
		private Dictionary<char, int> lookup;

		// Token: 0x0400017C RID: 380
		public Material material;

		// Token: 0x0400017D RID: 381
		public const ushort Newline = 65535;
	}
}
