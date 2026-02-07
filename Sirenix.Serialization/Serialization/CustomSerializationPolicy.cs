using System;
using System.Reflection;

namespace Sirenix.Serialization
{
	// Token: 0x0200005D RID: 93
	public class CustomSerializationPolicy : ISerializationPolicy
	{
		// Token: 0x06000333 RID: 819 RVA: 0x00016FAD File Offset: 0x000151AD
		public CustomSerializationPolicy(string id, bool allowNonSerializableTypes, Func<MemberInfo, bool> shouldSerializeFunc)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (shouldSerializeFunc == null)
			{
				throw new ArgumentNullException("shouldSerializeFunc");
			}
			this.id = id;
			this.allowNonSerializableTypes = allowNonSerializableTypes;
			this.shouldSerializeFunc = shouldSerializeFunc;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00016FE6 File Offset: 0x000151E6
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00016FEE File Offset: 0x000151EE
		public bool AllowNonSerializableTypes
		{
			get
			{
				return this.allowNonSerializableTypes;
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00016FF6 File Offset: 0x000151F6
		public bool ShouldSerializeMember(MemberInfo member)
		{
			return this.shouldSerializeFunc.Invoke(member);
		}

		// Token: 0x04000104 RID: 260
		private string id;

		// Token: 0x04000105 RID: 261
		private bool allowNonSerializableTypes;

		// Token: 0x04000106 RID: 262
		private Func<MemberInfo, bool> shouldSerializeFunc;
	}
}
