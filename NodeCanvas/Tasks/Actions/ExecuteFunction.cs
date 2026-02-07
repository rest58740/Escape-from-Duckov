using System;
using System.Collections;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000C4 RID: 196
	[Name("Execute Function (Desktop Only)", 10)]
	[Category("✫ Reflected/Faster Versions (Desktop Platforms Only)")]
	[Description("This version works in destop/JIT platform only.\n\nExecute a function on a script, of up to 6 parameters and save the return if any. If function is an IEnumerator it will execute as a coroutine.")]
	public class ExecuteFunction : ActionTask, IReflectedWrapper
	{
		// Token: 0x06000349 RID: 841 RVA: 0x0000D140 File Offset: 0x0000B340
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			ReflectedWrapper reflectedWrapper = this.functionWrapper;
			if (reflectedWrapper == null)
			{
				return null;
			}
			return reflectedWrapper.GetSerializedMethod();
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000D153 File Offset: 0x0000B353
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000D16A File Offset: 0x0000B36A
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000D1A0 File Offset: 0x0000B3A0
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
					if (!(this.targetMethod.ReturnType == typeof(void)) && !(this.targetMethod.ReturnType == typeof(IEnumerator)) && !variables[0].isNone)
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
				string text4 = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				return string.Format("{0}{1}.{2}({3})", new object[]
				{
					text,
					text4,
					this.targetMethod.Name,
					text2
				});
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000D32C File Offset: 0x0000B52C
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.functionWrapper != null && this.functionWrapper.HasChanged())
			{
				this.SetMethod(this.functionWrapper.GetMethod());
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000D354 File Offset: 0x0000B554
		protected override string OnInit()
		{
			if (this.functionWrapper == null)
			{
				return "No Method selected";
			}
			if (this.targetMethod == null)
			{
				return string.Format("Missing Method '{0}'", this.functionWrapper.AsString());
			}
			string result;
			try
			{
				this.functionWrapper.Init(this.targetMethod.IsStatic ? null : base.agent);
				result = null;
			}
			catch
			{
				result = "ExecuteFunction Error";
			}
			return result;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000D3D4 File Offset: 0x0000B5D4
		protected override void OnExecute()
		{
			if (this.targetMethod == null)
			{
				base.EndAction(false);
				return;
			}
			if (this.targetMethod.ReturnType == typeof(IEnumerator))
			{
				base.StartCoroutine(this.InternalCoroutine((IEnumerator)((ReflectedFunctionWrapper)this.functionWrapper).Call()));
				return;
			}
			if (this.targetMethod.ReturnType == typeof(void))
			{
				((ReflectedActionWrapper)this.functionWrapper).Call();
			}
			else
			{
				((ReflectedFunctionWrapper)this.functionWrapper).Call();
			}
			base.EndAction(true);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000D47C File Offset: 0x0000B67C
		protected override void OnStop()
		{
			this.routineRunning = false;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000D485 File Offset: 0x0000B685
		private IEnumerator InternalCoroutine(IEnumerator routine)
		{
			this.routineRunning = true;
			while (this.routineRunning && routine.MoveNext())
			{
				if (!this.routineRunning)
				{
					yield break;
				}
				yield return routine.Current;
			}
			if (this.routineRunning)
			{
				base.EndAction();
			}
			yield break;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000D49B File Offset: 0x0000B69B
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.functionWrapper = ReflectedWrapper.Create(method, base.blackboard);
			}
		}

		// Token: 0x04000251 RID: 593
		[SerializeField]
		protected ReflectedWrapper functionWrapper;

		// Token: 0x04000252 RID: 594
		private bool routineRunning;
	}
}
