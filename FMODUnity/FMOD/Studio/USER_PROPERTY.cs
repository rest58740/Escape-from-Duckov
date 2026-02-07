using System;

namespace FMOD.Studio
{
	// Token: 0x020000DD RID: 221
	public struct USER_PROPERTY
	{
		// Token: 0x06000498 RID: 1176 RVA: 0x00005340 File Offset: 0x00003540
		public int intValue()
		{
			if (this.type != USER_PROPERTY_TYPE.INTEGER)
			{
				return -1;
			}
			return this.value.intvalue;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00005357 File Offset: 0x00003557
		public bool boolValue()
		{
			return this.type == USER_PROPERTY_TYPE.BOOLEAN && this.value.boolvalue;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000536F File Offset: 0x0000356F
		public float floatValue()
		{
			if (this.type != USER_PROPERTY_TYPE.FLOAT)
			{
				return -1f;
			}
			return this.value.floatvalue;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000538B File Offset: 0x0000358B
		public string stringValue()
		{
			if (this.type != USER_PROPERTY_TYPE.STRING)
			{
				return "";
			}
			return this.value.stringvalue;
		}

		// Token: 0x040004F1 RID: 1265
		public StringWrapper name;

		// Token: 0x040004F2 RID: 1266
		public USER_PROPERTY_TYPE type;

		// Token: 0x040004F3 RID: 1267
		private Union_IntBoolFloatString value;
	}
}
