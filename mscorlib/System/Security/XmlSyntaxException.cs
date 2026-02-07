using System;
using System.Runtime.Serialization;

namespace System.Security
{
	// Token: 0x020003CC RID: 972
	[Serializable]
	public sealed class XmlSyntaxException : SystemException
	{
		// Token: 0x0600285A RID: 10330 RVA: 0x00092A55 File Offset: 0x00090C55
		public XmlSyntaxException()
		{
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00092A55 File Offset: 0x00090C55
		public XmlSyntaxException(int lineNumber)
		{
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x00092A55 File Offset: 0x00090C55
		public XmlSyntaxException(int lineNumber, string message)
		{
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x00092A55 File Offset: 0x00090C55
		public XmlSyntaxException(string message)
		{
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x00092A55 File Offset: 0x00090C55
		public XmlSyntaxException(string message, Exception inner)
		{
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x00020A69 File Offset: 0x0001EC69
		private XmlSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
