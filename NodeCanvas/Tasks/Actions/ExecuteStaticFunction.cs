using System;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D8 RID: 216
	[Obsolete("Execute Function now supports static functions as well")]
	public class ExecuteStaticFunction : ActionTask, ISubParametersContainer
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x0000E615 File Offset: 0x0000C815
		BBParameter[] ISubParametersContainer.GetSubParameters()
		{
			if (this.functionWrapper == null)
			{
				return null;
			}
			return this.functionWrapper.GetVariables();
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000E62C File Offset: 0x0000C82C
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000E644 File Offset: 0x0000C844
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
				string text2 = "";
				if (this.targetMethod.ReturnType == typeof(void))
				{
					for (int i = 0; i < variables.Length; i++)
					{
						text2 = text2 + ((i != 0) ? ", " : "") + variables[i].ToString();
					}
				}
				else
				{
					string text3;
					if (!variables[0].isNone)
					{
						BBParameter bbparameter = variables[0];
						text3 = ((bbparameter != null) ? bbparameter.ToString() : null) + " = ";
					}
					else
					{
						text3 = "";
					}
					text = text3;
					for (int j = 1; j < variables.Length; j++)
					{
						text2 = text2 + ((j != 1) ? ", " : "") + variables[j].ToString();
					}
				}
				return string.Format("{0}{1}.{2} ({3})", new object[]
				{
					text,
					this.targetMethod.DeclaringType.FriendlyName(false),
					this.targetMethod.Name,
					text2
				});
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000E778 File Offset: 0x0000C978
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.functionWrapper != null && this.functionWrapper.HasChanged())
			{
				this.SetMethod(this.functionWrapper.GetMethod());
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		protected override string OnInit()
		{
			if (this.targetMethod == null)
			{
				return "Missing Method";
			}
			string result;
			try
			{
				this.functionWrapper.Init(null);
				result = null;
			}
			catch
			{
				result = "ExecuteFunction Error";
			}
			return result;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000E7EC File Offset: 0x0000C9EC
		protected override void OnExecute()
		{
			if (this.targetMethod == null)
			{
				base.EndAction(false);
				return;
			}
			if (this.functionWrapper is ReflectedActionWrapper)
			{
				(this.functionWrapper as ReflectedActionWrapper).Call();
			}
			else
			{
				(this.functionWrapper as ReflectedFunctionWrapper).Call();
			}
			base.EndAction();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000E845 File Offset: 0x0000CA45
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.functionWrapper = ReflectedWrapper.Create(method, base.blackboard);
			}
		}

		// Token: 0x04000284 RID: 644
		[SerializeField]
		protected ReflectedWrapper functionWrapper;
	}
}
