using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x02000168 RID: 360
	[Serializable]
	public class ObjectDisposedException : InvalidOperationException
	{
		// Token: 0x06000E58 RID: 3672 RVA: 0x0003AC55 File Offset: 0x00038E55
		private ObjectDisposedException() : this(null, "Cannot access a disposed object.")
		{
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0003AC63 File Offset: 0x00038E63
		public ObjectDisposedException(string objectName) : this(objectName, "Cannot access a disposed object.")
		{
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0003AC71 File Offset: 0x00038E71
		public ObjectDisposedException(string objectName, string message) : base(message)
		{
			base.HResult = -2146232798;
			this._objectName = objectName;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0003AC8C File Offset: 0x00038E8C
		public ObjectDisposedException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146232798;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0003ACA1 File Offset: 0x00038EA1
		protected ObjectDisposedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._objectName = info.GetString("ObjectName");
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0003ACBC File Offset: 0x00038EBC
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ObjectName", this.ObjectName, typeof(string));
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0003ACE4 File Offset: 0x00038EE4
		public override string Message
		{
			get
			{
				string objectName = this.ObjectName;
				if (objectName == null || objectName.Length == 0)
				{
					return base.Message;
				}
				string str = SR.Format("Object name: '{0}'.", objectName);
				return base.Message + Environment.NewLine + str;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0003AD27 File Offset: 0x00038F27
		public string ObjectName
		{
			get
			{
				if (this._objectName == null)
				{
					return string.Empty;
				}
				return this._objectName;
			}
		}

		// Token: 0x040012A4 RID: 4772
		private string _objectName;
	}
}
