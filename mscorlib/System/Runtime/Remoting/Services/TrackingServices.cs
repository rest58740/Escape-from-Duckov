using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x0200057D RID: 1405
	[ComVisible(true)]
	public class TrackingServices
	{
		// Token: 0x0600370F RID: 14095 RVA: 0x000C6A84 File Offset: 0x000C4C84
		public static void RegisterTrackingHandler(ITrackingHandler handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			object syncRoot = TrackingServices._handlers.SyncRoot;
			lock (syncRoot)
			{
				if (-1 != TrackingServices._handlers.IndexOf(handler))
				{
					throw new RemotingException("handler already registered");
				}
				TrackingServices._handlers.Add(handler);
			}
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x000C6AF8 File Offset: 0x000C4CF8
		public static void UnregisterTrackingHandler(ITrackingHandler handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			object syncRoot = TrackingServices._handlers.SyncRoot;
			lock (syncRoot)
			{
				int num = TrackingServices._handlers.IndexOf(handler);
				if (num == -1)
				{
					throw new RemotingException("handler is not registered");
				}
				TrackingServices._handlers.RemoveAt(num);
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06003711 RID: 14097 RVA: 0x000C6B6C File Offset: 0x000C4D6C
		public static ITrackingHandler[] RegisteredHandlers
		{
			get
			{
				object syncRoot = TrackingServices._handlers.SyncRoot;
				ITrackingHandler[] result;
				lock (syncRoot)
				{
					if (TrackingServices._handlers.Count == 0)
					{
						result = new ITrackingHandler[0];
					}
					else
					{
						result = (ITrackingHandler[])TrackingServices._handlers.ToArray(typeof(ITrackingHandler));
					}
				}
				return result;
			}
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x000C6BDC File Offset: 0x000C4DDC
		internal static void NotifyMarshaledObject(object obj, ObjRef or)
		{
			object syncRoot = TrackingServices._handlers.SyncRoot;
			ITrackingHandler[] array;
			lock (syncRoot)
			{
				if (TrackingServices._handlers.Count == 0)
				{
					return;
				}
				array = (ITrackingHandler[])TrackingServices._handlers.ToArray(typeof(ITrackingHandler));
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i].MarshaledObject(obj, or);
			}
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000C6C5C File Offset: 0x000C4E5C
		internal static void NotifyUnmarshaledObject(object obj, ObjRef or)
		{
			object syncRoot = TrackingServices._handlers.SyncRoot;
			ITrackingHandler[] array;
			lock (syncRoot)
			{
				if (TrackingServices._handlers.Count == 0)
				{
					return;
				}
				array = (ITrackingHandler[])TrackingServices._handlers.ToArray(typeof(ITrackingHandler));
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UnmarshaledObject(obj, or);
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000C6CDC File Offset: 0x000C4EDC
		internal static void NotifyDisconnectedObject(object obj)
		{
			object syncRoot = TrackingServices._handlers.SyncRoot;
			ITrackingHandler[] array;
			lock (syncRoot)
			{
				if (TrackingServices._handlers.Count == 0)
				{
					return;
				}
				array = (ITrackingHandler[])TrackingServices._handlers.ToArray(typeof(ITrackingHandler));
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DisconnectedObject(obj);
			}
		}

		// Token: 0x04002576 RID: 9590
		private static ArrayList _handlers = new ArrayList();
	}
}
