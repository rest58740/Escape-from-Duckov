using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020009C1 RID: 2497
	[ComVisible(true)]
	[MonoTODO("Serialized objects are not compatible with MS.NET")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StackFrame
	{
		// Token: 0x060059D3 RID: 22995
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool get_frame_info(int skip, bool needFileInfo, out MethodBase method, out int iloffset, out int native_offset, out string file, out int line, out int column);

		// Token: 0x060059D4 RID: 22996 RVA: 0x001332D0 File Offset: 0x001314D0
		public StackFrame()
		{
			bool flag = StackFrame.get_frame_info(2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x00133320 File Offset: 0x00131520
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StackFrame(bool fNeedFileInfo)
		{
			bool flag = StackFrame.get_frame_info(2, fNeedFileInfo, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x00133370 File Offset: 0x00131570
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StackFrame(int skipFrames)
		{
			bool flag = StackFrame.get_frame_info(skipFrames + 2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x001333C0 File Offset: 0x001315C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StackFrame(int skipFrames, bool fNeedFileInfo)
		{
			bool flag = StackFrame.get_frame_info(skipFrames + 2, fNeedFileInfo, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x00133410 File Offset: 0x00131610
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StackFrame(string fileName, int lineNumber)
		{
			bool flag = StackFrame.get_frame_info(2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out fileName, out lineNumber, out this.columnNumber);
			this.fileName = fileName;
			this.lineNumber = lineNumber;
			this.columnNumber = 0;
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x0013346C File Offset: 0x0013166C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StackFrame(string fileName, int lineNumber, int colNumber)
		{
			bool flag = StackFrame.get_frame_info(2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out fileName, out lineNumber, out this.columnNumber);
			this.fileName = fileName;
			this.lineNumber = lineNumber;
			this.columnNumber = colNumber;
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x001334C6 File Offset: 0x001316C6
		public virtual int GetFileLineNumber()
		{
			return this.lineNumber;
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x001334CE File Offset: 0x001316CE
		public virtual int GetFileColumnNumber()
		{
			return this.columnNumber;
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x001334D6 File Offset: 0x001316D6
		public virtual string GetFileName()
		{
			return this.fileName;
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x001334E0 File Offset: 0x001316E0
		internal string GetSecureFileName()
		{
			string result = "<filename unknown>";
			if (this.fileName == null)
			{
				return result;
			}
			try
			{
				result = this.GetFileName();
			}
			catch (SecurityException)
			{
			}
			return result;
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x0013351C File Offset: 0x0013171C
		public virtual int GetILOffset()
		{
			return this.ilOffset;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x00133524 File Offset: 0x00131724
		public virtual MethodBase GetMethod()
		{
			return this.methodBase;
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x0013352C File Offset: 0x0013172C
		public virtual int GetNativeOffset()
		{
			return this.nativeOffset;
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x00133534 File Offset: 0x00131734
		internal long GetMethodAddress()
		{
			return this.methodAddress;
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x0013353C File Offset: 0x0013173C
		internal uint GetMethodIndex()
		{
			return this.methodIndex;
		}

		// Token: 0x060059E3 RID: 23011 RVA: 0x00133544 File Offset: 0x00131744
		internal string GetInternalMethodName()
		{
			return this.internalMethodName;
		}

		// Token: 0x060059E4 RID: 23012 RVA: 0x0013354C File Offset: 0x0013174C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.methodBase == null)
			{
				stringBuilder.Append(Locale.GetText("<unknown method>"));
			}
			else
			{
				stringBuilder.Append(this.methodBase.Name);
			}
			stringBuilder.Append(Locale.GetText(" at "));
			if (this.ilOffset == -1)
			{
				stringBuilder.Append(Locale.GetText("<unknown offset>"));
			}
			else
			{
				stringBuilder.Append(Locale.GetText("offset "));
				stringBuilder.Append(this.ilOffset);
			}
			stringBuilder.Append(Locale.GetText(" in file:line:column "));
			stringBuilder.Append(this.GetSecureFileName());
			stringBuilder.AppendFormat(":{0}:{1}", this.lineNumber, this.columnNumber);
			return stringBuilder.ToString();
		}

		// Token: 0x0400378E RID: 14222
		public const int OFFSET_UNKNOWN = -1;

		// Token: 0x0400378F RID: 14223
		private int ilOffset = -1;

		// Token: 0x04003790 RID: 14224
		private int nativeOffset = -1;

		// Token: 0x04003791 RID: 14225
		private long methodAddress;

		// Token: 0x04003792 RID: 14226
		private uint methodIndex;

		// Token: 0x04003793 RID: 14227
		private MethodBase methodBase;

		// Token: 0x04003794 RID: 14228
		private string fileName;

		// Token: 0x04003795 RID: 14229
		private int lineNumber;

		// Token: 0x04003796 RID: 14230
		private int columnNumber;

		// Token: 0x04003797 RID: 14231
		private string internalMethodName;
	}
}
