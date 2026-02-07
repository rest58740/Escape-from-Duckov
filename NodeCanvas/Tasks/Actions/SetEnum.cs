using System;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200007F RID: 127
	[Category("✫ Blackboard")]
	public class SetEnum : ActionTask
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000094C7 File Offset: 0x000076C7
		protected override string info
		{
			get
			{
				BBObjectParameter bbobjectParameter = this.valueA;
				string text = (bbobjectParameter != null) ? bbobjectParameter.ToString() : null;
				string text2 = " = ";
				BBObjectParameter bbobjectParameter2 = this.valueB;
				return text + text2 + ((bbobjectParameter2 != null) ? bbobjectParameter2.ToString() : null);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000094F7 File Offset: 0x000076F7
		protected override void OnExecute()
		{
			this.valueA.value = this.valueB.value;
			base.EndAction();
		}

		// Token: 0x04000176 RID: 374
		[BlackboardOnly]
		[RequiredField]
		public BBObjectParameter valueA = new BBObjectParameter(typeof(Enum));

		// Token: 0x04000177 RID: 375
		public BBObjectParameter valueB = new BBObjectParameter(typeof(Enum));
	}
}
