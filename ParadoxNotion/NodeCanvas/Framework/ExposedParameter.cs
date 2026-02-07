using System;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Framework
{
	// Token: 0x0200000A RID: 10
	[SpoofAOT]
	public abstract class ExposedParameter
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000026 RID: 38
		public abstract string targetVariableID { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39
		public abstract Type type { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000028 RID: 40
		// (set) Token: 0x06000029 RID: 41
		public abstract object valueBoxed { get; set; }

		// Token: 0x0600002A RID: 42
		public abstract void Bind(IBlackboard blackboard);

		// Token: 0x0600002B RID: 43
		public abstract void UnBind();

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002C RID: 44
		public abstract Variable varRefBoxed { get; }

		// Token: 0x0600002D RID: 45 RVA: 0x000025B5 File Offset: 0x000007B5
		public static ExposedParameter CreateInstance(Variable target)
		{
			return (ExposedParameter)Activator.CreateInstance(typeof(ExposedParameter<>).MakeGenericType(new Type[]
			{
				target.varType
			}), ReflectionTools.SingleTempArgsArray(target));
		}
	}
}
