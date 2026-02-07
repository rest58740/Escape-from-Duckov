using System;
using System.Numerics;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200002E RID: 46
	[Preserve]
	[ES3Properties(new string[]
	{
		"bytes"
	})]
	public class ES3Type_BigInteger : ES3Type
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00009610 File Offset: 0x00007810
		public ES3Type_BigInteger() : base(typeof(BigInteger))
		{
			ES3Type_BigInteger.Instance = this;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009628 File Offset: 0x00007828
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WriteProperty("bytes", ((BigInteger)obj).ToByteArray(), ES3Type_byteArray.Instance);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009653 File Offset: 0x00007853
		public override object Read<T>(ES3Reader reader)
		{
			return new BigInteger(reader.ReadProperty<byte[]>(ES3Type_byteArray.Instance));
		}

		// Token: 0x04000072 RID: 114
		public static ES3Type Instance;
	}
}
