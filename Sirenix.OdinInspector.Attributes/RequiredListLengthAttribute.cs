using System;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200005C RID: 92
	public sealed class RequiredListLengthAttribute : Attribute
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000361B File Offset: 0x0000181B
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00003623 File Offset: 0x00001823
		public int MinLength
		{
			get
			{
				return this.minLength;
			}
			set
			{
				this.minLength = value;
				this.minLengthIsSet = true;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00003633 File Offset: 0x00001833
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000363B File Offset: 0x0000183B
		public int MaxLength
		{
			get
			{
				return this.maxLength;
			}
			set
			{
				this.maxLength = value;
				this.maxLengthIsSet = true;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000364B File Offset: 0x0000184B
		public bool MinLengthIsSet
		{
			get
			{
				return this.minLengthIsSet;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00003653 File Offset: 0x00001853
		public bool MaxLengthIsSet
		{
			get
			{
				return this.maxLengthIsSet;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000365B File Offset: 0x0000185B
		public bool PrefabKindIsSet
		{
			get
			{
				return this.prefabKindIsSet;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00003663 File Offset: 0x00001863
		// (set) Token: 0x06000140 RID: 320 RVA: 0x0000366B File Offset: 0x0000186B
		public PrefabKind PrefabKind
		{
			get
			{
				return this.prefabKind;
			}
			set
			{
				this.prefabKind = value;
				this.prefabKindIsSet = true;
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00002102 File Offset: 0x00000302
		public RequiredListLengthAttribute()
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000367B File Offset: 0x0000187B
		public RequiredListLengthAttribute(int fixedLength)
		{
			this.MinLength = fixedLength;
			this.MaxLength = fixedLength;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00003691 File Offset: 0x00001891
		public RequiredListLengthAttribute(int minLength, int maxLength)
		{
			this.MinLength = minLength;
			this.MaxLength = maxLength;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000036A7 File Offset: 0x000018A7
		public RequiredListLengthAttribute(int minLength, string maxLengthGetter)
		{
			this.MinLength = minLength;
			this.MaxLengthGetter = maxLengthGetter;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000036BD File Offset: 0x000018BD
		public RequiredListLengthAttribute(string fixedLengthGetter)
		{
			this.MinLengthGetter = fixedLengthGetter;
			this.MaxLengthGetter = fixedLengthGetter;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000036D3 File Offset: 0x000018D3
		public RequiredListLengthAttribute(string minLengthGetter, string maxLengthGetter)
		{
			this.MinLengthGetter = minLengthGetter;
			this.MaxLengthGetter = maxLengthGetter;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000036E9 File Offset: 0x000018E9
		public RequiredListLengthAttribute(string minLengthGetter, int maxLength)
		{
			this.MinLengthGetter = minLengthGetter;
			this.MaxLength = maxLength;
		}

		// Token: 0x040000FC RID: 252
		private PrefabKind prefabKind;

		// Token: 0x040000FD RID: 253
		private bool prefabKindIsSet;

		// Token: 0x040000FE RID: 254
		private int minLength;

		// Token: 0x040000FF RID: 255
		private int maxLength;

		// Token: 0x04000100 RID: 256
		private bool minLengthIsSet;

		// Token: 0x04000101 RID: 257
		private bool maxLengthIsSet;

		// Token: 0x04000102 RID: 258
		public string MinLengthGetter;

		// Token: 0x04000103 RID: 259
		public string MaxLengthGetter;
	}
}
