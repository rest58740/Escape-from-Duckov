using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Drawing.Text
{
	// Token: 0x0200005E RID: 94
	internal struct SDFLookupData
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
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

		// Token: 0x060002AD RID: 685 RVA: 0x0000F8E4 File Offset: 0x0000DAE4
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

		// Token: 0x060002AE RID: 686 RVA: 0x0000F91B File Offset: 0x0000DB1B
		public void Dispose()
		{
			if (this.characters.IsCreated)
			{
				this.characters.Dispose();
			}
		}

		// Token: 0x04000185 RID: 389
		public NativeArray<SDFCharacter> characters;

		// Token: 0x04000186 RID: 390
		private Dictionary<char, int> lookup;

		// Token: 0x04000187 RID: 391
		public Material material;

		// Token: 0x04000188 RID: 392
		public const ushort Newline = 65535;
	}
}
