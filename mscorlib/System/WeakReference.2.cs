using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200026B RID: 619
	[Serializable]
	public sealed class WeakReference<T> : ISerializable where T : class
	{
		// Token: 0x06001C23 RID: 7203 RVA: 0x00069374 File Offset: 0x00067574
		public WeakReference(T target) : this(target, false)
		{
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00069380 File Offset: 0x00067580
		public WeakReference(T target, bool trackResurrection)
		{
			this.trackResurrection = trackResurrection;
			GCHandleType type = trackResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak;
			this.handle = GCHandle.Alloc(target, type);
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x000693B4 File Offset: 0x000675B4
		private WeakReference(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.trackResurrection = info.GetBoolean("TrackResurrection");
			object value = info.GetValue("TrackedObject", typeof(T));
			GCHandleType type = this.trackResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak;
			this.handle = GCHandle.Alloc(value, type);
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x00069418 File Offset: 0x00067618
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("TrackResurrection", this.trackResurrection);
			if (this.handle.IsAllocated)
			{
				info.AddValue("TrackedObject", this.handle.Target);
				return;
			}
			info.AddValue("TrackedObject", null);
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x00069474 File Offset: 0x00067674
		public void SetTarget(T target)
		{
			this.handle.Target = target;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x00069487 File Offset: 0x00067687
		public bool TryGetTarget(out T target)
		{
			if (!this.handle.IsAllocated)
			{
				target = default(T);
				return false;
			}
			target = (T)((object)this.handle.Target);
			return target != null;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000694C4 File Offset: 0x000676C4
		~WeakReference()
		{
			this.handle.Free();
		}

		// Token: 0x040019BE RID: 6590
		private GCHandle handle;

		// Token: 0x040019BF RID: 6591
		private bool trackResurrection;
	}
}
