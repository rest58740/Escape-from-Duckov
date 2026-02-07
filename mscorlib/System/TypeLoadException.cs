using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x02000210 RID: 528
	[ComVisible(true)]
	[Serializable]
	public class TypeLoadException : SystemException, ISerializable
	{
		// Token: 0x06001740 RID: 5952 RVA: 0x0005A91A File Offset: 0x00058B1A
		public TypeLoadException() : base(Environment.GetResourceString("Failure has occurred while loading a type."))
		{
			base.SetErrorCode(-2146233054);
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x0005A937 File Offset: 0x00058B37
		public TypeLoadException(string message) : base(message)
		{
			base.SetErrorCode(-2146233054);
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x0005A94B File Offset: 0x00058B4B
		public TypeLoadException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233054);
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x0005A960 File Offset: 0x00058B60
		public override string Message
		{
			[SecuritySafeCritical]
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x0005A970 File Offset: 0x00058B70
		[SecurityCritical]
		private void SetMessageField()
		{
			if (this._message == null)
			{
				if (this.ClassName == null && this.ResourceId == 0)
				{
					this._message = Environment.GetResourceString("Failure has occurred while loading a type.");
					return;
				}
				if (this.AssemblyName == null)
				{
					this.AssemblyName = Environment.GetResourceString("[Unknown]");
				}
				if (this.ClassName == null)
				{
					this.ClassName = Environment.GetResourceString("[Unknown]");
				}
				string format = "Could not load type '{0}' from assembly '{1}'.";
				this._message = string.Format(CultureInfo.CurrentCulture, format, this.ClassName, this.AssemblyName, this.MessageArg);
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x0005AA01 File Offset: 0x00058C01
		public string TypeName
		{
			get
			{
				if (this.ClassName == null)
				{
					return string.Empty;
				}
				return this.ClassName;
			}
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x0005AA17 File Offset: 0x00058C17
		private TypeLoadException(string className, string assemblyName) : this(className, assemblyName, null, 0)
		{
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0005AA23 File Offset: 0x00058C23
		[SecurityCritical]
		private TypeLoadException(string className, string assemblyName, string messageArg, int resourceId) : base(null)
		{
			base.SetErrorCode(-2146233054);
			this.ClassName = className;
			this.AssemblyName = assemblyName;
			this.MessageArg = messageArg;
			this.ResourceId = resourceId;
			this.SetMessageField();
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0005AA5C File Offset: 0x00058C5C
		protected TypeLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.ClassName = info.GetString("TypeLoadClassName");
			this.AssemblyName = info.GetString("TypeLoadAssemblyName");
			this.MessageArg = info.GetString("TypeLoadMessageArg");
			this.ResourceId = info.GetInt32("TypeLoadResourceID");
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0005AAC4 File Offset: 0x00058CC4
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("TypeLoadClassName", this.ClassName, typeof(string));
			info.AddValue("TypeLoadAssemblyName", this.AssemblyName, typeof(string));
			info.AddValue("TypeLoadMessageArg", this.MessageArg, typeof(string));
			info.AddValue("TypeLoadResourceID", this.ResourceId);
		}

		// Token: 0x0400162F RID: 5679
		private string ClassName;

		// Token: 0x04001630 RID: 5680
		private string AssemblyName;

		// Token: 0x04001631 RID: 5681
		private string MessageArg;

		// Token: 0x04001632 RID: 5682
		internal int ResourceId;
	}
}
