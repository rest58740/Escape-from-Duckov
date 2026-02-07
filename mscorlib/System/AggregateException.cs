using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System
{
	// Token: 0x020000F1 RID: 241
	[DebuggerDisplay("Count = {InnerExceptionCount}")]
	[Serializable]
	public class AggregateException : Exception
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x00020A73 File Offset: 0x0001EC73
		public AggregateException() : base("One or more errors occurred.")
		{
			this.m_innerExceptions = new ReadOnlyCollection<Exception>(Array.Empty<Exception>());
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00020A90 File Offset: 0x0001EC90
		public AggregateException(string message) : base(message)
		{
			this.m_innerExceptions = new ReadOnlyCollection<Exception>(Array.Empty<Exception>());
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00020AA9 File Offset: 0x0001ECA9
		public AggregateException(string message, Exception innerException) : base(message, innerException)
		{
			if (innerException == null)
			{
				throw new ArgumentNullException("innerException");
			}
			this.m_innerExceptions = new ReadOnlyCollection<Exception>(new Exception[]
			{
				innerException
			});
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00020AD6 File Offset: 0x0001ECD6
		public AggregateException(IEnumerable<Exception> innerExceptions) : this("One or more errors occurred.", innerExceptions)
		{
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00020AE4 File Offset: 0x0001ECE4
		public AggregateException(params Exception[] innerExceptions) : this("One or more errors occurred.", innerExceptions)
		{
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00020AF2 File Offset: 0x0001ECF2
		public AggregateException(string message, IEnumerable<Exception> innerExceptions) : this(message, (innerExceptions as IList<Exception>) ?? ((innerExceptions == null) ? null : new List<Exception>(innerExceptions)))
		{
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00020B11 File Offset: 0x0001ED11
		public AggregateException(string message, params Exception[] innerExceptions) : this(message, innerExceptions)
		{
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00020B1C File Offset: 0x0001ED1C
		private AggregateException(string message, IList<Exception> innerExceptions) : base(message, (innerExceptions != null && innerExceptions.Count > 0) ? innerExceptions[0] : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			Exception[] array = new Exception[innerExceptions.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = innerExceptions[i];
				if (array[i] == null)
				{
					throw new ArgumentException("An element of innerExceptions was null.");
				}
			}
			this.m_innerExceptions = new ReadOnlyCollection<Exception>(array);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00020B94 File Offset: 0x0001ED94
		internal AggregateException(IEnumerable<ExceptionDispatchInfo> innerExceptionInfos) : this("One or more errors occurred.", innerExceptionInfos)
		{
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00020BA2 File Offset: 0x0001EDA2
		internal AggregateException(string message, IEnumerable<ExceptionDispatchInfo> innerExceptionInfos) : this(message, (innerExceptionInfos as IList<ExceptionDispatchInfo>) ?? ((innerExceptionInfos == null) ? null : new List<ExceptionDispatchInfo>(innerExceptionInfos)))
		{
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00020BC4 File Offset: 0x0001EDC4
		private AggregateException(string message, IList<ExceptionDispatchInfo> innerExceptionInfos) : base(message, (innerExceptionInfos != null && innerExceptionInfos.Count > 0 && innerExceptionInfos[0] != null) ? innerExceptionInfos[0].SourceException : null)
		{
			if (innerExceptionInfos == null)
			{
				throw new ArgumentNullException("innerExceptionInfos");
			}
			Exception[] array = new Exception[innerExceptionInfos.Count];
			for (int i = 0; i < array.Length; i++)
			{
				ExceptionDispatchInfo exceptionDispatchInfo = innerExceptionInfos[i];
				if (exceptionDispatchInfo != null)
				{
					array[i] = exceptionDispatchInfo.SourceException;
				}
				if (array[i] == null)
				{
					throw new ArgumentException("An element of innerExceptions was null.");
				}
			}
			this.m_innerExceptions = new ReadOnlyCollection<Exception>(array);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00020C54 File Offset: 0x0001EE54
		protected AggregateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			Exception[] array = info.GetValue("InnerExceptions", typeof(Exception[])) as Exception[];
			if (array == null)
			{
				throw new SerializationException("The serialization stream contains no inner exceptions.");
			}
			this.m_innerExceptions = new ReadOnlyCollection<Exception>(array);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00020CAC File Offset: 0x0001EEAC
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			Exception[] array = new Exception[this.m_innerExceptions.Count];
			this.m_innerExceptions.CopyTo(array, 0);
			info.AddValue("InnerExceptions", array, typeof(Exception[]));
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00020CF8 File Offset: 0x0001EEF8
		public override Exception GetBaseException()
		{
			Exception ex = this;
			AggregateException ex2 = this;
			while (ex2 != null && ex2.InnerExceptions.Count == 1)
			{
				ex = ex.InnerException;
				ex2 = (ex as AggregateException);
			}
			return ex;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00020D2B File Offset: 0x0001EF2B
		public ReadOnlyCollection<Exception> InnerExceptions
		{
			get
			{
				return this.m_innerExceptions;
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00020D34 File Offset: 0x0001EF34
		public void Handle(Func<Exception, bool> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			List<Exception> list = null;
			for (int i = 0; i < this.m_innerExceptions.Count; i++)
			{
				if (!predicate(this.m_innerExceptions[i]))
				{
					if (list == null)
					{
						list = new List<Exception>();
					}
					list.Add(this.m_innerExceptions[i]);
				}
			}
			if (list != null)
			{
				throw new AggregateException(this.Message, list);
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00020DA8 File Offset: 0x0001EFA8
		public AggregateException Flatten()
		{
			List<Exception> list = new List<Exception>();
			List<AggregateException> list2 = new List<AggregateException>();
			list2.Add(this);
			int num = 0;
			while (list2.Count > num)
			{
				IList<Exception> innerExceptions = list2[num++].InnerExceptions;
				for (int i = 0; i < innerExceptions.Count; i++)
				{
					Exception ex = innerExceptions[i];
					if (ex != null)
					{
						AggregateException ex2 = ex as AggregateException;
						if (ex2 != null)
						{
							list2.Add(ex2);
						}
						else
						{
							list.Add(ex);
						}
					}
				}
			}
			return new AggregateException(this.Message, list);
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00020E34 File Offset: 0x0001F034
		public override string Message
		{
			get
			{
				if (this.m_innerExceptions.Count == 0)
				{
					return base.Message;
				}
				StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
				stringBuilder.Append(base.Message);
				stringBuilder.Append(' ');
				for (int i = 0; i < this.m_innerExceptions.Count; i++)
				{
					stringBuilder.Append('(');
					stringBuilder.Append(this.m_innerExceptions[i].Message);
					stringBuilder.Append(") ");
				}
				stringBuilder.Length--;
				return StringBuilderCache.GetStringAndRelease(stringBuilder);
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00020ECC File Offset: 0x0001F0CC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.ToString());
			for (int i = 0; i < this.m_innerExceptions.Count; i++)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("---> ");
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "(Inner Exception #{0}) ", i);
				stringBuilder.Append(this.m_innerExceptions[i].ToString());
				stringBuilder.Append("<---");
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00020F5D File Offset: 0x0001F15D
		private int InnerExceptionCount
		{
			get
			{
				return this.InnerExceptions.Count;
			}
		}

		// Token: 0x04001047 RID: 4167
		private ReadOnlyCollection<Exception> m_innerExceptions;
	}
}
