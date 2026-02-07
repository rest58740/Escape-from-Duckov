using System;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System.Reflection
{
	// Token: 0x020008BE RID: 2238
	[Serializable]
	public sealed class ReflectionTypeLoadException : SystemException, ISerializable
	{
		// Token: 0x06004A0C RID: 18956 RVA: 0x000EF555 File Offset: 0x000ED755
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions) : base(null)
		{
			this.Types = classes;
			this.LoaderExceptions = exceptions;
			base.HResult = -2146232830;
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x000EF577 File Offset: 0x000ED777
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions, string message) : base(message)
		{
			this.Types = classes;
			this.LoaderExceptions = exceptions;
			base.HResult = -2146232830;
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x000EF599 File Offset: 0x000ED799
		private ReflectionTypeLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.LoaderExceptions = (Exception[])info.GetValue("Exceptions", typeof(Exception[]));
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x000EF5C3 File Offset: 0x000ED7C3
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Types", null, typeof(Type[]));
			info.AddValue("Exceptions", this.LoaderExceptions, typeof(Exception[]));
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004A10 RID: 18960 RVA: 0x000EF5FE File Offset: 0x000ED7FE
		public Type[] Types { get; }

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004A11 RID: 18961 RVA: 0x000EF606 File Offset: 0x000ED806
		public Exception[] LoaderExceptions { get; }

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004A12 RID: 18962 RVA: 0x000EF60E File Offset: 0x000ED80E
		public override string Message
		{
			get
			{
				return this.CreateString(true);
			}
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x000EF617 File Offset: 0x000ED817
		public override string ToString()
		{
			return this.CreateString(false);
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x000EF620 File Offset: 0x000ED820
		private string CreateString(bool isMessage)
		{
			string text = isMessage ? base.Message : base.ToString();
			Exception[] loaderExceptions = this.LoaderExceptions;
			if (loaderExceptions == null || loaderExceptions.Length == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text);
			foreach (Exception ex in loaderExceptions)
			{
				if (ex != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(isMessage ? ex.Message : ex.ToString());
				}
			}
			return stringBuilder.ToString();
		}
	}
}
