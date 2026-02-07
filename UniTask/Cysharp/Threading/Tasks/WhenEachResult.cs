using System;
using System.Runtime.ExceptionServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000045 RID: 69
	public readonly struct WhenEachResult<T>
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000069A5 File Offset: 0x00004BA5
		public T Result { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000069AD File Offset: 0x00004BAD
		public Exception Exception { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000069B5 File Offset: 0x00004BB5
		public bool IsCompletedSuccessfully
		{
			get
			{
				return this.Exception == null;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000069C0 File Offset: 0x00004BC0
		public bool IsFaulted
		{
			get
			{
				return this.Exception != null;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000069CB File Offset: 0x00004BCB
		public WhenEachResult(T result)
		{
			this.Result = result;
			this.Exception = null;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000069DB File Offset: 0x00004BDB
		public WhenEachResult(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this.Result = default(T);
			this.Exception = exception;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000069FE File Offset: 0x00004BFE
		public void TryThrow()
		{
			if (this.IsFaulted)
			{
				ExceptionDispatchInfo.Capture(this.Exception).Throw();
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006A18 File Offset: 0x00004C18
		public T GetResult()
		{
			if (this.IsFaulted)
			{
				ExceptionDispatchInfo.Capture(this.Exception).Throw();
			}
			return this.Result;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006A38 File Offset: 0x00004C38
		public override string ToString()
		{
			if (this.IsCompletedSuccessfully)
			{
				T result = this.Result;
				return ((result != null) ? result.ToString() : null) ?? "";
			}
			return "Exception{" + this.Exception.Message + "}";
		}
	}
}
