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
	// Token: 0x020000C5 RID: 197
	[Name("Get Property (Desktop Only)", 8)]
	[Category("✫ Reflected/Faster Versions (Desktop Platforms Only)")]
	[Description("This version works in destop/JIT platform only.\n\nGet a property of a script and save it to the blackboard")]
	public class GetProperty : ActionTask, IReflectedWrapper
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000D4D7 File Offset: 0x0000B6D7
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000D50C File Offset: 0x0000B70C
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
				return string.Format("{0} = {1}.{2}", this.functionWrapper.GetVariables()[0], text, this.targetMethod.Name);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000D590 File Offset: 0x0000B790
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			ReflectedFunctionWrapper reflectedFunctionWrapper = this.functionWrapper;
			if (reflectedFunctionWrapper == null)
			{
				return null;
			}
			return reflectedFunctionWrapper.GetSerializedMethod();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000D5A3 File Offset: 0x0000B7A3
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.functionWrapper != null && this.functionWrapper.HasChanged())
			{
				this.SetMethod(this.functionWrapper.GetMethod());
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000D5CC File Offset: 0x0000B7CC
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
				result = "Get Property Error";
			}
			return result;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000D630 File Offset: 0x0000B830
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

		// Token: 0x0600035B RID: 859 RVA: 0x0000D654 File Offset: 0x0000B854
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.functionWrapper = ReflectedFunctionWrapper.Create(method, base.blackboard);
			}
		}

		// Token: 0x04000253 RID: 595
		[SerializeField]
		protected ReflectedFunctionWrapper functionWrapper;
	}
}
