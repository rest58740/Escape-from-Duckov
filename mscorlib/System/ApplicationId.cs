using System;
using System.Text;

namespace System
{
	// Token: 0x020001C8 RID: 456
	[Serializable]
	public sealed class ApplicationId
	{
		// Token: 0x060013A9 RID: 5033 RVA: 0x0004E4EC File Offset: 0x0004C6EC
		public ApplicationId(byte[] publicKeyToken, string name, Version version, string processorArchitecture, string culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("ApplicationId cannot have an empty string for the name.");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (publicKeyToken == null)
			{
				throw new ArgumentNullException("publicKeyToken");
			}
			this._publicKeyToken = (byte[])publicKeyToken.Clone();
			this.Name = name;
			this.Version = version;
			this.ProcessorArchitecture = processorArchitecture;
			this.Culture = culture;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x0004E571 File Offset: 0x0004C771
		public string Culture { get; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x0004E579 File Offset: 0x0004C779
		public string Name { get; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x0004E581 File Offset: 0x0004C781
		public string ProcessorArchitecture { get; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x0004E589 File Offset: 0x0004C789
		public Version Version { get; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x0004E591 File Offset: 0x0004C791
		public byte[] PublicKeyToken
		{
			get
			{
				return (byte[])this._publicKeyToken.Clone();
			}
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0004E5A3 File Offset: 0x0004C7A3
		public ApplicationId Copy()
		{
			return new ApplicationId(this._publicKeyToken, this.Name, this.Version, this.ProcessorArchitecture, this.Culture);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0004E5C8 File Offset: 0x0004C7C8
		public override string ToString()
		{
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			stringBuilder.Append(this.Name);
			if (this.Culture != null)
			{
				stringBuilder.Append(", culture=\"");
				stringBuilder.Append(this.Culture);
				stringBuilder.Append('"');
			}
			stringBuilder.Append(", version=\"");
			stringBuilder.Append(this.Version.ToString());
			stringBuilder.Append('"');
			if (this._publicKeyToken != null)
			{
				stringBuilder.Append(", publicKeyToken=\"");
				stringBuilder.Append(ApplicationId.EncodeHexString(this._publicKeyToken));
				stringBuilder.Append('"');
			}
			if (this.ProcessorArchitecture != null)
			{
				stringBuilder.Append(", processorArchitecture =\"");
				stringBuilder.Append(this.ProcessorArchitecture);
				stringBuilder.Append('"');
			}
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x0004E69A File Offset: 0x0004C89A
		private static char HexDigit(int num)
		{
			return (char)((num < 10) ? (num + 48) : (num + 55));
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0004E6AC File Offset: 0x0004C8AC
		private static string EncodeHexString(byte[] sArray)
		{
			string result = null;
			if (sArray != null)
			{
				char[] array = new char[sArray.Length * 2];
				int i = 0;
				int num = 0;
				while (i < sArray.Length)
				{
					int num2 = (sArray[i] & 240) >> 4;
					array[num++] = ApplicationId.HexDigit(num2);
					num2 = (int)(sArray[i] & 15);
					array[num++] = ApplicationId.HexDigit(num2);
					i++;
				}
				result = new string(array);
			}
			return result;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0004E714 File Offset: 0x0004C914
		public override bool Equals(object o)
		{
			ApplicationId applicationId = o as ApplicationId;
			if (applicationId == null)
			{
				return false;
			}
			if (!object.Equals(this.Name, applicationId.Name) || !object.Equals(this.Version, applicationId.Version) || !object.Equals(this.ProcessorArchitecture, applicationId.ProcessorArchitecture) || !object.Equals(this.Culture, applicationId.Culture))
			{
				return false;
			}
			if (this._publicKeyToken.Length != applicationId._publicKeyToken.Length)
			{
				return false;
			}
			for (int i = 0; i < this._publicKeyToken.Length; i++)
			{
				if (this._publicKeyToken[i] != applicationId._publicKeyToken[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0004E7B7 File Offset: 0x0004C9B7
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() ^ this.Version.GetHashCode();
		}

		// Token: 0x04001447 RID: 5191
		private readonly byte[] _publicKeyToken;
	}
}
