using System;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200003A RID: 58
	[Name("Check Property", 9)]
	[Category("✫ Reflected")]
	[Description("Check a property on a script and return if it's equal or not to the check value")]
	public class CheckProperty_Multiplatform : ConditionTask, IReflectedWrapper
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005703 File Offset: 0x00003903
		private MethodInfo targetMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005710 File Offset: 0x00003910
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005748 File Offset: 0x00003948
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
				return string.Format("{0}.{1}{2}", text, this.targetMethod.Name, OperationTools.GetCompareString(this.comparison) + this.checkValue.ToString());
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000057DA File Offset: 0x000039DA
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.method;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000057E2 File Offset: 0x000039E2
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.method != null && this.method.HasChanged())
			{
				this.SetMethod(this.method);
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000580A File Offset: 0x00003A0A
		protected override string OnInit()
		{
			if (this.method == null)
			{
				return "No Property Selected";
			}
			if (this.targetMethod == null)
			{
				return this.method.AsString();
			}
			return null;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005838 File Offset: 0x00003A38
		protected override bool OnCheck()
		{
			Component component = this.targetMethod.IsStatic ? null : base.agent;
			if (this.checkValue.varType == typeof(float))
			{
				return OperationTools.Compare((float)this.targetMethod.Invoke(component, null), (float)this.checkValue.value, this.comparison, 0.05f);
			}
			if (this.checkValue.varType == typeof(int))
			{
				return OperationTools.Compare((int)this.targetMethod.Invoke(component, null), (int)this.checkValue.value, this.comparison);
			}
			return ObjectUtils.AnyEquals(this.targetMethod.Invoke(component, null), this.checkValue.value);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005912 File Offset: 0x00003B12
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.method = new SerializedMethodInfo(method);
				this.checkValue.SetType(method.ReturnType);
				this.comparison = CompareMethod.EqualTo;
			}
		}

		// Token: 0x040000AC RID: 172
		[SerializeField]
		protected SerializedMethodInfo method;

		// Token: 0x040000AD RID: 173
		[SerializeField]
		protected BBObjectParameter checkValue;

		// Token: 0x040000AE RID: 174
		[SerializeField]
		protected CompareMethod comparison;
	}
}
