using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000114 RID: 276
	[Category("Composites")]
	[Description("Used for Utility AI, the Priority Selector executes the child with the highest utility weight. If it fails, the Priority Selector will continue with the next highest utility weight child until one Succeeds, or until all Fail (similar to how a normal Selector does).\n\nEach child branch represents a desire, where each desire has one or more consideration which are all averaged.\nConsiderations are a pair of input value and curve, which together produce the consideration utility weight.\n\nIf Dynamic option is enabled, will continously evaluate utility weights and execute the child with the highest one regardless of what status the children return.")]
	[ParadoxNotion.Design.Icon("Priority", false, "")]
	[Color("b3ff7f")]
	[fsMigrateVersions(new Type[]
	{
		typeof(PrioritySelector_0)
	})]
	public class PrioritySelector : BTComposite, IMigratable<PrioritySelector_0>, IMigratable
	{
		// Token: 0x060005DC RID: 1500 RVA: 0x0001295C File Offset: 0x00010B5C
		void IMigratable<PrioritySelector_0>.Migrate(PrioritySelector_0 model)
		{
			this.desires = new List<PrioritySelector.Desire>();
			foreach (BBParameter<float> input in model.priorities)
			{
				PrioritySelector.Desire desire = new PrioritySelector.Desire();
				this.desires.Add(desire);
				desire.AddConsideration(base.graphBlackboard).input = input;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000129D8 File Offset: 0x00010BD8
		public override void OnChildConnected(int index)
		{
			if (this.desires == null)
			{
				this.desires = new List<PrioritySelector.Desire>();
			}
			if (this.desires.Count < base.outConnections.Count)
			{
				this.desires.Insert(index, new PrioritySelector.Desire());
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00012A16 File Offset: 0x00010C16
		public override void OnChildDisconnected(int index)
		{
			this.desires.RemoveAt(index);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00012A24 File Offset: 0x00010C24
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (this.dynamic)
			{
				float num = float.NegativeInfinity;
				int num2 = 0;
				for (int i = 0; i < base.outConnections.Count; i++)
				{
					float compoundUtility = this.desires[i].GetCompoundUtility();
					if (compoundUtility > num)
					{
						num = compoundUtility;
						num2 = i;
					}
				}
				if (num2 != this.current)
				{
					base.outConnections[this.current].Reset(true);
					this.current = num2;
				}
				return base.outConnections[this.current].Execute(agent, blackboard);
			}
			if (base.status == Status.Resting)
			{
				this.orderedConnections = (from c in base.outConnections
				orderby this.desires[base.outConnections.IndexOf(c)].GetCompoundUtility()
				select c).ToArray<Connection>();
			}
			int num3 = this.orderedConnections.Length;
			while (num3-- > 0)
			{
				base.status = this.orderedConnections[num3].Execute(agent, blackboard);
				if (base.status == Status.Success)
				{
					return Status.Success;
				}
				if (base.status == Status.Running)
				{
					this.current = num3;
					return Status.Running;
				}
			}
			return Status.Failure;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00012B27 File Offset: 0x00010D27
		protected override void OnReset()
		{
			this.current = 0;
		}

		// Token: 0x04000312 RID: 786
		[Tooltip("If enabled, will continously evaluate utility weights and execute the child with the highest one accordingly. In this mode child return status does not matter.")]
		public bool dynamic;

		// Token: 0x04000313 RID: 787
		[Node.AutoSortWithChildrenConnections]
		public List<PrioritySelector.Desire> desires;

		// Token: 0x04000314 RID: 788
		private Connection[] orderedConnections;

		// Token: 0x04000315 RID: 789
		private int current;

		// Token: 0x02000168 RID: 360
		[Serializable]
		public class Desire
		{
			// Token: 0x060006E3 RID: 1763 RVA: 0x000151C4 File Offset: 0x000133C4
			public PrioritySelector.Consideration AddConsideration(IBlackboard bb)
			{
				PrioritySelector.Consideration consideration = new PrioritySelector.Consideration(bb);
				this.considerations.Add(consideration);
				return consideration;
			}

			// Token: 0x060006E4 RID: 1764 RVA: 0x000151E5 File Offset: 0x000133E5
			public void RemoveConsideration(PrioritySelector.Consideration consideration)
			{
				this.considerations.Remove(consideration);
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x000151F4 File Offset: 0x000133F4
			public float GetCompoundUtility()
			{
				float num = 0f;
				for (int i = 0; i < this.considerations.Count; i++)
				{
					num += this.considerations[i].utility;
				}
				return num / (float)this.considerations.Count;
			}

			// Token: 0x0400040F RID: 1039
			[fsIgnoreInBuild]
			public string name;

			// Token: 0x04000410 RID: 1040
			[fsIgnoreInBuild]
			public bool foldout;

			// Token: 0x04000411 RID: 1041
			public List<PrioritySelector.Consideration> considerations = new List<PrioritySelector.Consideration>();
		}

		// Token: 0x02000169 RID: 361
		[Serializable]
		public class Consideration
		{
			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00015252 File Offset: 0x00013452
			public float utility
			{
				get
				{
					if (this.function.value == null)
					{
						return this.input.value;
					}
					return this.function.value.Evaluate(this.input.value);
				}
			}

			// Token: 0x060006E8 RID: 1768 RVA: 0x00015288 File Offset: 0x00013488
			public Consideration(IBlackboard blackboard)
			{
				this.input = new BBParameter<float>
				{
					value = 1f,
					bb = blackboard
				};
				this.function = new BBParameter<AnimationCurve>
				{
					bb = blackboard
				};
			}

			// Token: 0x04000412 RID: 1042
			public BBParameter<float> input;

			// Token: 0x04000413 RID: 1043
			public BBParameter<AnimationCurve> function;
		}
	}
}
