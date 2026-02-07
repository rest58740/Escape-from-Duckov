using System;
using System.Runtime.Serialization;
using System.Security;

namespace System.IO
{
	// Token: 0x02000B06 RID: 2822
	[Serializable]
	public class FileLoadException : IOException
	{
		// Token: 0x060064C6 RID: 25798 RVA: 0x001567B5 File Offset: 0x001549B5
		public FileLoadException() : base("Could not load the specified file.")
		{
			base.HResult = -2146232799;
		}

		// Token: 0x060064C7 RID: 25799 RVA: 0x001567CD File Offset: 0x001549CD
		public FileLoadException(string message) : base(message)
		{
			base.HResult = -2146232799;
		}

		// Token: 0x060064C8 RID: 25800 RVA: 0x001567E1 File Offset: 0x001549E1
		public FileLoadException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146232799;
		}

		// Token: 0x060064C9 RID: 25801 RVA: 0x001567F6 File Offset: 0x001549F6
		public FileLoadException(string message, string fileName) : base(message)
		{
			base.HResult = -2146232799;
			this.FileName = fileName;
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x00156811 File Offset: 0x00154A11
		public FileLoadException(string message, string fileName, Exception inner) : base(message, inner)
		{
			base.HResult = -2146232799;
			this.FileName = fileName;
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x060064CB RID: 25803 RVA: 0x0015682D File Offset: 0x00154A2D
		public override string Message
		{
			get
			{
				if (this._message == null)
				{
					this._message = FileLoadException.FormatFileLoadExceptionMessage(this.FileName, base.HResult);
				}
				return this._message;
			}
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x060064CC RID: 25804 RVA: 0x00156854 File Offset: 0x00154A54
		public string FileName { get; }

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x060064CD RID: 25805 RVA: 0x0015685C File Offset: 0x00154A5C
		public string FusionLog { get; }

		// Token: 0x060064CE RID: 25806 RVA: 0x00156864 File Offset: 0x00154A64
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

		// Token: 0x060064CF RID: 25807 RVA: 0x0015692E File Offset: 0x00154B2E
		protected FileLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.FileName = info.GetString("FileLoad_FileName");
			this.FusionLog = info.GetString("FileLoad_FusionLog");
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x0015695A File Offset: 0x00154B5A
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("FileLoad_FileName", this.FileName, typeof(string));
			info.AddValue("FileLoad_FusionLog", this.FusionLog, typeof(string));
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x0015699A File Offset: 0x00154B9A
		internal static string FormatFileLoadExceptionMessage(string fileName, int hResult)
		{
			if (fileName != null)
			{
				return SR.Format("Could not load the file '{0}'.", fileName);
			}
			return "Could not load the specified file.";
		}
	}
}
