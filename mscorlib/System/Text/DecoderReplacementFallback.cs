using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x02000399 RID: 921
	[Serializable]
	public sealed class DecoderReplacementFallback : DecoderFallback, ISerializable
	{
		// Token: 0x060025D7 RID: 9687 RVA: 0x00086668 File Offset: 0x00084868
		public DecoderReplacementFallback() : this("?")
		{
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00086678 File Offset: 0x00084878
		internal DecoderReplacementFallback(SerializationInfo info, StreamingContext context)
		{
			try
			{
				this._strDefault = info.GetString("strDefault");
			}
			catch
			{
				this._strDefault = info.GetString("_strDefault");
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000866C4 File Offset: 0x000848C4
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("strDefault", this._strDefault);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000866D8 File Offset: 0x000848D8
		public DecoderReplacementFallback(string replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			bool flag = false;
			for (int i = 0; i < replacement.Length; i++)
			{
				if (char.IsSurrogate(replacement, i))
				{
					if (char.IsHighSurrogate(replacement, i))
					{
						if (flag)
						{
							break;
						}
						flag = true;
					}
					else
					{
						if (!flag)
						{
							flag = true;
							break;
						}
						flag = false;
					}
				}
				else if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				throw new ArgumentException(SR.Format("String contains invalid Unicode code points.", "replacement"));
			}
			this._strDefault = replacement;
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060025DB RID: 9691 RVA: 0x00086752 File Offset: 0x00084952
		public string DefaultString
		{
			get
			{
				return this._strDefault;
			}
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x0008675A File Offset: 0x0008495A
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new DecoderReplacementFallbackBuffer(this);
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x00086762 File Offset: 0x00084962
		public override int MaxCharCount
		{
			get
			{
				return this._strDefault.Length;
			}
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00086770 File Offset: 0x00084970
		public override bool Equals(object value)
		{
			DecoderReplacementFallback decoderReplacementFallback = value as DecoderReplacementFallback;
			return decoderReplacementFallback != null && this._strDefault == decoderReplacementFallback._strDefault;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x0008679A File Offset: 0x0008499A
		public override int GetHashCode()
		{
			return this._strDefault.GetHashCode();
		}

		// Token: 0x04001DA0 RID: 7584
		private string _strDefault;
	}
}
