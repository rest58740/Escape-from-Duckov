using System;
using System.Collections.Generic;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009F7 RID: 2551
	public class EventSource : IDisposable
	{
		// Token: 0x06005AD2 RID: 23250 RVA: 0x0013453D File Offset: 0x0013273D
		protected EventSource()
		{
			this.Name = base.GetType().Name;
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x0007B5A8 File Offset: 0x000797A8
		protected EventSource(bool throwOnEventWriteErrors) : this()
		{
		}

		// Token: 0x06005AD4 RID: 23252 RVA: 0x00134556 File Offset: 0x00132756
		protected EventSource(EventSourceSettings settings) : this()
		{
			this.Settings = settings;
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x00134565 File Offset: 0x00132765
		protected EventSource(EventSourceSettings settings, params string[] traits) : this(settings)
		{
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x0013456E File Offset: 0x0013276E
		public EventSource(string eventSourceName)
		{
			this.Name = eventSourceName;
		}

		// Token: 0x06005AD7 RID: 23255 RVA: 0x0013457D File Offset: 0x0013277D
		public EventSource(string eventSourceName, EventSourceSettings config) : this(eventSourceName)
		{
			this.Settings = config;
		}

		// Token: 0x06005AD8 RID: 23256 RVA: 0x0013458D File Offset: 0x0013278D
		public EventSource(string eventSourceName, EventSourceSettings config, params string[] traits) : this(eventSourceName, config)
		{
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x00134597 File Offset: 0x00132797
		internal EventSource(Guid eventSourceGuid, string eventSourceName) : this(eventSourceName)
		{
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x001345A0 File Offset: 0x001327A0
		~EventSource()
		{
			this.Dispose(false);
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x0000AF5E File Offset: 0x0000915E
		public Exception ConstructionException
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x001345D0 File Offset: 0x001327D0
		public static Guid CurrentThreadActivityId
		{
			get
			{
				return Guid.Empty;
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x001345D0 File Offset: 0x001327D0
		public Guid Guid
		{
			get
			{
				return Guid.Empty;
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x001345D7 File Offset: 0x001327D7
		// (set) Token: 0x06005ADF RID: 23263 RVA: 0x001345DF File Offset: 0x001327DF
		public string Name { get; private set; }

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x001345E8 File Offset: 0x001327E8
		// (set) Token: 0x06005AE1 RID: 23265 RVA: 0x001345F0 File Offset: 0x001327F0
		public EventSourceSettings Settings { get; private set; }

		// Token: 0x06005AE2 RID: 23266 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsEnabled()
		{
			return false;
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsEnabled(EventLevel level, EventKeywords keywords)
		{
			return false;
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsEnabled(EventLevel level, EventKeywords keywords, EventChannel channel)
		{
			return false;
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x001345F9 File Offset: 0x001327F9
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x0000AF5E File Offset: 0x0000915E
		public string GetTrait(string key)
		{
			return null;
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Write(string eventName)
		{
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Write(string eventName, EventSourceOptions options)
		{
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Write<T>(string eventName, T data)
		{
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Write<T>(string eventName, EventSourceOptions options, T data)
		{
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[CLSCompliant(false)]
		public void Write<T>(string eventName, ref EventSourceOptions options, ref T data)
		{
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Write<T>(string eventName, ref EventSourceOptions options, ref Guid activityId, ref Guid relatedActivityId, ref T data)
		{
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void OnEventCommand(EventCommandEventArgs command)
		{
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal void ReportOutOfBandMessage(string msg, bool flush)
		{
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x00134608 File Offset: 0x00132808
		protected void WriteEvent(int eventId)
		{
			this.WriteEvent(eventId, new object[0]);
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x00134617 File Offset: 0x00132817
		protected void WriteEvent(int eventId, byte[] arg1)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1
			});
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x0013462A File Offset: 0x0013282A
		protected void WriteEvent(int eventId, int arg1)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1
			});
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x00134617 File Offset: 0x00132817
		protected void WriteEvent(int eventId, string arg1)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1
			});
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x00134642 File Offset: 0x00132842
		protected void WriteEvent(int eventId, int arg1, int arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x00134663 File Offset: 0x00132863
		protected void WriteEvent(int eventId, int arg1, int arg2, int arg3)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x0013468E File Offset: 0x0013288E
		protected void WriteEvent(int eventId, int arg1, string arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x001346AA File Offset: 0x001328AA
		protected void WriteEvent(int eventId, long arg1)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1
			});
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x001346C2 File Offset: 0x001328C2
		protected void WriteEvent(int eventId, long arg1, byte[] arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x001346DE File Offset: 0x001328DE
		protected void WriteEvent(int eventId, long arg1, long arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x001346FF File Offset: 0x001328FF
		protected void WriteEvent(int eventId, long arg1, long arg2, long arg3)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x001346C2 File Offset: 0x001328C2
		protected void WriteEvent(int eventId, long arg1, string arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected void WriteEvent(int eventId, params object[] args)
		{
		}

		// Token: 0x06005AFD RID: 23293 RVA: 0x0013472A File Offset: 0x0013292A
		protected void WriteEvent(int eventId, string arg1, int arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005AFE RID: 23294 RVA: 0x00134746 File Offset: 0x00132946
		protected void WriteEvent(int eventId, string arg1, int arg2, int arg3)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x06005AFF RID: 23295 RVA: 0x0013476C File Offset: 0x0013296C
		protected void WriteEvent(int eventId, string arg1, long arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x00134788 File Offset: 0x00132988
		protected void WriteEvent(int eventId, string arg1, string arg2)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x0013479F File Offset: 0x0013299F
		protected void WriteEvent(int eventId, string arg1, string arg2, string arg3)
		{
			this.WriteEvent(eventId, new object[]
			{
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x06005B02 RID: 23298 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[CLSCompliant(false)]
		protected unsafe void WriteEventCore(int eventId, int eventDataCount, EventSource.EventData* data)
		{
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected void WriteEventWithRelatedActivityId(int eventId, Guid relatedActivityId, params object[] args)
		{
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[CLSCompliant(false)]
		protected unsafe void WriteEventWithRelatedActivityIdCore(int eventId, Guid* relatedActivityId, int eventDataCount, EventSource.EventData* data)
		{
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06005B05 RID: 23301 RVA: 0x000479FC File Offset: 0x00045BFC
		// (remove) Token: 0x06005B06 RID: 23302 RVA: 0x000479FC File Offset: 0x00045BFC
		public event EventHandler<EventCommandEventArgs> EventCommandExecuted
		{
			add
			{
				throw new NotImplementedException();
			}
			remove
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x000479FC File Offset: 0x00045BFC
		public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x000479FC File Offset: 0x00045BFC
		public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest, EventManifestOptions flags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x000479FC File Offset: 0x00045BFC
		public static Guid GetGuid(Type eventSourceType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x000479FC File Offset: 0x00045BFC
		public static string GetName(Type eventSourceType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005B0B RID: 23307 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IEnumerable<EventSource> GetSources()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005B0C RID: 23308 RVA: 0x000479FC File Offset: 0x00045BFC
		public static void SendCommand(EventSource eventSource, EventCommand command, IDictionary<string, string> commandArguments)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005B0D RID: 23309 RVA: 0x000479FC File Offset: 0x00045BFC
		public static void SetCurrentThreadActivityId(Guid activityId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005B0E RID: 23310 RVA: 0x000479FC File Offset: 0x00045BFC
		public static void SetCurrentThreadActivityId(Guid activityId, out Guid oldActivityThatWillContinue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x020009F8 RID: 2552
		protected internal struct EventData
		{
			// Token: 0x17000F93 RID: 3987
			// (get) Token: 0x06005B0F RID: 23311 RVA: 0x001347BB File Offset: 0x001329BB
			// (set) Token: 0x06005B10 RID: 23312 RVA: 0x001347C3 File Offset: 0x001329C3
			public IntPtr DataPointer { readonly get; set; }

			// Token: 0x17000F94 RID: 3988
			// (get) Token: 0x06005B11 RID: 23313 RVA: 0x001347CC File Offset: 0x001329CC
			// (set) Token: 0x06005B12 RID: 23314 RVA: 0x001347D4 File Offset: 0x001329D4
			public int Size { readonly get; set; }

			// Token: 0x17000F95 RID: 3989
			// (get) Token: 0x06005B13 RID: 23315 RVA: 0x001347DD File Offset: 0x001329DD
			// (set) Token: 0x06005B14 RID: 23316 RVA: 0x001347E5 File Offset: 0x001329E5
			internal int Reserved { readonly get; set; }
		}
	}
}
