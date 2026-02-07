using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E9 RID: 1513
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapHexBinary : ISoapXsd
	{
		// Token: 0x06003966 RID: 14694 RVA: 0x000CB9B8 File Offset: 0x000C9BB8
		public SoapHexBinary()
		{
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x000CB9CB File Offset: 0x000C9BCB
		public SoapHexBinary(byte[] value)
		{
			this._value = value;
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06003968 RID: 14696 RVA: 0x000CB9E5 File Offset: 0x000C9BE5
		// (set) Token: 0x06003969 RID: 14697 RVA: 0x000CB9ED File Offset: 0x000C9BED
		public byte[] Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x000CB9F6 File Offset: 0x000C9BF6
		public static string XsdType
		{
			get
			{
				return "hexBinary";
			}
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x000CB9FD File Offset: 0x000C9BFD
		public string GetXsdType()
		{
			return SoapHexBinary.XsdType;
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x000CBA04 File Offset: 0x000C9C04
		public static SoapHexBinary Parse(string value)
		{
			return new SoapHexBinary(SoapHexBinary.FromBinHexString(value));
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x000CBA14 File Offset: 0x000C9C14
		internal static byte[] FromBinHexString(string value)
		{
			char[] array = value.ToCharArray();
			byte[] array2 = new byte[array.Length / 2 + array.Length % 2];
			int num = array.Length;
			if (num % 2 != 0)
			{
				throw SoapHexBinary.CreateInvalidValueException(value);
			}
			int num2 = 0;
			for (int i = 0; i < num - 1; i += 2)
			{
				array2[num2] = SoapHexBinary.FromHex(array[i], value);
				byte[] array3 = array2;
				int num3 = num2;
				array3[num3] = (byte)(array3[num3] << 4);
				byte[] array4 = array2;
				int num4 = num2;
				array4[num4] += SoapHexBinary.FromHex(array[i + 1], value);
				num2++;
			}
			return array2;
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x000CBA94 File Offset: 0x000C9C94
		private static byte FromHex(char hexDigit, string value)
		{
			byte result;
			try
			{
				result = byte.Parse(hexDigit.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				throw SoapHexBinary.CreateInvalidValueException(value);
			}
			return result;
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x000CBAD4 File Offset: 0x000C9CD4
		private static Exception CreateInvalidValueException(string value)
		{
			return new RemotingException(string.Format(CultureInfo.InvariantCulture, "Invalid value '{0}' for xsd:{1}.", value, SoapHexBinary.XsdType));
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x000CBAF0 File Offset: 0x000C9CF0
		public override string ToString()
		{
			this.sb.Length = 0;
			foreach (byte b in this._value)
			{
				this.sb.Append(b.ToString("X2"));
			}
			return this.sb.ToString();
		}

		// Token: 0x04002629 RID: 9769
		private byte[] _value;

		// Token: 0x0400262A RID: 9770
		private StringBuilder sb = new StringBuilder();
	}
}
