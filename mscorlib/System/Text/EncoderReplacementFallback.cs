using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x020003A4 RID: 932
	[Serializable]
	public sealed class EncoderReplacementFallback : EncoderFallback, ISerializable
	{
		// Token: 0x06002638 RID: 9784 RVA: 0x000877E7 File Offset: 0x000859E7
		public EncoderReplacementFallback() : this("?")
		{
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000877F4 File Offset: 0x000859F4
		internal EncoderReplacementFallback(SerializationInfo info, StreamingContext context)
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

		// Token: 0x0600263A RID: 9786 RVA: 0x00087840 File Offset: 0x00085A40
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("strDefault", this._strDefault);
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x00087854 File Offset: 0x00085A54
		public EncoderReplacementFallback(string replacement)
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

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x000878CE File Offset: 0x00085ACE
		public string DefaultString
		{
			get
			{
				return this._strDefault;
			}
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000878D6 File Offset: 0x00085AD6
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new EncoderReplacementFallbackBuffer(this);
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600263E RID: 9790 RVA: 0x000878DE File Offset: 0x00085ADE
		public override int MaxCharCount
		{
			get
			{
				return this._strDefault.Length;
			}
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000878EC File Offset: 0x00085AEC
		public override bool Equals(object value)
		{
			EncoderReplacementFallback encoderReplacementFallback = value as EncoderReplacementFallback;
			return encoderReplacementFallback != null && this._strDefault == encoderReplacementFallback._strDefault;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x00087916 File Offset: 0x00085B16
		public override int GetHashCode()
		{
			return this._strDefault.GetHashCode();
		}

		// Token: 0x04001DC0 RID: 7616
		private string _strDefault;
	}
}
