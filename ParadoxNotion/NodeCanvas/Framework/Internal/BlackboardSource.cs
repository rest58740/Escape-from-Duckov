using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public class BlackboardSource : IBlackboard
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000398 RID: 920 RVA: 0x0000A170 File Offset: 0x00008370
		// (remove) Token: 0x06000399 RID: 921 RVA: 0x0000A1A8 File Offset: 0x000083A8
		public event Action<Variable> onVariableAdded;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600039A RID: 922 RVA: 0x0000A1E0 File Offset: 0x000083E0
		// (remove) Token: 0x0600039B RID: 923 RVA: 0x0000A218 File Offset: 0x00008418
		public event Action<Variable> onVariableRemoved;

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000A24D File Offset: 0x0000844D
		public string identifier
		{
			get
			{
				return "Graph";
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000A254 File Offset: 0x00008454
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0000A25C File Offset: 0x0000845C
		public Dictionary<string, Variable> variables
		{
			get
			{
				return this._variables;
			}
			set
			{
				this._variables = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000A265 File Offset: 0x00008465
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x0000A26D File Offset: 0x0000846D
		public IBlackboard parent { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000A276 File Offset: 0x00008476
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000A27E File Offset: 0x0000847E
		public Object unityContextObject { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000A287 File Offset: 0x00008487
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x0000A28F File Offset: 0x0000848F
		public Component propertiesBindTarget { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000A298 File Offset: 0x00008498
		string IBlackboard.independantVariablesFieldName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000A29B File Offset: 0x0000849B
		void IBlackboard.TryInvokeOnVariableAdded(Variable variable)
		{
			if (this.onVariableAdded != null)
			{
				this.onVariableAdded.Invoke(variable);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000A2B1 File Offset: 0x000084B1
		void IBlackboard.TryInvokeOnVariableRemoved(Variable variable)
		{
			if (this.onVariableRemoved != null)
			{
				this.onVariableRemoved.Invoke(variable);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000A2DF File Offset: 0x000084DF
		public override string ToString()
		{
			return this.identifier;
		}

		// Token: 0x04000106 RID: 262
		[SerializeField]
		private Dictionary<string, Variable> _variables = new Dictionary<string, Variable>(StringComparer.Ordinal);
	}
}
