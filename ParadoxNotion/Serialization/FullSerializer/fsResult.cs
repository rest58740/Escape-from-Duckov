using System;
using System.Collections.Generic;
using System.Linq;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000AD RID: 173
	public struct fsResult
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x000133D4 File Offset: 0x000115D4
		public static fsResult Warn(string warning)
		{
			fsResult result = default(fsResult);
			result._success = true;
			List<string> list = new List<string>();
			list.Add(warning);
			result._messages = list;
			return result;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00013408 File Offset: 0x00011608
		public static fsResult Fail(string warning)
		{
			fsResult result = default(fsResult);
			result._success = false;
			List<string> list = new List<string>();
			list.Add(warning);
			result._messages = list;
			return result;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00013439 File Offset: 0x00011639
		public void AddMessage(string message)
		{
			if (this._messages == null)
			{
				this._messages = new List<string>();
			}
			this._messages.Add(message);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001345A File Offset: 0x0001165A
		public void AddMessages(fsResult result)
		{
			if (result._messages == null)
			{
				return;
			}
			if (this._messages == null)
			{
				this._messages = new List<string>();
			}
			this._messages.AddRange(result._messages);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001348C File Offset: 0x0001168C
		private fsResult Merge(fsResult other)
		{
			this._success = (this._success && other._success);
			if (other._messages != null)
			{
				if (this._messages == null)
				{
					this._messages = new List<string>(other._messages);
				}
				else
				{
					this._messages.AddRange(other._messages);
				}
			}
			return this;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000134EA File Offset: 0x000116EA
		public static fsResult operator +(fsResult a, fsResult b)
		{
			return a.Merge(b);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x000134F4 File Offset: 0x000116F4
		public bool Failed
		{
			get
			{
				return !this._success;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x000134FF File Offset: 0x000116FF
		public bool Succeeded
		{
			get
			{
				return this._success;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x00013507 File Offset: 0x00011707
		public bool HasWarnings
		{
			get
			{
				return this._messages != null && this._messages.Any<string>();
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001351E File Offset: 0x0001171E
		public fsResult AssertSuccess()
		{
			if (this.Failed)
			{
				throw this.AsException;
			}
			return this;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00013535 File Offset: 0x00011735
		public fsResult AssertSuccessWithoutWarnings()
		{
			if (this.Failed || this.RawMessages.Any<string>())
			{
				throw this.AsException;
			}
			return this;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00013559 File Offset: 0x00011759
		public Exception AsException
		{
			get
			{
				if (!this.Failed && !this.RawMessages.Any<string>())
				{
					throw new Exception("Only a failed result can be converted to an exception");
				}
				return new Exception(this.FormattedMessages);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x00013586 File Offset: 0x00011786
		public IEnumerable<string> RawMessages
		{
			get
			{
				if (this._messages != null)
				{
					return this._messages;
				}
				return fsResult.EmptyStringArray;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001359C File Offset: 0x0001179C
		public string FormattedMessages
		{
			get
			{
				return string.Join(",\n", this.RawMessages.ToArray<string>());
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000135B3 File Offset: 0x000117B3
		public override string ToString()
		{
			return this.FormattedMessages;
		}

		// Token: 0x04000201 RID: 513
		private static readonly string[] EmptyStringArray = new string[0];

		// Token: 0x04000202 RID: 514
		private bool _success;

		// Token: 0x04000203 RID: 515
		private List<string> _messages;

		// Token: 0x04000204 RID: 516
		public static fsResult Success = new fsResult
		{
			_success = true
		};
	}
}
