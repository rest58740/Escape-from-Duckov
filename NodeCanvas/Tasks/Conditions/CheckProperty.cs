using System;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000042 RID: 66
	[Name("Check Property (Desktop Only)", 0)]
	[Category("✫ Reflected/Faster Versions (Desktop Platforms Only)")]
	[Description("This version works in destop/JIT platform only.\n\nCheck a property on a script and return if it's equal or not to the check value")]
	public class CheckProperty : ConditionTask
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600012A RID: 298 RVA: 0x0000626E File Offset: 0x0000446E
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006285 File Offset: 0x00004485
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000062BC File Offset: 0x000044BC
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
				return string.Format("{0}.{1}{2}", text, this.targetMethod.Name, OperationTools.GetCompareString(this.comparison) + this.checkValue.ToString());
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000634E File Offset: 0x0000454E
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.functionWrapper != null && this.functionWrapper.HasChanged())
			{
				this.SetMethod(this.functionWrapper.GetMethod());
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006378 File Offset: 0x00004578
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
				result = "CheckProperty Error";
			}
			return result;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000063DC File Offset: 0x000045DC
		protected override bool OnCheck()
		{
			if (this.functionWrapper == null)
			{
				return true;
			}
			if (this.checkValue.varType == typeof(float))
			{
				return OperationTools.Compare((float)this.functionWrapper.Call(), (float)this.checkValue.value, this.comparison, 0.05f);
			}
			if (this.checkValue.varType == typeof(int))
			{
				return OperationTools.Compare((int)this.functionWrapper.Call(), (int)this.checkValue.value, this.comparison);
			}
			return ObjectUtils.AnyEquals(this.functionWrapper.Call(), this.checkValue.value);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000064A3 File Offset: 0x000046A3
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.functionWrapper = ReflectedFunctionWrapper.Create(method, base.blackboard);
				this.checkValue = BBParameter.CreateInstance(method.ReturnType, base.blackboard);
				this.comparison = CompareMethod.EqualTo;
			}
		}

		// Token: 0x040000C0 RID: 192
		[SerializeField]
		protected ReflectedFunctionWrapper functionWrapper;

		// Token: 0x040000C1 RID: 193
		[SerializeField]
		protected BBParameter checkValue;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		protected CompareMethod comparison;
	}
}
