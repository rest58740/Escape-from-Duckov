using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000618 RID: 1560
	[ComVisible(true)]
	[Serializable]
	public class Header
	{
		// Token: 0x06003AEC RID: 15084 RVA: 0x000CE331 File Offset: 0x000CC531
		public Header(string _Name, object _Value) : this(_Name, _Value, true)
		{
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x000CE33C File Offset: 0x000CC53C
		public Header(string _Name, object _Value, bool _MustUnderstand) : this(_Name, _Value, _MustUnderstand, null)
		{
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x000CE348 File Offset: 0x000CC548
		public Header(string _Name, object _Value, bool _MustUnderstand, string _HeaderNamespace)
		{
			this.Name = _Name;
			this.Value = _Value;
			this.MustUnderstand = _MustUnderstand;
			this.HeaderNamespace = _HeaderNamespace;
		}

		// Token: 0x0400268E RID: 9870
		public string HeaderNamespace;

		// Token: 0x0400268F RID: 9871
		public bool MustUnderstand;

		// Token: 0x04002690 RID: 9872
		public string Name;

		// Token: 0x04002691 RID: 9873
		public object Value;
	}
}
