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
	// Token: 0x020000C7 RID: 199
	[Name("Set Property (Desktop Only)", 7)]
	[Category("✫ Reflected/Faster Versions (Desktop Platforms Only)")]
	[Description("This version works in destop/JIT platform only.\n\nSet a property on a script.")]
	public class SetProperty : ActionTask, IReflectedWrapper
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000D891 File Offset: 0x0000BA91
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000D8A8 File Offset: 0x0000BAA8
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000D8E0 File Offset: 0x0000BAE0
		protected override string info
		{
			get
			{
				if (this.functionWrapper == null)
				{
					return "No Property Selected";
				}
				if (this.targetMethod == null)
				{
					return this.functionWrapper.AsString().FormatError();
				}
				string text = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				return string.Format("{0}.{1} = {2}", text, this.targetMethod.Name, this.functionWrapper.GetVariables()[0]);
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000D964 File Offset: 0x0000BB64
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			ReflectedActionWrapper reflectedActionWrapper = this.functionWrapper;
			if (reflectedActionWrapper == null)
			{
				return null;
			}
			return reflectedActionWrapper.GetSerializedMethod();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000D977 File Offset: 0x0000BB77
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.functionWrapper != null && this.functionWrapper.HasChanged())
			{
				this.SetMethod(this.functionWrapper.GetMethod());
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		protected override string OnInit()
		{
			if (this.targetMethod == null)
			{
				return "Missing Property";
			}
			string result;
			try
			{
				this.functionWrapper.Init(this.targetMethod.IsStatic ? null : base.agent);
				result = null;
			}
			catch
			{
				result = "SetProperty Error";
			}
			return result;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000DA04 File Offset: 0x0000BC04
		protected override void OnExecute()
		{
			if (this.functionWrapper == null)
			{
				base.EndAction(false);
				return;
			}
			this.functionWrapper.Call();
			base.EndAction();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000DA27 File Offset: 0x0000BC27
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.functionWrapper = ReflectedActionWrapper.Create(method, base.blackboard);
			}
		}

		// Token: 0x04000256 RID: 598
		[SerializeField]
		protected ReflectedActionWrapper functionWrapper;
	}
}
