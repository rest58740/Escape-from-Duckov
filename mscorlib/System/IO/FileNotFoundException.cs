using System;
using System.Runtime.Serialization;
using System.Security;

namespace System.IO
{
	// Token: 0x02000B08 RID: 2824
	[Serializable]
	public class FileNotFoundException : IOException
	{
		// Token: 0x060064D2 RID: 25810 RVA: 0x001569B0 File Offset: 0x00154BB0
		public FileNotFoundException() : base("Unable to find the specified file.")
		{
			base.HResult = -2147024894;
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x001569C8 File Offset: 0x00154BC8
		public FileNotFoundException(string message) : base(message)
		{
			base.HResult = -2147024894;
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x001569DC File Offset: 0x00154BDC
		public FileNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024894;
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x001569F1 File Offset: 0x00154BF1
		public FileNotFoundException(string message, string fileName) : base(message)
		{
			base.HResult = -2147024894;
			this.FileName = fileName;
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x00156A0C File Offset: 0x00154C0C
		public FileNotFoundException(string message, string fileName, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024894;
			this.FileName = fileName;
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x060064D7 RID: 25815 RVA: 0x00156A28 File Offset: 0x00154C28
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x00156A38 File Offset: 0x00154C38
		private void SetMessageField()
		{
			if (this._message == null)
			{
				if (this.FileName == null && base.HResult == -2146233088)
				{
					this._message = "Unable to find the specified file.";
					return;
				}
				if (this.FileName != null)
				{
					this._message = FileLoadException.FormatFileLoadExceptionMessage(this.FileName, base.HResult);
				}
			}
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x060064D9 RID: 25817 RVA: 0x00156A8D File Offset: 0x00154C8D
		public string FileName { get; }

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x060064DA RID: 25818 RVA: 0x00156A95 File Offset: 0x00154C95
		public string FusionLog { get; }

		// Token: 0x060064DB RID: 25819 RVA: 0x00156AA0 File Offset: 0x00154CA0
		public override string ToString()
		{
			string text = base.GetType().ToString() + ": " + this.Message;
			if (this.FileName != null && this.FileName.Length != 0)
			{
				text = text + Environment.NewLine + SR.Format("File name: '{0}'", this.FileName);
			}
			if (base.InnerException != null)
			{
				text = text + " ---> " + base.InnerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			if (this.FusionLog != null)
			{
				if (text == null)
				{
					text = " ";
				}
				text += Environment.NewLine;
				text += Environment.NewLine;
				text += this.FusionLog;
			}
			return text;
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x00156B6A File Offset: 0x00154D6A
		protected FileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.FileName = info.GetString("FileNotFound_FileName");
			this.FusionLog = info.GetString("FileNotFound_FusionLog");
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x00156B96 File Offset: 0x00154D96
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("FileNotFound_FileName", this.FileName, typeof(string));
			info.AddValue("FileNotFound_FusionLog", this.FusionLog, typeof(string));
		}
	}
}
