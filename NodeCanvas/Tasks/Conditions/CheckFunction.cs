using System;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000041 RID: 65
	[Name("Check Function (Desktop Only)", 0)]
	[Category("✫ Reflected/Faster Versions (Desktop Platforms Only)")]
	[Description("This version works in destop/JIT platform only.\n\nCall a function with none or up to 6 parameters on a component and return whether or not the return value is equal to the check value")]
	public class CheckFunction : ConditionTask
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005FA0 File Offset: 0x000041A0
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00005FB7 File Offset: 0x000041B7
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

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005FEC File Offset: 0x000041EC
		protected override string info
		{
			get
			{
				if (this.functionWrapper == null)
				{
					return "No Method Selected";
				}
				if (this.targetMethod == null)
				{
					return this.functionWrapper.AsString().FormatError();
				}
				BBParameter[] variables = this.functionWrapper.GetVariables();
				string text = "";
				for (int i = 1; i < variables.Length; i++)
				{
					text = text + ((i != 1) ? ", " : "") + variables[i].ToString();
				}
				string text2 = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				string text3 = "{0}.{1}({2}){3}";
				object[] array = new object[4];
				array[0] = text2;
				array[1] = this.targetMethod.Name;
				array[2] = text;
				int num = 3;
				string compareString = OperationTools.GetCompareString(this.comparison);
				BBParameter bbparameter = this.checkValue;
				array[num] = compareString + ((bbparameter != null) ? bbparameter.ToString() : null);
				return string.Format(text3, array);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000060D7 File Offset: 0x000042D7
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.functionWrapper != null && this.functionWrapper.HasChanged())
			{
				this.SetMethod(this.functionWrapper.GetMethod());
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006100 File Offset: 0x00004300
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
				result = "CheckFunction Error";
			}
			return result;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006164 File Offset: 0x00004364
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

		// Token: 0x06000128 RID: 296 RVA: 0x0000622B File Offset: 0x0000442B
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.functionWrapper = ReflectedFunctionWrapper.Create(method, base.blackboard);
				this.checkValue = BBParameter.CreateInstance(method.ReturnType, base.blackboard);
				this.comparison = CompareMethod.EqualTo;
			}
		}

		// Token: 0x040000BD RID: 189
		[SerializeField]
		protected ReflectedFunctionWrapper functionWrapper;

		// Token: 0x040000BE RID: 190
		[SerializeField]
		protected BBParameter checkValue;

		// Token: 0x040000BF RID: 191
		[SerializeField]
		protected CompareMethod comparison;
	}
}
