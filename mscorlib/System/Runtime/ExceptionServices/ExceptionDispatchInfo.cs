using System;
using System.Diagnostics;
using Unity;

namespace System.Runtime.ExceptionServices
{
	// Token: 0x020007D2 RID: 2002
	public sealed class ExceptionDispatchInfo
	{
		// Token: 0x060045AB RID: 17835 RVA: 0x000E5070 File Offset: 0x000E3270
		private ExceptionDispatchInfo(Exception exception)
		{
			this.m_Exception = exception;
			StackTrace[] captured_traces = exception.captured_traces;
			int num = (captured_traces == null) ? 0 : captured_traces.Length;
			StackTrace[] array = new StackTrace[num + 1];
			if (num != 0)
			{
				Array.Copy(captured_traces, 0, array, 0, num);
			}
			array[num] = new StackTrace(exception, 0, true);
			this.m_stackTrace = array;
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060045AC RID: 17836 RVA: 0x000E50C3 File Offset: 0x000E32C3
		internal object BinaryStackTraceArray
		{
			get
			{
				return this.m_stackTrace;
			}
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x000E50CB File Offset: 0x000E32CB
		public static ExceptionDispatchInfo Capture(Exception source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source", Environment.GetResourceString("Object cannot be null."));
			}
			return new ExceptionDispatchInfo(source);
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060045AE RID: 17838 RVA: 0x000E50EB File Offset: 0x000E32EB
		public Exception SourceException
		{
			get
			{
				return this.m_Exception;
			}
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x000E50F3 File Offset: 0x000E32F3
		[StackTraceHidden]
		public void Throw()
		{
			this.m_Exception.RestoreExceptionDispatchInfo(this);
			throw this.m_Exception;
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x00017829 File Offset: 0x00015A29
		[StackTraceHidden]
		public static void Throw(Exception source)
		{
			ExceptionDispatchInfo.Capture(source).Throw();
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x000173AD File Offset: 0x000155AD
		internal ExceptionDispatchInfo()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002D14 RID: 11540
		private Exception m_Exception;

		// Token: 0x04002D15 RID: 11541
		private object m_stackTrace;
	}
}
