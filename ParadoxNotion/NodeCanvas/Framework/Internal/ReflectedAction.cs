using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	public class ReflectedAction : ReflectedActionWrapper
	{
		// Token: 0x0600032C RID: 812 RVA: 0x00009201 File Offset: 0x00007401
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[0];
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00009209 File Offset: 0x00007409
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000921D File Offset: 0x0000741D
		public override void Call()
		{
			this.call();
		}

		// Token: 0x040000BC RID: 188
		private ActionCall call;
	}
}
