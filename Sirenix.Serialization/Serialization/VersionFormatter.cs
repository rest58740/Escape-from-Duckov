using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000055 RID: 85
	public sealed class VersionFormatter : MinimalBaseFormatter<Version>
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override Version GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000169F4 File Offset: 0x00014BF4
		protected override void Read(ref Version value, IDataReader reader)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			reader.ReadInt32(out num);
			reader.ReadInt32(out num2);
			reader.ReadInt32(out num3);
			reader.ReadInt32(out num4);
			if (num < 0 || num2 < 0)
			{
				value = new Version();
				return;
			}
			if (num3 < 0)
			{
				value = new Version(num, num2);
				return;
			}
			if (num4 < 0)
			{
				value = new Version(num, num2, num3);
				return;
			}
			value = new Version(num, num2, num3, num4);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00016A65 File Offset: 0x00014C65
		protected override void Write(ref Version value, IDataWriter writer)
		{
			writer.WriteInt32(null, value.Major);
			writer.WriteInt32(null, value.Minor);
			writer.WriteInt32(null, value.Build);
			writer.WriteInt32(null, value.Revision);
		}
	}
}
