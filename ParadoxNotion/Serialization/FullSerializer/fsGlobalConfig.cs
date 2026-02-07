using System;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000A7 RID: 167
	public static class fsGlobalConfig
	{
		// Token: 0x040001EB RID: 491
		public static bool SerializeDefaultValues = false;

		// Token: 0x040001EC RID: 492
		public static bool IsCaseSensitive = false;

		// Token: 0x040001ED RID: 493
		public static Type[] IgnoreSerializeAttributes = new Type[]
		{
			typeof(NonSerializedAttribute),
			typeof(fsIgnoreAttribute)
		};

		// Token: 0x040001EE RID: 494
		public static Type[] SerializeAttributes = new Type[]
		{
			typeof(SerializeField),
			typeof(fsSerializeAsAttribute)
		};

		// Token: 0x040001EF RID: 495
		public static string CustomDateTimeFormatString = null;

		// Token: 0x040001F0 RID: 496
		public static bool Serialize64BitIntegerAsString = false;

		// Token: 0x040001F1 RID: 497
		public static bool SerializeEnumsAsInteger = true;
	}
}
