using System;
using System.IO;

namespace Sirenix.Serialization
{
	// Token: 0x02000009 RID: 9
	public abstract class BaseDataReader : BaseDataReaderWriter, IDataReader, IDisposable
	{
		// Token: 0x0600002B RID: 43 RVA: 0x0000233D File Offset: 0x0000053D
		protected BaseDataReader(Stream stream, DeserializationContext context)
		{
			this.context = context;
			if (stream != null)
			{
				this.Stream = stream;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002356 File Offset: 0x00000556
		public int CurrentNodeId
		{
			get
			{
				return base.CurrentNode.Id;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002363 File Offset: 0x00000563
		public int CurrentNodeDepth
		{
			get
			{
				return base.NodeDepth;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000236B File Offset: 0x0000056B
		public string CurrentNodeName
		{
			get
			{
				return base.CurrentNode.Name;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002378 File Offset: 0x00000578
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002380 File Offset: 0x00000580
		public virtual Stream Stream
		{
			get
			{
				return this.stream;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.CanRead)
				{
					throw new ArgumentException("Cannot read from stream");
				}
				this.stream = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000023AA File Offset: 0x000005AA
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000023C5 File Offset: 0x000005C5
		public DeserializationContext Context
		{
			get
			{
				if (this.context == null)
				{
					this.context = new DeserializationContext();
				}
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x06000033 RID: 51
		public abstract bool EnterNode(out Type type);

		// Token: 0x06000034 RID: 52
		public abstract bool ExitNode();

		// Token: 0x06000035 RID: 53
		public abstract bool EnterArray(out long length);

		// Token: 0x06000036 RID: 54
		public abstract bool ExitArray();

		// Token: 0x06000037 RID: 55
		public abstract bool ReadPrimitiveArray<T>(out T[] array) where T : struct;

		// Token: 0x06000038 RID: 56
		public abstract EntryType PeekEntry(out string name);

		// Token: 0x06000039 RID: 57
		public abstract bool ReadInternalReference(out int id);

		// Token: 0x0600003A RID: 58
		public abstract bool ReadExternalReference(out int index);

		// Token: 0x0600003B RID: 59
		public abstract bool ReadExternalReference(out Guid guid);

		// Token: 0x0600003C RID: 60
		public abstract bool ReadExternalReference(out string id);

		// Token: 0x0600003D RID: 61
		public abstract bool ReadChar(out char value);

		// Token: 0x0600003E RID: 62
		public abstract bool ReadString(out string value);

		// Token: 0x0600003F RID: 63
		public abstract bool ReadGuid(out Guid value);

		// Token: 0x06000040 RID: 64
		public abstract bool ReadSByte(out sbyte value);

		// Token: 0x06000041 RID: 65
		public abstract bool ReadInt16(out short value);

		// Token: 0x06000042 RID: 66
		public abstract bool ReadInt32(out int value);

		// Token: 0x06000043 RID: 67
		public abstract bool ReadInt64(out long value);

		// Token: 0x06000044 RID: 68
		public abstract bool ReadByte(out byte value);

		// Token: 0x06000045 RID: 69
		public abstract bool ReadUInt16(out ushort value);

		// Token: 0x06000046 RID: 70
		public abstract bool ReadUInt32(out uint value);

		// Token: 0x06000047 RID: 71
		public abstract bool ReadUInt64(out ulong value);

		// Token: 0x06000048 RID: 72
		public abstract bool ReadDecimal(out decimal value);

		// Token: 0x06000049 RID: 73
		public abstract bool ReadSingle(out float value);

		// Token: 0x0600004A RID: 74
		public abstract bool ReadDouble(out double value);

		// Token: 0x0600004B RID: 75
		public abstract bool ReadBoolean(out bool value);

		// Token: 0x0600004C RID: 76
		public abstract bool ReadNull();

		// Token: 0x0600004D RID: 77 RVA: 0x000023D0 File Offset: 0x000005D0
		public virtual void SkipEntry()
		{
			EntryType entryType = this.PeekEntry();
			if (entryType == EntryType.StartOfNode)
			{
				bool flag = true;
				Type type;
				this.EnterNode(out type);
				try
				{
					if (type != null)
					{
						if (FormatterUtilities.IsPrimitiveType(type))
						{
							Serializer serializer = Serializer.Get(type);
							object reference = serializer.ReadValueWeak(this);
							if (this.CurrentNodeId >= 0)
							{
								this.Context.RegisterInternalReference(this.CurrentNodeId, reference);
							}
						}
						else
						{
							IFormatter formatter = FormatterLocator.GetFormatter(type, this.Context.Config.SerializationPolicy);
							object reference2 = formatter.Deserialize(this);
							if (this.CurrentNodeId >= 0)
							{
								this.Context.RegisterInternalReference(this.CurrentNodeId, reference2);
							}
						}
					}
					else
					{
						for (;;)
						{
							entryType = this.PeekEntry();
							if (entryType == EntryType.EndOfStream || entryType == EntryType.EndOfNode)
							{
								break;
							}
							if (entryType == EntryType.EndOfArray)
							{
								this.ReadToNextEntry();
							}
							else
							{
								this.SkipEntry();
							}
						}
					}
					return;
				}
				catch (SerializationAbortException ex)
				{
					flag = false;
					throw ex;
				}
				finally
				{
					if (flag)
					{
						this.ExitNode();
					}
				}
			}
			if (entryType == EntryType.StartOfArray)
			{
				this.ReadToNextEntry();
				for (;;)
				{
					entryType = this.PeekEntry();
					if (entryType == EntryType.EndOfStream)
					{
						return;
					}
					if (entryType == EntryType.EndOfArray)
					{
						break;
					}
					if (entryType == EntryType.EndOfNode)
					{
						this.ReadToNextEntry();
					}
					else
					{
						this.SkipEntry();
					}
				}
				this.ReadToNextEntry();
				return;
			}
			if (entryType != EntryType.EndOfArray && entryType != EntryType.EndOfNode)
			{
				this.ReadToNextEntry();
			}
		}

		// Token: 0x0600004E RID: 78
		public abstract void Dispose();

		// Token: 0x0600004F RID: 79 RVA: 0x00002518 File Offset: 0x00000718
		public virtual void PrepareNewSerializationSession()
		{
			base.ClearNodes();
		}

		// Token: 0x06000050 RID: 80
		public abstract string GetDataDump();

		// Token: 0x06000051 RID: 81
		protected abstract EntryType PeekEntry();

		// Token: 0x06000052 RID: 82
		protected abstract EntryType ReadToNextEntry();

		// Token: 0x04000009 RID: 9
		private DeserializationContext context;

		// Token: 0x0400000A RID: 10
		private Stream stream;
	}
}
