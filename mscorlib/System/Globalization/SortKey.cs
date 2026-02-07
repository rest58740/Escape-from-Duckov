using System;
using System.Runtime.InteropServices;
using Unity;

namespace System.Globalization
{
	// Token: 0x0200099D RID: 2461
	[ComVisible(true)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class SortKey
	{
		// Token: 0x06005899 RID: 22681 RVA: 0x0012AA80 File Offset: 0x00128C80
		public static int Compare(SortKey sortkey1, SortKey sortkey2)
		{
			if (sortkey1 == null)
			{
				throw new ArgumentNullException("sortkey1");
			}
			if (sortkey2 == null)
			{
				throw new ArgumentNullException("sortkey2");
			}
			if (sortkey1 == sortkey2 || sortkey1.OriginalString == sortkey2.OriginalString)
			{
				return 0;
			}
			byte[] keyData = sortkey1.KeyData;
			byte[] keyData2 = sortkey2.KeyData;
			int num = (keyData.Length > keyData2.Length) ? keyData2.Length : keyData.Length;
			int i = 0;
			while (i < num)
			{
				if (keyData[i] != keyData2[i])
				{
					if (keyData[i] >= keyData2[i])
					{
						return 1;
					}
					return -1;
				}
				else
				{
					i++;
				}
			}
			if (keyData.Length == keyData2.Length)
			{
				return 0;
			}
			if (keyData.Length >= keyData2.Length)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600589A RID: 22682 RVA: 0x0012AB14 File Offset: 0x00128D14
		internal SortKey(int lcid, string source, CompareOptions opt)
		{
			this.lcid = lcid;
			this.source = source;
			this.options = opt;
			int length = source.Length;
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (byte)source[i];
			}
			this.key = array;
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x0012AB68 File Offset: 0x00128D68
		internal SortKey(int lcid, string source, byte[] buffer, CompareOptions opt, int lv1Length, int lv2Length, int lv3Length, int kanaSmallLength, int markTypeLength, int katakanaLength, int kanaWidthLength, int identLength)
		{
			this.lcid = lcid;
			this.source = source;
			this.key = buffer;
			this.options = opt;
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x0012AB8D File Offset: 0x00128D8D
		internal SortKey(string localeName, string str, CompareOptions options, byte[] keyData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x0600589D RID: 22685 RVA: 0x0012AB9A File Offset: 0x00128D9A
		public virtual string OriginalString
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x0600589E RID: 22686 RVA: 0x0012ABA2 File Offset: 0x00128DA2
		public virtual byte[] KeyData
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x0012ABAC File Offset: 0x00128DAC
		public override bool Equals(object value)
		{
			SortKey sortKey = value as SortKey;
			return sortKey != null && this.lcid == sortKey.lcid && this.options == sortKey.options && SortKey.Compare(this, sortKey) == 0;
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x0012ABEC File Offset: 0x00128DEC
		public override int GetHashCode()
		{
			if (this.key.Length == 0)
			{
				return 0;
			}
			int num = (int)this.key[0];
			for (int i = 1; i < this.key.Length; i++)
			{
				num ^= (int)this.key[i] << (i & 3);
			}
			return num;
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x0012AC34 File Offset: 0x00128E34
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"SortKey - ",
				this.lcid.ToString(),
				", ",
				this.options.ToString(),
				", ",
				this.source
			});
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x000173AD File Offset: 0x000155AD
		internal SortKey()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040036C7 RID: 14023
		private readonly string source;

		// Token: 0x040036C8 RID: 14024
		private readonly byte[] key;

		// Token: 0x040036C9 RID: 14025
		private readonly CompareOptions options;

		// Token: 0x040036CA RID: 14026
		private readonly int lcid;
	}
}
