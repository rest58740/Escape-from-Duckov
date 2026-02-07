using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020000FF RID: 255
	[Serializable]
	public class BadImageFormatException : SystemException
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x000217BA File Offset: 0x0001F9BA
		public BadImageFormatException() : base("Format of the executable (.exe) or library (.dll) is invalid.")
		{
			base.HResult = -2147024885;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000217D2 File Offset: 0x0001F9D2
		public BadImageFormatException(string message) : base(message)
		{
			base.HResult = -2147024885;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000217E6 File Offset: 0x0001F9E6
		public BadImageFormatException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2147024885;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000217FB File Offset: 0x0001F9FB
		public BadImageFormatException(string message, string fileName) : base(message)
		{
			base.HResult = -2147024885;
			this._fileName = fileName;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00021816 File Offset: 0x0001FA16
		public BadImageFormatException(string message, string fileName, Exception inner) : base(message, inner)
		{
			base.HResult = -2147024885;
			this._fileName = fileName;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00021832 File Offset: 0x0001FA32
		protected BadImageFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._fileName = info.GetString("BadImageFormat_FileName");
			this._fusionLog = info.GetString("BadImageFormat_FusionLog");
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002185E File Offset: 0x0001FA5E
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("BadImageFormat_FileName", this._fileName, typeof(string));
			info.AddValue("BadImageFormat_FusionLog", this._fusionLog, typeof(string));
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0002189E File Offset: 0x0001FA9E
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x000218AC File Offset: 0x0001FAAC
		private void SetMessageField()
		{
			if (this._message == null)
			{
				if (this._fileName == null && base.HResult == -2146233088)
				{
					this._message = "Format of the executable (.exe) or library (.dll) is invalid.";
					return;
				}
				this._message = FileLoadException.FormatFileLoadExceptionMessage(this._fileName, base.HResult);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x000218F9 File Offset: 0x0001FAF9
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00021904 File Offset: 0x0001FB04
		public override string ToString()
		{
			string text = base.GetType().ToString() + ": " + this.Message;
			if (this._fileName != null && this._fileName.Length != 0)
			{
				text = text + Environment.NewLine + SR.Format("File name: '{0}'", this._fileName);
			}
			if (base.InnerException != null)
			{
				text = text + " ---> " + base.InnerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			if (this._fusionLog != null)
			{
				if (text == null)
				{
					text = " ";
				}
				text += Environment.NewLine;
				text += Environment.NewLine;
				text += this._fusionLog;
			}
			return text;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x000219CE File Offset: 0x0001FBCE
		public string FusionLog
		{
			get
			{
				return this._fusionLog;
			}
		}

		// Token: 0x04001068 RID: 4200
		private string _fileName;

		// Token: 0x04001069 RID: 4201
		private string _fusionLog;
	}
}
