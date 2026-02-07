using System;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000BD RID: 189
	[Name("Implemented Action", 9)]
	[Category("✫ Reflected")]
	[Description("Calls a function that has signature of 'public Status NAME()' or 'public Status NAME(T)'. You should return Status.Success, Failure or Running within that function.")]
	public class ImplementedAction_Multiplatform : ActionTask, IReflectedWrapper
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		private MethodInfo targetMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000CA49 File Offset: 0x0000AC49
		public override Type agentType
		{
			get
			{
				if (this.targetMethod == null)
				{
					return typeof(Transform);
				}
				if (!this.targetMethod.IsStatic)
				{
					return this.targetMethod.RTReflectedOrDeclaredType();
				}
				return null;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000CA80 File Offset: 0x0000AC80
		protected override string info
		{
			get
			{
				if (this.method == null)
				{
					return "No Action Selected";
				}
				if (this.targetMethod == null)
				{
					return this.method.AsString().FormatError();
				}
				string text = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				return string.Format("[ {0}.{1}({2}) ]", text, this.targetMethod.Name, (this.parameters.Count == 1) ? this.parameters[0].ToString() : "");
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000CB1D File Offset: 0x0000AD1D
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.method;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000CB25 File Offset: 0x0000AD25
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.method != null && this.method.HasChanged())
			{
				this.SetMethod(this.method);
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000CB50 File Offset: 0x0000AD50
		protected override string OnInit()
		{
			if (this.method == null)
			{
				return "No method selected";
			}
			if (this.targetMethod == null)
			{
				return string.Format("Missing method '{0}'", this.method.AsString());
			}
			if (this.args == null)
			{
				this.args = new object[this.targetMethod.GetParameters().Length];
			}
			return null;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		protected override void OnUpdate()
		{
			for (int i = 0; i < this.parameters.Count; i++)
			{
				this.args[i] = this.parameters[i].value;
			}
			this.actionStatus = (Status)this.targetMethod.Invoke(this.targetMethod.IsStatic ? null : base.agent, this.args);
			if (this.actionStatus == Status.Success)
			{
				base.EndAction(true);
				return;
			}
			if (this.actionStatus == Status.Failure)
			{
				base.EndAction(false);
				return;
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000CC3F File Offset: 0x0000AE3F
		protected override void OnStop()
		{
			this.actionStatus = Status.Resting;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000CC48 File Offset: 0x0000AE48
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.method = new SerializedMethodInfo(method);
				this.parameters.Clear();
				ParameterInfo[] array = method.GetParameters();
				for (int i = 0; i < array.Length; i++)
				{
					BBObjectParameter bbobjectParameter = new BBObjectParameter(array[i].ParameterType)
					{
						bb = base.blackboard
					};
					this.parameters.Add(bbobjectParameter);
				}
			}
		}

		// Token: 0x04000242 RID: 578
		[SerializeField]
		private SerializedMethodInfo method;

		// Token: 0x04000243 RID: 579
		[SerializeField]
		private List<BBObjectParameter> parameters = new List<BBObjectParameter>();

		// Token: 0x04000244 RID: 580
		private Status actionStatus = Status.Resting;

		// Token: 0x04000245 RID: 581
		private object[] args;
	}
}
