using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class ReflectedAction<T1, T2> : ReflectedActionWrapper
	{
		// Token: 0x06000334 RID: 820 RVA: 0x00009282 File Offset: 0x00007482
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.p1,
				this.p2
			};
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000929C File Offset: 0x0000749C
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000092B0 File Offset: 0x000074B0
		public override void Call()
		{
			this.call(this.p1.value, this.p2.value);
		}

		// Token: 0x040000BF RID: 191
		private ActionCall<T1, T2> call;

		// Token: 0x040000C0 RID: 192
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000C1 RID: 193
		public BBParameter<T2> p2 = new BBParameter<T2>();
	}
}
