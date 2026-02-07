using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs
{
	// Token: 0x02000016 RID: 22
	public abstract class MiniExcelDataReaderBase : IMiniExcelDataReader, IDataReader, IDisposable, IDataRecord
	{
		// Token: 0x17000012 RID: 18
		public virtual object this[int i]
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000013 RID: 19
		public virtual object this[string name]
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003972 File Offset: 0x00001B72
		public virtual int Depth { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000397A File Offset: 0x00001B7A
		public virtual bool IsClosed { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003982 File Offset: 0x00001B82
		public virtual int RecordsAffected { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000398A File Offset: 0x00001B8A
		public virtual int FieldCount { get; }

		// Token: 0x0600008A RID: 138 RVA: 0x00003992 File Offset: 0x00001B92
		public virtual bool GetBoolean(int i)
		{
			return false;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003995 File Offset: 0x00001B95
		public virtual byte GetByte(int i)
		{
			return 0;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003998 File Offset: 0x00001B98
		public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			return 0L;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000399C File Offset: 0x00001B9C
		public virtual char GetChar(int i)
		{
			return '\0';
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000399F File Offset: 0x00001B9F
		public virtual long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			return 0L;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000039A3 File Offset: 0x00001BA3
		public virtual IDataReader GetData(int i)
		{
			return null;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000039A6 File Offset: 0x00001BA6
		public virtual string GetDataTypeName(int i)
		{
			return string.Empty;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000039AD File Offset: 0x00001BAD
		public virtual DateTime GetDateTime(int i)
		{
			return DateTime.MinValue;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000039B4 File Offset: 0x00001BB4
		public virtual decimal GetDecimal(int i)
		{
			return 0m;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000039BB File Offset: 0x00001BBB
		public virtual double GetDouble(int i)
		{
			return 0.0;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000039C6 File Offset: 0x00001BC6
		public virtual Type GetFieldType(int i)
		{
			return null;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000039C9 File Offset: 0x00001BC9
		public virtual float GetFloat(int i)
		{
			return 0f;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000039D0 File Offset: 0x00001BD0
		public virtual Guid GetGuid(int i)
		{
			return Guid.Empty;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000039D7 File Offset: 0x00001BD7
		public virtual short GetInt16(int i)
		{
			return 0;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000039DA File Offset: 0x00001BDA
		public virtual int GetInt32(int i)
		{
			return 0;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000039DD File Offset: 0x00001BDD
		public virtual long GetInt64(int i)
		{
			return 0L;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000039E1 File Offset: 0x00001BE1
		public virtual int GetOrdinal(string name)
		{
			return 0;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000039E4 File Offset: 0x00001BE4
		public virtual DataTable GetSchemaTable()
		{
			return null;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000039E7 File Offset: 0x00001BE7
		public virtual string GetString(int i)
		{
			return string.Empty;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000039EE File Offset: 0x00001BEE
		public virtual int GetValues(object[] values)
		{
			return 0;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000039F1 File Offset: 0x00001BF1
		public virtual bool IsDBNull(int i)
		{
			return false;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000039F4 File Offset: 0x00001BF4
		public virtual bool NextResult()
		{
			return false;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000039F8 File Offset: 0x00001BF8
		public virtual Task<bool> NextResultAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return MiniExcelTask.FromCanceled<bool>(cancellationToken);
			}
			Task<bool> result;
			try
			{
				result = (this.NextResult() ? Task.FromResult<bool>(true) : Task.FromResult<bool>(false));
			}
			catch (Exception exception)
			{
				result = MiniExcelTask.FromException<bool>(exception);
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161
		public abstract string GetName(int i);

		// Token: 0x060000A2 RID: 162 RVA: 0x00003A48 File Offset: 0x00001C48
		public virtual Task<string> GetNameAsync(int i, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return MiniExcelTask.FromCanceled<string>(cancellationToken);
			}
			Task<string> result;
			try
			{
				result = Task.FromResult<string>(this.GetName(i));
			}
			catch (Exception exception)
			{
				result = MiniExcelTask.FromException<string>(exception);
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163
		public abstract object GetValue(int i);

		// Token: 0x060000A4 RID: 164 RVA: 0x00003A90 File Offset: 0x00001C90
		public virtual Task<object> GetValueAsync(int i, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return MiniExcelTask.FromCanceled<object>(cancellationToken);
			}
			Task<object> result;
			try
			{
				result = Task.FromResult<object>(this.GetValue(i));
			}
			catch (Exception exception)
			{
				result = MiniExcelTask.FromException<object>(exception);
			}
			return result;
		}

		// Token: 0x060000A5 RID: 165
		public abstract bool Read();

		// Token: 0x060000A6 RID: 166 RVA: 0x00003AD8 File Offset: 0x00001CD8
		public virtual Task<bool> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return MiniExcelTask.FromCanceled<bool>(cancellationToken);
			}
			Task<bool> result;
			try
			{
				result = (this.Read() ? Task.FromResult<bool>(true) : Task.FromResult<bool>(false));
			}
			catch (Exception exception)
			{
				result = MiniExcelTask.FromException<bool>(exception);
			}
			return result;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003B28 File Offset: 0x00001D28
		public virtual void Close()
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003B2C File Offset: 0x00001D2C
		public virtual Task CloseAsync()
		{
			Task result;
			try
			{
				this.Close();
				result = MiniExcelTask.CompletedTask;
			}
			catch (Exception exception)
			{
				result = MiniExcelTask.FromException(exception);
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003B60 File Offset: 0x00001D60
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003B6F File Offset: 0x00001D6F
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}
	}
}
