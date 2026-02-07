using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Steamworks
{
	// Token: 0x02000185 RID: 389
	public static class CallbackDispatcher
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x0000CA68 File Offset: 0x0000AC68
		public static void ExceptionHandler(Exception e)
		{
			Debug.LogException(e);
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0000CA70 File Offset: 0x0000AC70
		public static bool IsInitialized
		{
			get
			{
				return CallbackDispatcher.m_initCount > 0;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0000CA7C File Offset: 0x0000AC7C
		internal static void Initialize()
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				if (CallbackDispatcher.m_initCount == 0)
				{
					NativeMethods.SteamAPI_ManualDispatch_Init();
					CallbackDispatcher.m_pCallbackMsg = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CallbackMsg_t)));
				}
				CallbackDispatcher.m_initCount++;
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
		internal static void Shutdown()
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				CallbackDispatcher.m_initCount--;
				if (CallbackDispatcher.m_initCount == 0)
				{
					CallbackDispatcher.UnregisterAll();
					Marshal.FreeHGlobal(CallbackDispatcher.m_pCallbackMsg);
					CallbackDispatcher.m_pCallbackMsg = IntPtr.Zero;
				}
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0000CB50 File Offset: 0x0000AD50
		internal static void Register(Callback cb)
		{
			int callbackIdentity = CallbackIdentities.GetCallbackIdentity(cb.GetCallbackType());
			Dictionary<int, List<Callback>> dictionary = cb.IsGameServer ? CallbackDispatcher.m_registeredGameServerCallbacks : CallbackDispatcher.m_registeredCallbacks;
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<Callback> list;
				if (!dictionary.TryGetValue(callbackIdentity, out list))
				{
					list = new List<Callback>();
					dictionary.Add(callbackIdentity, list);
				}
				list.Add(cb);
			}
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000CBD0 File Offset: 0x0000ADD0
		internal static void Register(SteamAPICall_t asyncCall, CallResult cr)
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<CallResult> list;
				if (!CallbackDispatcher.m_registeredCallResults.TryGetValue((ulong)asyncCall, out list))
				{
					list = new List<CallResult>();
					CallbackDispatcher.m_registeredCallResults.Add((ulong)asyncCall, list);
				}
				list.Add(cr);
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		internal static void Unregister(Callback cb)
		{
			int callbackIdentity = CallbackIdentities.GetCallbackIdentity(cb.GetCallbackType());
			Dictionary<int, List<Callback>> dictionary = cb.IsGameServer ? CallbackDispatcher.m_registeredGameServerCallbacks : CallbackDispatcher.m_registeredCallbacks;
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<Callback> list;
				if (dictionary.TryGetValue(callbackIdentity, out list))
				{
					list.Remove(cb);
					if (list.Count == 0)
					{
						dictionary.Remove(callbackIdentity);
					}
				}
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000CCBC File Offset: 0x0000AEBC
		internal static void Unregister(SteamAPICall_t asyncCall, CallResult cr)
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<CallResult> list;
				if (CallbackDispatcher.m_registeredCallResults.TryGetValue((ulong)asyncCall, out list))
				{
					list.Remove(cr);
					if (list.Count == 0)
					{
						CallbackDispatcher.m_registeredCallResults.Remove((ulong)asyncCall);
					}
				}
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0000CD2C File Offset: 0x0000AF2C
		private static void UnregisterAll()
		{
			List<Callback> list = new List<Callback>();
			List<CallResult> list2 = new List<CallResult>();
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				foreach (KeyValuePair<int, List<Callback>> keyValuePair in CallbackDispatcher.m_registeredCallbacks)
				{
					list.AddRange(keyValuePair.Value);
				}
				CallbackDispatcher.m_registeredCallbacks.Clear();
				foreach (KeyValuePair<int, List<Callback>> keyValuePair2 in CallbackDispatcher.m_registeredGameServerCallbacks)
				{
					list.AddRange(keyValuePair2.Value);
				}
				CallbackDispatcher.m_registeredGameServerCallbacks.Clear();
				foreach (KeyValuePair<ulong, List<CallResult>> keyValuePair3 in CallbackDispatcher.m_registeredCallResults)
				{
					list2.AddRange(keyValuePair3.Value);
				}
				CallbackDispatcher.m_registeredCallResults.Clear();
				foreach (Callback callback in list)
				{
					callback.SetUnregistered();
				}
				foreach (CallResult callResult in list2)
				{
					callResult.SetUnregistered();
				}
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0000CF28 File Offset: 0x0000B128
		internal static void RunFrame(bool isGameServer)
		{
			if (!CallbackDispatcher.IsInitialized)
			{
				throw new InvalidOperationException("Callback dispatcher is not initialized.");
			}
			HSteamPipe hSteamPipe = (HSteamPipe)(isGameServer ? NativeMethods.SteamGameServer_GetHSteamPipe() : NativeMethods.SteamAPI_GetHSteamPipe());
			NativeMethods.SteamAPI_ManualDispatch_RunFrame(hSteamPipe);
			Dictionary<int, List<Callback>> dictionary = isGameServer ? CallbackDispatcher.m_registeredGameServerCallbacks : CallbackDispatcher.m_registeredCallbacks;
			while (NativeMethods.SteamAPI_ManualDispatch_GetNextCallback(hSteamPipe, CallbackDispatcher.m_pCallbackMsg))
			{
				CallbackMsg_t callbackMsg_t = (CallbackMsg_t)Marshal.PtrToStructure(CallbackDispatcher.m_pCallbackMsg, typeof(CallbackMsg_t));
				try
				{
					if (callbackMsg_t.m_iCallback == 703)
					{
						SteamAPICallCompleted_t steamAPICallCompleted_t = (SteamAPICallCompleted_t)Marshal.PtrToStructure(callbackMsg_t.m_pubParam, typeof(SteamAPICallCompleted_t));
						IntPtr intPtr = Marshal.AllocHGlobal((int)steamAPICallCompleted_t.m_cubParam);
						bool bFailed;
						if (NativeMethods.SteamAPI_ManualDispatch_GetAPICallResult(hSteamPipe, steamAPICallCompleted_t.m_hAsyncCall, intPtr, (int)steamAPICallCompleted_t.m_cubParam, steamAPICallCompleted_t.m_iCallback, out bFailed))
						{
							object sync = CallbackDispatcher.m_sync;
							lock (sync)
							{
								List<CallResult> list;
								if (CallbackDispatcher.m_registeredCallResults.TryGetValue((ulong)steamAPICallCompleted_t.m_hAsyncCall, out list))
								{
									CallbackDispatcher.m_registeredCallResults.Remove((ulong)steamAPICallCompleted_t.m_hAsyncCall);
									foreach (CallResult callResult in list)
									{
										callResult.OnRunCallResult(intPtr, bFailed, (ulong)steamAPICallCompleted_t.m_hAsyncCall);
										callResult.SetUnregistered();
									}
								}
							}
						}
						Marshal.FreeHGlobal(intPtr);
					}
					else
					{
						List<Callback> list2 = null;
						object sync = CallbackDispatcher.m_sync;
						lock (sync)
						{
							List<Callback> collection = null;
							if (dictionary.TryGetValue(callbackMsg_t.m_iCallback, out collection))
							{
								list2 = new List<Callback>(collection);
							}
						}
						if (list2 != null)
						{
							foreach (Callback callback in list2)
							{
								callback.OnRunCallback(callbackMsg_t.m_pubParam);
							}
						}
					}
				}
				catch (Exception e)
				{
					CallbackDispatcher.ExceptionHandler(e);
				}
				finally
				{
					NativeMethods.SteamAPI_ManualDispatch_FreeLastCallback(hSteamPipe);
				}
			}
		}

		// Token: 0x04000A64 RID: 2660
		private static Dictionary<int, List<Callback>> m_registeredCallbacks = new Dictionary<int, List<Callback>>();

		// Token: 0x04000A65 RID: 2661
		private static Dictionary<int, List<Callback>> m_registeredGameServerCallbacks = new Dictionary<int, List<Callback>>();

		// Token: 0x04000A66 RID: 2662
		private static Dictionary<ulong, List<CallResult>> m_registeredCallResults = new Dictionary<ulong, List<CallResult>>();

		// Token: 0x04000A67 RID: 2663
		private static object m_sync = new object();

		// Token: 0x04000A68 RID: 2664
		private static IntPtr m_pCallbackMsg;

		// Token: 0x04000A69 RID: 2665
		private static int m_initCount;
	}
}
