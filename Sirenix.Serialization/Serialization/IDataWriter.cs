using System;
using System.ComponentModel;
using System.IO;

namespace Sirenix.Serialization
{
	// Token: 0x02000010 RID: 16
	public interface IDataWriter : IDisposable
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000120 RID: 288
		// (set) Token: 0x06000121 RID: 289
		TwoWaySerializationBinder Binder { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000122 RID: 290
		// (set) Token: 0x06000123 RID: 291
		[Obsolete("Data readers and writers don't necessarily have streams any longer, so this API has been made obsolete. Using this property may result in NotSupportedExceptions being thrown.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Stream Stream { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000124 RID: 292
		bool IsInArrayNode { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000125 RID: 293
		// (set) Token: 0x06000126 RID: 294
		SerializationContext Context { get; set; }

		// Token: 0x06000127 RID: 295
		string GetDataDump();

		// Token: 0x06000128 RID: 296
		void FlushToStream();

		// Token: 0x06000129 RID: 297
		void BeginReferenceNode(string name, Type type, int id);

		// Token: 0x0600012A RID: 298
		void BeginStructNode(string name, Type type);

		// Token: 0x0600012B RID: 299
		void EndNode(string name);

		// Token: 0x0600012C RID: 300
		void BeginArrayNode(long length);

		// Token: 0x0600012D RID: 301
		void EndArrayNode();

		// Token: 0x0600012E RID: 302
		void WritePrimitiveArray<T>(T[] array) where T : struct;

		// Token: 0x0600012F RID: 303
		void WriteNull(string name);

		// Token: 0x06000130 RID: 304
		void WriteInternalReference(string name, int id);

		// Token: 0x06000131 RID: 305
		void WriteExternalReference(string name, int index);

		// Token: 0x06000132 RID: 306
		void WriteExternalReference(string name, Guid guid);

		// Token: 0x06000133 RID: 307
		void WriteExternalReference(string name, string id);

		// Token: 0x06000134 RID: 308
		void WriteChar(string name, char value);

		// Token: 0x06000135 RID: 309
		void WriteString(string name, string value);

		// Token: 0x06000136 RID: 310
		void WriteGuid(string name, Guid value);

		// Token: 0x06000137 RID: 311
		void WriteSByte(string name, sbyte value);

		// Token: 0x06000138 RID: 312
		void WriteInt16(string name, short value);

		// Token: 0x06000139 RID: 313
		void WriteInt32(string name, int value);

		// Token: 0x0600013A RID: 314
		void WriteInt64(string name, long value);

		// Token: 0x0600013B RID: 315
		void WriteByte(string name, byte value);

		// Token: 0x0600013C RID: 316
		void WriteUInt16(string name, ushort value);

		// Token: 0x0600013D RID: 317
		void WriteUInt32(string name, uint value);

		// Token: 0x0600013E RID: 318
		void WriteUInt64(string name, ulong value);

		// Token: 0x0600013F RID: 319
		void WriteDecimal(string name, decimal value);

		// Token: 0x06000140 RID: 320
		void WriteSingle(string name, float value);

		// Token: 0x06000141 RID: 321
		void WriteDouble(string name, double value);

		// Token: 0x06000142 RID: 322
		void WriteBoolean(string name, bool value);

		// Token: 0x06000143 RID: 323
		void PrepareNewSerializationSession();
	}
}
