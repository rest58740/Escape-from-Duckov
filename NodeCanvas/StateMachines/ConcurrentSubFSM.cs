using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E5 RID: 229
	[Name("Parallel Sub FSM", -1)]
	[Description("Execute a Sub FSM in parallel and for as long as this FSM is running.")]
	[Category("SubGraphs")]
	[Color("ff64cb")]
	public class ConcurrentSubFSM : FSMNodeNested<FSM>, IUpdatable, IGraphElement
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x0000F989 File Offset: 0x0000DB89
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000F996 File Offset: 0x0000DB96
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0000F999 File Offset: 0x0000DB99
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000F99C File Offset: 0x0000DB9C
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000F99F File Offset: 0x0000DB9F
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x0000F9AC File Offset: 0x0000DBAC
		public override FSM subGraph
		{
			get
			{
				return this._subFSM.value;
			}
			set
			{
				this._subFSM.value = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x0000F9BA File Offset: 0x0000DBBA
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._subFSM;
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000F9C2 File Offset: 0x0000DBC2
		public override void OnGraphStarted()
		{
			if (this.subGraph == null)
			{
				return;
			}
			base.status = Status.Running;
			this.TryStartSubGraph(base.graphAgent, delegate(bool result)
			{
				base.status = (result ? Status.Success : Status.Failure);
			});
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000F9F3 File Offset: 0x0000DBF3
		void IUpdatable.Update()
		{
			this.TryUpdateSubGraph();
		}

		// Token: 0x0400029F RID: 671
		[SerializeField]
		[ExposeField]
		[Name("Parallel FSM", 0)]
		protected BBParameter<FSM> _subFSM;
	}
}
