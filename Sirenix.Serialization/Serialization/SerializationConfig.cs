using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000075 RID: 117
	public class SerializationConfig
	{
		// Token: 0x060003BB RID: 955 RVA: 0x0001A539 File Offset: 0x00018739
		public SerializationConfig()
		{
			this.ResetToDefault();
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0001A554 File Offset: 0x00018754
		// (set) Token: 0x060003BD RID: 957 RVA: 0x0001A5B8 File Offset: 0x000187B8
		public ISerializationPolicy SerializationPolicy
		{
			get
			{
				if (this.serializationPolicy == null)
				{
					object @lock = this.LOCK;
					lock (@lock)
					{
						if (this.serializationPolicy == null)
						{
							this.serializationPolicy = SerializationPolicies.Unity;
						}
					}
				}
				return this.serializationPolicy;
			}
			set
			{
				object @lock = this.LOCK;
				lock (@lock)
				{
					this.serializationPolicy = value;
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0001A5FC File Offset: 0x000187FC
		// (set) Token: 0x060003BF RID: 959 RVA: 0x0001A660 File Offset: 0x00018860
		public DebugContext DebugContext
		{
			get
			{
				if (this.debugContext == null)
				{
					object @lock = this.LOCK;
					lock (@lock)
					{
						if (this.debugContext == null)
						{
							this.debugContext = new DebugContext();
						}
					}
				}
				return this.debugContext;
			}
			set
			{
				object @lock = this.LOCK;
				lock (@lock)
				{
					this.debugContext = value;
				}
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001A6A4 File Offset: 0x000188A4
		public void ResetToDefault()
		{
			object @lock = this.LOCK;
			lock (@lock)
			{
				this.AllowDeserializeInvalidData = false;
				this.serializationPolicy = null;
				if (this.debugContext != null)
				{
					this.debugContext.ResetToDefault();
				}
			}
		}

		// Token: 0x04000150 RID: 336
		private readonly object LOCK = new object();

		// Token: 0x04000151 RID: 337
		private volatile ISerializationPolicy serializationPolicy;

		// Token: 0x04000152 RID: 338
		private volatile DebugContext debugContext;

		// Token: 0x04000153 RID: 339
		public bool AllowDeserializeInvalidData;
	}
}
