using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public class ReflectedAction<T1, T2, T3> : ReflectedActionWrapper
	{
		// Token: 0x06000338 RID: 824 RVA: 0x000092F1 File Offset: 0x000074F1
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.p1,
				this.p2,
				this.p3
			};
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00009314 File Offset: 0x00007514
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00009328 File Offset: 0x00007528
		public override void Call()
		{
			this.call(this.p1.value, this.p2.value, this.p3.value);
		}

		// Token: 0x040000C2 RID: 194
		private ActionCall<T1, T2, T3> call;

		// Token: 0x040000C3 RID: 195
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000C4 RID: 196
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000C5 RID: 197
		public BBParameter<T3> p3 = new BBParameter<T3>();
	}
}
