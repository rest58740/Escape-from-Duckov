using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200026A RID: 618
	[ComVisible(true)]
	[Serializable]
	public class WeakReference : ISerializable
	{
		// Token: 0x06001C19 RID: 7193 RVA: 0x0006920C File Offset: 0x0006740C
		private void AllocateHandle(object target)
		{
			if (this.isLongReference)
			{
				this.gcHandle = GCHandle.Alloc(target, GCHandleType.WeakTrackResurrection);
				return;
			}
			this.gcHandle = GCHandle.Alloc(target, GCHandleType.Weak);
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00069231 File Offset: 0x00067431
		public WeakReference(object target) : this(target, false)
		{
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0006923B File Offset: 0x0006743B
		public WeakReference(object target, bool trackResurrection)
		{
			this.isLongReference = trackResurrection;
			this.AllocateHandle(target);
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x00069254 File Offset: 0x00067454
		protected WeakReference(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.isLongReference = info.GetBoolean("TrackResurrection");
			object value = info.GetValue("TrackedObject", typeof(object));
			this.AllocateHandle(value);
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x000692A3 File Offset: 0x000674A3
		public virtual bool IsAlive
		{
			get
			{
				return this.Target != null;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x000692AE File Offset: 0x000674AE
		// (set) Token: 0x06001C1F RID: 7199 RVA: 0x000692CA File Offset: 0x000674CA
		public virtual object Target
		{
			get
			{
				if (!this.gcHandle.IsAllocated)
				{
					return null;
				}
				return this.gcHandle.Target;
			}
			set
			{
				this.gcHandle.Target = value;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001C20 RID: 7200 RVA: 0x000692D8 File Offset: 0x000674D8
		public virtual bool TrackResurrection
		{
			get
			{
				return this.isLongReference;
			}
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000692E0 File Offset: 0x000674E0
		~WeakReference()
		{
			this.gcHandle.Free();
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x00069314 File Offset: 0x00067514
		[SecurityCritical]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("TrackResurrection", this.TrackResurrection);
			try
			{
				info.AddValue("TrackedObject", this.Target);
			}
			catch (Exception)
			{
				info.AddValue("TrackedObject", null);
			}
		}

		// Token: 0x040019BC RID: 6588
		private bool isLongReference;

		// Token: 0x040019BD RID: 6589
		private GCHandle gcHandle;
	}
}
