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
	// Token: 0x020000C3 RID: 195
	[Name("Set Property", 7)]
	[Category("✫ Reflected")]
	[Description("Set a property on a script")]
	public class SetProperty_Multiplatform : ActionTask, IReflectedWrapper
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000CFA5 File Offset: 0x0000B1A5
		private MethodInfo targetMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000CFB2 File Offset: 0x0000B1B2
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
		protected override string info
		{
			get
			{
				if (this.method == null)
				{
					return "No Property Selected";
				}
				if (this.targetMethod == null)
				{
					return this.method.AsString().FormatError();
				}
				string text = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				return string.Format("{0}.{1} = {2}", text, this.targetMethod.Name, this.parameter.ToString());
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000D06A File Offset: 0x0000B26A
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.method;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000D072 File Offset: 0x0000B272
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.method != null && this.method.HasChanged())
			{
				this.SetMethod(this.method);
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000D09A File Offset: 0x0000B29A
		protected override string OnInit()
		{
			if (this.method == null)
			{
				return "No property selected";
			}
			if (this.targetMethod == null)
			{
				return string.Format("Missing property '{0}'", this.method.AsString());
			}
			return null;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000D0CF File Offset: 0x0000B2CF
		protected override void OnExecute()
		{
			this.targetMethod.Invoke(this.targetMethod.IsStatic ? null : base.agent, ReflectionTools.SingleTempArgsArray(this.parameter.value));
			base.EndAction();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000D109 File Offset: 0x0000B309
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.method = new SerializedMethodInfo(method);
				this.parameter.SetType(method.GetParameters()[0].ParameterType);
			}
		}

		// Token: 0x0400024F RID: 591
		[SerializeField]
		protected SerializedMethodInfo method;

		// Token: 0x04000250 RID: 592
		[SerializeField]
		protected BBObjectParameter parameter;
	}
}
