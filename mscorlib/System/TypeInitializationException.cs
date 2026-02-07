using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020001A4 RID: 420
	[Serializable]
	public sealed class TypeInitializationException : SystemException
	{
		// Token: 0x060011E9 RID: 4585 RVA: 0x00047C84 File Offset: 0x00045E84
		private TypeInitializationException() : base("Type constructor threw an exception.")
		{
			base.HResult = -2146233036;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00047C9C File Offset: 0x00045E9C
		public TypeInitializationException(string fullTypeName, Exception innerException) : this(fullTypeName, SR.Format("The type initializer for '{0}' threw an exception.", fullTypeName), innerException)
		{
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00047CB1 File Offset: 0x00045EB1
		internal TypeInitializationException(string message) : base(message)
		{
			base.HResult = -2146233036;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00047CC5 File Offset: 0x00045EC5
		internal TypeInitializationException(string fullTypeName, string message, Exception innerException) : base(message, innerException)
		{
			this._typeName = fullTypeName;
			base.HResult = -2146233036;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00047CE1 File Offset: 0x00045EE1
		internal TypeInitializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._typeName = info.GetString("TypeName");
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00047CFC File Offset: 0x00045EFC
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("TypeName", this.TypeName, typeof(string));
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x00047D21 File Offset: 0x00045F21
		public string TypeName
		{
			get
			{
				if (this._typeName == null)
				{
					return string.Empty;
				}
				return this._typeName;
			}
		}

		// Token: 0x0400135A RID: 4954
		private string _typeName;
	}
}
