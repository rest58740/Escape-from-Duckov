using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000C0 RID: 192
	[Category("✫ Reflected")]
	[Description("Send a Unity message to all game objects with a component of the specified type.\nNotice: This is slow and should not be called per-fame.")]
	public class SendMessageToType<T> : ActionTask where T : Component
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000CD86 File Offset: 0x0000AF86
		protected override string info
		{
			get
			{
				return string.Format("Message {0}({1}) to all {2}s", this.message, this.argument, typeof(T).Name);
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000CDB0 File Offset: 0x0000AFB0
		protected override void OnExecute()
		{
			T[] array = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
			if (array.Length == 0)
			{
				base.EndAction(false);
				return;
			}
			T[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].gameObject.SendMessage(this.message.value, this.argument.value);
			}
			base.EndAction(true);
		}

		// Token: 0x04000249 RID: 585
		[RequiredField]
		public BBParameter<string> message;

		// Token: 0x0400024A RID: 586
		[BlackboardOnly]
		public BBParameter<object> argument;
	}
}
