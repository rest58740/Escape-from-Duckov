using System;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000259 RID: 601
	[RequireComponent(typeof(Perk))]
	public abstract class PerkBehaviour : MonoBehaviour
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x00048A7A File Offset: 0x00046C7A
		protected Perk Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x00048A82 File Offset: 0x00046C82
		private bool Unlocked
		{
			get
			{
				return !(this.master == null) && this.master.Unlocked;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x00048A9F File Offset: 0x00046C9F
		public virtual string Description
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00048AA8 File Offset: 0x00046CA8
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<Perk>();
			}
			this.master.onUnlockStateChanged += this.OnMasterUnlockStateChanged;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.OnLevelInitialized();
			}
			this.OnAwake();
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00048B0A File Offset: 0x00046D0A
		private void OnLevelInitialized()
		{
			this.NotifyUnlockStateChanged(this.Unlocked);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00048B18 File Offset: 0x00046D18
		private void OnDestroy()
		{
			this.OnOnDestroy();
			if (this.master == null)
			{
				return;
			}
			this.master.onUnlockStateChanged -= this.OnMasterUnlockStateChanged;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00048B57 File Offset: 0x00046D57
		private void OnValidate()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<Perk>();
			}
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x00048B73 File Offset: 0x00046D73
		private void OnMasterUnlockStateChanged(Perk perk, bool unlocked)
		{
			if (perk != this.master)
			{
				Debug.LogError("Perk对象不匹配");
			}
			this.NotifyUnlockStateChanged(unlocked);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00048B94 File Offset: 0x00046D94
		private void NotifyUnlockStateChanged(bool unlocked)
		{
			this.OnUnlockStateChanged(unlocked);
			if (unlocked)
			{
				this.OnUnlocked();
				return;
			}
			this.OnLocked();
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00048BAD File Offset: 0x00046DAD
		protected virtual void OnUnlockStateChanged(bool unlocked)
		{
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00048BAF File Offset: 0x00046DAF
		protected virtual void OnUnlocked()
		{
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00048BB1 File Offset: 0x00046DB1
		protected virtual void OnLocked()
		{
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x00048BB3 File Offset: 0x00046DB3
		protected virtual void OnAwake()
		{
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00048BB5 File Offset: 0x00046DB5
		protected virtual void OnOnDestroy()
		{
		}

		// Token: 0x04000E8E RID: 3726
		private Perk master;
	}
}
