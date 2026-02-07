using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000AC RID: 172
	public abstract class fsObjectProcessor
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x000133BB File Offset: 0x000115BB
		public virtual bool CanProcess(Type type)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000133C2 File Offset: 0x000115C2
		public virtual void OnBeforeSerialize(Type storageType, object instance)
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000133C4 File Offset: 0x000115C4
		public virtual void OnAfterSerialize(Type storageType, object instance, ref fsData data)
		{
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000133C6 File Offset: 0x000115C6
		public virtual void OnBeforeDeserialize(Type storageType, ref fsData data)
		{
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000133C8 File Offset: 0x000115C8
		public virtual void OnBeforeDeserializeAfterInstanceCreation(Type storageType, object instance, ref fsData data)
		{
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000133CA File Offset: 0x000115CA
		public virtual void OnAfterDeserialize(Type storageType, object instance)
		{
		}
	}
}
