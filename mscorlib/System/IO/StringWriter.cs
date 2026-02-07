using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B5A RID: 2906
	[ComVisible(true)]
	[Serializable]
	public class StringWriter : TextWriter
	{
		// Token: 0x06006948 RID: 26952 RVA: 0x00167E3B File Offset: 0x0016603B
		public StringWriter() : this(new StringBuilder(), CultureInfo.CurrentCulture)
		{
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x00167E4D File Offset: 0x0016604D
		public StringWriter(IFormatProvider formatProvider) : this(new StringBuilder(), formatProvider)
		{
		}

		// Token: 0x0600694A RID: 26954 RVA: 0x00167E5B File Offset: 0x0016605B
		public StringWriter(StringBuilder sb) : this(sb, CultureInfo.CurrentCulture)
		{
		}

		// Token: 0x0600694B RID: 26955 RVA: 0x00167E69 File Offset: 0x00166069
		public StringWriter(StringBuilder sb, IFormatProvider formatProvider) : base(formatProvider)
		{
			if (sb == null)
			{
				throw new ArgumentNullException("sb", Environment.GetResourceString("Buffer cannot be null."));
			}
			this._sb = sb;
			this._isOpen = true;
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x00167E98 File Offset: 0x00166098
		public override void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x0600694D RID: 26957 RVA: 0x00167EA1 File Offset: 0x001660A1
		protected override void Dispose(bool disposing)
		{
			this._isOpen = false;
			base.Dispose(disposing);
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x0600694E RID: 26958 RVA: 0x00167EB1 File Offset: 0x001660B1
		public override Encoding Encoding
		{
			get
			{
				if (StringWriter.m_encoding == null)
				{
					StringWriter.m_encoding = new UnicodeEncoding(false, false);
				}
				return StringWriter.m_encoding;
			}
		}

		// Token: 0x0600694F RID: 26959 RVA: 0x00167ED1 File Offset: 0x001660D1
		public virtual StringBuilder GetStringBuilder()
		{
			return this._sb;
		}

		// Token: 0x06006950 RID: 26960 RVA: 0x00167ED9 File Offset: 0x001660D9
		public override void Write(char value)
		{
			if (!this._isOpen)
			{
				__Error.WriterClosed();
			}
			this._sb.Append(value);
		}

		// Token: 0x06006951 RID: 26961 RVA: 0x00167EF8 File Offset: 0x001660F8
		public override void Write(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("Buffer cannot be null."));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("Non-negative number required."));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			if (!this._isOpen)
			{
				__Error.WriterClosed();
			}
			this._sb.Append(buffer, index, count);
		}

		// Token: 0x06006952 RID: 26962 RVA: 0x00167F83 File Offset: 0x00166183
		public override void Write(string value)
		{
			if (!this._isOpen)
			{
				__Error.WriterClosed();
			}
			if (value != null)
			{
				this._sb.Append(value);
			}
		}

		// Token: 0x06006953 RID: 26963 RVA: 0x0015D6AC File Offset: 0x0015B8AC
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override Task WriteAsync(char value)
		{
			this.Write(value);
			return Task.CompletedTask;
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x0015D6BA File Offset: 0x0015B8BA
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override Task WriteAsync(string value)
		{
			this.Write(value);
			return Task.CompletedTask;
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x0015D6C8 File Offset: 0x0015B8C8
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override Task WriteAsync(char[] buffer, int index, int count)
		{
			this.Write(buffer, index, count);
			return Task.CompletedTask;
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x0015D6D8 File Offset: 0x0015B8D8
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override Task WriteLineAsync(char value)
		{
			this.WriteLine(value);
			return Task.CompletedTask;
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x0015D6E6 File Offset: 0x0015B8E6
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override Task WriteLineAsync(string value)
		{
			this.WriteLine(value);
			return Task.CompletedTask;
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x0015D6F4 File Offset: 0x0015B8F4
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override Task WriteLineAsync(char[] buffer, int index, int count)
		{
			this.WriteLine(buffer, index, count);
			return Task.CompletedTask;
		}

		// Token: 0x06006959 RID: 26969 RVA: 0x00078866 File Offset: 0x00076A66
		[ComVisible(false)]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override Task FlushAsync()
		{
			return Task.CompletedTask;
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x00167FA2 File Offset: 0x001661A2
		public override string ToString()
		{
			return this._sb.ToString();
		}

		// Token: 0x04003D27 RID: 15655
		private static volatile UnicodeEncoding m_encoding;

		// Token: 0x04003D28 RID: 15656
		private StringBuilder _sb;

		// Token: 0x04003D29 RID: 15657
		private bool _isOpen;
	}
}
