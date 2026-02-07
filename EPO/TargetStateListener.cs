using System;
using System.Collections.Generic;
using UnityEngine;

namespace EPOOutline
{
	// Token: 0x02000024 RID: 36
	[ExecuteAlways]
	public class TargetStateListener : MonoBehaviour
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00006C57 File Offset: 0x00004E57
		public void AddCallback(Outlinable outlinable, Action action)
		{
			this.callbacks.Add(new TargetStateListener.Callback(outlinable, action));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006C6C File Offset: 0x00004E6C
		public void RemoveCallback(Outlinable outlinable, Action callback)
		{
			int num = this.callbacks.FindIndex((TargetStateListener.Callback x) => x.Target == outlinable && x.Action == callback);
			if (num == -1)
			{
				return;
			}
			this.callbacks.RemoveAt(num);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006CB6 File Offset: 0x00004EB6
		private void Awake()
		{
			base.hideFlags = HideFlags.HideInInspector;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006CC0 File Offset: 0x00004EC0
		public void ForceUpdate()
		{
			this.callbacks.RemoveAll((TargetStateListener.Callback x) => x.Target == null);
			foreach (TargetStateListener.Callback callback in this.callbacks)
			{
				callback.Action();
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006D40 File Offset: 0x00004F40
		private void OnBecameVisible()
		{
			this.ForceUpdate();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006D48 File Offset: 0x00004F48
		private void OnBecameInvisible()
		{
			this.ForceUpdate();
		}

		// Token: 0x040000CB RID: 203
		private List<TargetStateListener.Callback> callbacks = new List<TargetStateListener.Callback>();

		// Token: 0x02000036 RID: 54
		public struct Callback
		{
			// Token: 0x06000132 RID: 306 RVA: 0x000072EE File Offset: 0x000054EE
			public Callback(Outlinable target, Action action)
			{
				this.Target = target;
				this.Action = action;
			}

			// Token: 0x0400011B RID: 283
			public readonly Outlinable Target;

			// Token: 0x0400011C RID: 284
			public readonly Action Action;
		}
	}
}
