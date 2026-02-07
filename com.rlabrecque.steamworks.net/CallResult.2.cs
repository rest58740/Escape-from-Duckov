using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000189 RID: 393
	public sealed class CallResult<T> : CallResult, IDisposable
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060008F3 RID: 2291 RVA: 0x0000D388 File Offset: 0x0000B588
		// (remove) Token: 0x060008F4 RID: 2292 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		private event CallResult<T>.APIDispatchDelegate m_Func;

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0000D3F5 File Offset: 0x0000B5F5
		public SteamAPICall_t Handle
		{
			get
			{
				return this.m_hAPICall;
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0000D3FD File Offset: 0x0000B5FD
		public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
		{
			return new CallResult<T>(func);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0000D405 File Offset: 0x0000B605
		public CallResult(CallResult<T>.APIDispatchDelegate func = null)
		{
			this.m_Func = func;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0000D420 File Offset: 0x0000B620
		~CallResult()
		{
			this.Dispose();
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0000D44C File Offset: 0x0000B64C
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			this.Cancel();
			this.m_bDisposed = true;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0000D46C File Offset: 0x0000B66C
		public void Set(SteamAPICall_t hAPICall, CallResult<T>.APIDispatchDelegate func = null)
		{
			if (func != null)
			{
				this.m_Func = func;
			}
			if (this.m_Func == null)
			{
				throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or via Set()");
			}
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				CallbackDispatcher.Unregister(this.m_hAPICall, this);
			}
			this.m_hAPICall = hAPICall;
			if (hAPICall != SteamAPICall_t.Invalid)
			{
				CallbackDispatcher.Register(hAPICall, this);
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0000D4CF File Offset: 0x0000B6CF
		public bool IsActive()
		{
			return this.m_hAPICall != SteamAPICall_t.Invalid;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0000D4E1 File Offset: 0x0000B6E1
		public void Cancel()
		{
			if (this.IsActive())
			{
				CallbackDispatcher.Unregister(this.m_hAPICall, this);
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0000D4F7 File Offset: 0x0000B6F7
		internal override Type GetCallbackType()
		{
			return typeof(T);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0000D504 File Offset: 0x0000B704
		internal override void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall_)
		{
			if ((SteamAPICall_t)hSteamAPICall_ == this.m_hAPICall)
			{
				try
				{
					this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))), bFailed);
				}
				catch (Exception e)
				{
					CallbackDispatcher.ExceptionHandler(e);
				}
			}
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0000D560 File Offset: 0x0000B760
		internal override void SetUnregistered()
		{
			this.m_hAPICall = SteamAPICall_t.Invalid;
		}

		// Token: 0x04000A6F RID: 2671
		private SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;

		// Token: 0x04000A70 RID: 2672
		private bool m_bDisposed;

		// Token: 0x020001D9 RID: 473
		// (Invoke) Token: 0x06000BC7 RID: 3015
		public delegate void APIDispatchDelegate(T param, bool bIOFailure);
	}
}
