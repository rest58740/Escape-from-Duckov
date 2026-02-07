using System;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000C6 RID: 198
	[Name("Implemented Action (Desktop Only)", 9)]
	[Category("✫ Reflected/Faster Versions (Desktop Platforms Only)")]
	[Description("This version works in destop/JIT platform only.\n\nCalls a function that has signature of 'public Status NAME()' or 'public Status NAME(T)'. You should return Status.Success, Failure or Running within that function.")]
	public class ImplementedAction : ActionTask, IReflectedWrapper
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000D679 File Offset: 0x0000B879
		private MethodInfo targetMethod
		{
			get
			{
				if (this.functionWrapper == null)
				{
					return null;
				}
				return this.functionWrapper.GetMethod();
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000D690 File Offset: 0x0000B890
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

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		protected override string info
		{
			get
			{
				if (this.functionWrapper == null)
				{
					return "No Action Selected";
				}
				if (this.targetMethod == null)
				{
					return this.functionWrapper.AsString().FormatError();
				}
				string text = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				return string.Format("[ {0}.{1}({2}) ]", text, this.targetMethod.Name, (this.functionWrapper.GetVariables().Length == 2) ? this.functionWrapper.GetVariables()[1].ToString() : "");
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000D768 File Offset: 0x0000B968
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			ReflectedFunctionWrapper reflectedFunctionWrapper = this.functionWrapper;
			if (reflectedFunctionWrapper == null)
			{
				return null;
			}
			return reflectedFunctionWrapper.GetSerializedMethod();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000D77B File Offset: 0x0000B97B
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.functionWrapper != null && this.functionWrapper.HasChanged())
			{
				this.SetMethod(this.functionWrapper.GetMethod());
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		protected override string OnInit()
		{
			if (this.targetMethod == null)
			{
				return "Missing Method";
			}
			string result;
			try
			{
				this.functionWrapper.Init(this.targetMethod.IsStatic ? null : base.agent);
				result = null;
			}
			catch
			{
				result = "ImplementedAction Error";
			}
			return result;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D808 File Offset: 0x0000BA08
		protected override void OnUpdate()
		{
			if (this.functionWrapper == null)
			{
				base.EndAction(false);
				return;
			}
			this.actionStatus = (Status)this.functionWrapper.Call();
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

		// Token: 0x06000364 RID: 868 RVA: 0x0000D85C File Offset: 0x0000BA5C
		protected override void OnStop()
		{
			this.actionStatus = Status.Resting;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000D865 File Offset: 0x0000BA65
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.functionWrapper = ReflectedFunctionWrapper.Create(method, base.blackboard);
			}
		}

		// Token: 0x04000254 RID: 596
		[SerializeField]
		protected ReflectedFunctionWrapper functionWrapper;

		// Token: 0x04000255 RID: 597
		private Status actionStatus = Status.Resting;
	}
}
