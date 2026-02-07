using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000187 RID: 391
	public sealed class Callback<T> : Callback, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060008E2 RID: 2274 RVA: 0x0000D1EC File Offset: 0x0000B3EC
		// (remove) Token: 0x060008E3 RID: 2275 RVA: 0x0000D224 File Offset: 0x0000B424
		private event Callback<T>.DispatchDelegate m_Func;

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000D259 File Offset: 0x0000B459
		public static Callback<T> Create(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, false);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0000D262 File Offset: 0x0000B462
		public static Callback<T> CreateGameServer(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, true);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0000D26B File Offset: 0x0000B46B
		public Callback(Callback<T>.DispatchDelegate func, bool bGameServer = false)
		{
			this.m_bGameServer = bGameServer;
			this.Register(func);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0000D284 File Offset: 0x0000B484
		~Callback()
		{
			this.Dispose();
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			if (this.m_bIsRegistered)
			{
				this.Unregister();
			}
			this.m_bDisposed = true;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0000D2D6 File Offset: 0x0000B4D6
		public void Register(Callback<T>.DispatchDelegate func)
		{
			if (func == null)
			{
				throw new Exception("Callback function must not be null.");
			}
			if (this.m_bIsRegistered)
			{
				this.Unregister();
			}
			this.m_Func = func;
			CallbackDispatcher.Register(this);
			this.m_bIsRegistered = true;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0000D308 File Offset: 0x0000B508
		public void Unregister()
		{
			CallbackDispatcher.Unregister(this);
			this.m_bIsRegistered = false;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0000D317 File Offset: 0x0000B517
		public override bool IsGameServer
		{
			get
			{
				return this.m_bGameServer;
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0000D31F File Offset: 0x0000B51F
		internal override Type GetCallbackType()
		{
			return typeof(T);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0000D32C File Offset: 0x0000B52C
		internal override void OnRunCallback(IntPtr pvParam)
		{
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))));
			}
			catch (Exception e)
			{
				CallbackDispatcher.ExceptionHandler(e);
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0000D374 File Offset: 0x0000B574
		internal override void SetUnregistered()
		{
			this.m_bIsRegistered = false;
		}

		// Token: 0x04000A6B RID: 2667
		private bool m_bGameServer;

		// Token: 0x04000A6C RID: 2668
		private bool m_bIsRegistered;

		// Token: 0x04000A6D RID: 2669
		private bool m_bDisposed;

		// Token: 0x020001D8 RID: 472
		// (Invoke) Token: 0x06000BC3 RID: 3011
		public delegate void DispatchDelegate(T param);
	}
}
