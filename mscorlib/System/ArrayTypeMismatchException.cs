using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000F9 RID: 249
	[Serializable]
	public class ArrayTypeMismatchException : SystemException
	{
		// Token: 0x06000743 RID: 1859 RVA: 0x000216D7 File Offset: 0x0001F8D7
		public ArrayTypeMismatchException() : base("Attempted to access an element as a type incompatible with the array.")
		{
			base.HResult = -2146233085;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000216EF File Offset: 0x0001F8EF
		public ArrayTypeMismatchException(string message) : base(message)
		{
			base.HResult = -2146233085;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00021703 File Offset: 0x0001F903
		public ArrayTypeMismatchException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233085;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected ArrayTypeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
