using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x02000400 RID: 1024
	public class ToggleAnimation : MonoBehaviour
	{
		// Token: 0x140000FD RID: 253
		// (add) Token: 0x06002527 RID: 9511 RVA: 0x00081FB0 File Offset: 0x000801B0
		// (remove) Token: 0x06002528 RID: 9512 RVA: 0x00081FE8 File Offset: 0x000801E8
		public event Action<ToggleAnimation, bool> onSetToggle;

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x0008201D File Offset: 0x0008021D
		// (set) Token: 0x0600252A RID: 9514 RVA: 0x00082025 File Offset: 0x00080225
		public bool Status
		{
			get
			{
				return this.status;
			}
			protected set
			{
				this.SetToggle(value);
			}
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x0008202E File Offset: 0x0008022E
		public void SetToggle(bool value)
		{
			this.status = value;
			if (!Application.isPlaying)
			{
				return;
			}
			this.OnSetToggle(this.Status);
			Action<ToggleAnimation, bool> action = this.onSetToggle;
			if (action == null)
			{
				return;
			}
			action(this, value);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x0008205D File Offset: 0x0008025D
		protected virtual void OnSetToggle(bool value)
		{
		}

		// Token: 0x04001940 RID: 6464
		[SerializeField]
		[HideInInspector]
		private bool status;
	}
}
