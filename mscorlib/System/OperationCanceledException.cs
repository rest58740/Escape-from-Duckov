using System;
using System.Runtime.Serialization;
using System.Threading;

namespace System
{
	// Token: 0x0200016A RID: 362
	[Serializable]
	public class OperationCanceledException : SystemException
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0003AD8F File Offset: 0x00038F8F
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x0003AD97 File Offset: 0x00038F97
		public CancellationToken CancellationToken
		{
			get
			{
				return this._cancellationToken;
			}
			private set
			{
				this._cancellationToken = value;
			}
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0003ADA0 File Offset: 0x00038FA0
		public OperationCanceledException() : base("The operation was canceled.")
		{
			base.HResult = -2146233029;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0003ADB8 File Offset: 0x00038FB8
		public OperationCanceledException(string message) : base(message)
		{
			base.HResult = -2146233029;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0003ADCC File Offset: 0x00038FCC
		public OperationCanceledException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233029;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0003ADE1 File Offset: 0x00038FE1
		public OperationCanceledException(CancellationToken token) : this()
		{
			this.CancellationToken = token;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0003ADF0 File Offset: 0x00038FF0
		public OperationCanceledException(string message, CancellationToken token) : this(message)
		{
			this.CancellationToken = token;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0003AE00 File Offset: 0x00039000
		public OperationCanceledException(string message, Exception innerException, CancellationToken token) : this(message, innerException)
		{
			this.CancellationToken = token;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected OperationCanceledException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040012A7 RID: 4775
		[NonSerialized]
		private CancellationToken _cancellationToken;
	}
}
