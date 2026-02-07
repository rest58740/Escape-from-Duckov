using System;
using System.Runtime.Serialization;
using System.Security;
using Unity;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000803 RID: 2051
	[Serializable]
	public sealed class RuntimeWrappedException : Exception
	{
		// Token: 0x0600461A RID: 17946 RVA: 0x000E58BA File Offset: 0x000E3ABA
		public RuntimeWrappedException(object thrownObject) : base("An object that does not derive from System.Exception has been wrapped in a RuntimeWrappedException.")
		{
			base.HResult = -2146233026;
			this._wrappedException = thrownObject;
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x000E58D9 File Offset: 0x000E3AD9
		private RuntimeWrappedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._wrappedException = info.GetValue("WrappedException", typeof(object));
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x000E58FE File Offset: 0x000E3AFE
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("WrappedException", this._wrappedException, typeof(object));
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x0600461D RID: 17949 RVA: 0x000E5923 File Offset: 0x000E3B23
		public object WrappedException
		{
			get
			{
				return this._wrappedException;
			}
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x000173AD File Offset: 0x000155AD
		internal RuntimeWrappedException()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002D40 RID: 11584
		private object _wrappedException;
	}
}
