using System;
using System.ComponentModel;
using System.IO;

namespace Sirenix.Serialization
{
	// Token: 0x0200000F RID: 15
	public interface IDataReader : IDisposable
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000F9 RID: 249
		// (set) Token: 0x060000FA RID: 250
		TwoWaySerializationBinder Binder { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000FB RID: 251
		// (set) Token: 0x060000FC RID: 252
		[Obsolete("Data readers and writers don't necessarily have streams any longer, so this API has been made obsolete. Using this property may result in NotSupportedExceptions being thrown.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Stream Stream { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000FD RID: 253
		bool IsInArrayNode { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000FE RID: 254
		string CurrentNodeName { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000FF RID: 255
		int CurrentNodeId { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000100 RID: 256
		int CurrentNodeDepth { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000101 RID: 257
		// (set) Token: 0x06000102 RID: 258
		DeserializationContext Context { get; set; }

		// Token: 0x06000103 RID: 259
		string GetDataDump();

		// Token: 0x06000104 RID: 260
		bool EnterNode(out Type type);

		// Token: 0x06000105 RID: 261
		bool ExitNode();

		// Token: 0x06000106 RID: 262
		bool EnterArray(out long length);

		// Token: 0x06000107 RID: 263
		bool ExitArray();

		// Token: 0x06000108 RID: 264
		bool ReadPrimitiveArray<T>(out T[] array) where T : struct;

		// Token: 0x06000109 RID: 265
		EntryType PeekEntry(out string name);

		// Token: 0x0600010A RID: 266
		bool ReadInternalReference(out int id);

		// Token: 0x0600010B RID: 267
		bool ReadExternalReference(out int index);

		// Token: 0x0600010C RID: 268
		bool ReadExternalReference(out Guid guid);

		// Token: 0x0600010D RID: 269
		bool ReadExternalReference(out string id);

		// Token: 0x0600010E RID: 270
		bool ReadChar(out char value);

		// Token: 0x0600010F RID: 271
		bool ReadString(out string value);

		// Token: 0x06000110 RID: 272
		bool ReadGuid(out Guid value);

		// Token: 0x06000111 RID: 273
		bool ReadSByte(out sbyte value);

		// Token: 0x06000112 RID: 274
		bool ReadInt16(out short value);

		// Token: 0x06000113 RID: 275
		bool ReadInt32(out int value);

		// Token: 0x06000114 RID: 276
		bool ReadInt64(out long value);

		// Token: 0x06000115 RID: 277
		bool ReadByte(out byte value);

		// Token: 0x06000116 RID: 278
		bool ReadUInt16(out ushort value);

		// Token: 0x06000117 RID: 279
		bool ReadUInt32(out uint value);

		// Token: 0x06000118 RID: 280
		bool ReadUInt64(out ulong value);

		// Token: 0x06000119 RID: 281
		bool ReadDecimal(out decimal value);

		// Token: 0x0600011A RID: 282
		bool ReadSingle(out float value);

		// Token: 0x0600011B RID: 283
		bool ReadDouble(out double value);

		// Token: 0x0600011C RID: 284
		bool ReadBoolean(out bool value);

		// Token: 0x0600011D RID: 285
		bool ReadNull();

		// Token: 0x0600011E RID: 286
		void SkipEntry();

		// Token: 0x0600011F RID: 287
		void PrepareNewSerializationSession();
	}
}
