using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200002C RID: 44
	public interface IBlackboard
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600029E RID: 670
		// (remove) Token: 0x0600029F RID: 671
		event Action<Variable> onVariableAdded;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060002A0 RID: 672
		// (remove) Token: 0x060002A1 RID: 673
		event Action<Variable> onVariableRemoved;

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002A2 RID: 674
		string identifier { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002A3 RID: 675
		IBlackboard parent { get; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002A4 RID: 676
		// (set) Token: 0x060002A5 RID: 677
		Dictionary<string, Variable> variables { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A6 RID: 678
		Component propertiesBindTarget { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002A7 RID: 679
		Object unityContextObject { get; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002A8 RID: 680
		string independantVariablesFieldName { get; }

		// Token: 0x060002A9 RID: 681
		void TryInvokeOnVariableAdded(Variable variable);

		// Token: 0x060002AA RID: 682
		void TryInvokeOnVariableRemoved(Variable variable);
	}
}
