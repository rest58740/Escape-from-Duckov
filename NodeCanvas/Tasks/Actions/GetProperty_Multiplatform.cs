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
	// Token: 0x020000BC RID: 188
	[Name("Get Property", 8)]
	[Category("✫ Reflected")]
	[Description("Get a property of a script and save it to the blackboard")]
	public class GetProperty_Multiplatform : ActionTask, IReflectedWrapper
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000C8AE File Offset: 0x0000AAAE
		private MethodInfo targetMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000C8BB File Offset: 0x0000AABB
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
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
				return string.Format("{0} = {1}.{2}", this.returnValue.ToString(), text, this.targetMethod.Name);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000C972 File Offset: 0x0000AB72
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.method;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000C97A File Offset: 0x0000AB7A
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.method != null && this.method.HasChanged())
			{
				this.SetMethod(this.method);
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000C9A2 File Offset: 0x0000ABA2
		protected override string OnInit()
		{
			if (this.method == null)
			{
				return "No Property selected";
			}
			if (this.targetMethod == null)
			{
				return string.Format("Missing Property '{0}'", this.method.AsString());
			}
			return null;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000C9D7 File Offset: 0x0000ABD7
		protected override void OnExecute()
		{
			this.returnValue.value = this.targetMethod.Invoke(this.targetMethod.IsStatic ? null : base.agent, null);
			base.EndAction();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000CA0C File Offset: 0x0000AC0C
		private void SetMethod(MethodInfo method)
		{
			if (method != null)
			{
				this.method = new SerializedMethodInfo(method);
				this.returnValue.SetType(method.ReturnType);
			}
		}

		// Token: 0x04000240 RID: 576
		[SerializeField]
		protected SerializedMethodInfo method;

		// Token: 0x04000241 RID: 577
		[SerializeField]
		[BlackboardOnly]
		protected BBObjectParameter returnValue;
	}
}
