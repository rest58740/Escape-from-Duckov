using System;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000037 RID: 55
	[SpoofAOT]
	[Serializable]
	public class ReflectedAction<T1> : ReflectedActionWrapper
	{
		// Token: 0x06000330 RID: 816 RVA: 0x00009232 File Offset: 0x00007432
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.p1
			};
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00009243 File Offset: 0x00007443
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00009257 File Offset: 0x00007457
		public override void Call()
		{
			this.call(this.p1.value);
		}

		// Token: 0x040000BD RID: 189
		private ActionCall<T1> call;

		// Token: 0x040000BE RID: 190
		public BBParameter<T1> p1 = new BBParameter<T1>();
	}
}
