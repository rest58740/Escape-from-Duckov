using System;
using Saves;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200034F RID: 847
	public abstract class Reward : MonoBehaviour, ISelfValidator, ISaveDataProvider
	{
		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06001D2F RID: 7471 RVA: 0x0006A510 File Offset: 0x00068710
		// (remove) Token: 0x06001D30 RID: 7472 RVA: 0x0006A544 File Offset: 0x00068744
		public static event Action<Reward> OnRewardClaimed;

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x0006A577 File Offset: 0x00068777
		// (set) Token: 0x06001D32 RID: 7474 RVA: 0x0006A57F File Offset: 0x0006877F
		public int ID
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06001D33 RID: 7475 RVA: 0x0006A588 File Offset: 0x00068788
		// (remove) Token: 0x06001D34 RID: 7476 RVA: 0x0006A5C0 File Offset: 0x000687C0
		internal event Action onStatusChanged;

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x0006A5F5 File Offset: 0x000687F5
		public bool Claimable
		{
			get
			{
				return this.master.Complete;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x0006A602 File Offset: 0x00068802
		public virtual Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x0006A605 File Offset: 0x00068805
		public virtual string Description
		{
			get
			{
				return "未定义奖励描述";
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001D38 RID: 7480
		public abstract bool Claimed { get; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001D39 RID: 7481 RVA: 0x0006A60C File Offset: 0x0006880C
		public virtual bool Claiming { get; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0006A614 File Offset: 0x00068814
		public virtual bool AutoClaim
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001D3B RID: 7483 RVA: 0x0006A617 File Offset: 0x00068817
		// (set) Token: 0x06001D3C RID: 7484 RVA: 0x0006A61F File Offset: 0x0006881F
		public Quest Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0006A628 File Offset: 0x00068828
		public void Claim()
		{
			if (!this.Claimable || this.Claimed)
			{
				return;
			}
			this.OnClaim();
			this.Master.NotifyRewardClaimed(this);
			Action<Reward> onRewardClaimed = Reward.OnRewardClaimed;
			if (onRewardClaimed == null)
			{
				return;
			}
			onRewardClaimed(this);
		}

		// Token: 0x06001D3E RID: 7486
		public abstract void OnClaim();

		// Token: 0x06001D3F RID: 7487 RVA: 0x0006A660 File Offset: 0x00068860
		public virtual void Validate(SelfValidationResult result)
		{
			if (this.master == null)
			{
				SelfValidationResultItemExtensions.WithFix(result.AddWarning("Reward需要master(Quest)。"), "设为父物体中的Quest。", delegate()
				{
					this.master = base.GetComponent<Quest>();
					if (this.master == null)
					{
						this.master = base.GetComponentInParent<Quest>();
					}
				}, true);
			}
			if (this.master != null)
			{
				if (base.transform != this.master.transform && !base.transform.IsChildOf(this.master.transform))
				{
					SelfValidationResultItemExtensions.WithFix(result.AddError("Task需要存在于master子物体中。"), "设为master子物体", delegate()
					{
						base.transform.SetParent(this.master.transform);
					}, true);
				}
				if (!this.master.rewards.Contains(this))
				{
					SelfValidationResultItemExtensions.WithFix(result.AddError("Master的Task列表中不包含本物体。"), "将本物体添加至master的Task列表中", delegate()
					{
						this.master.rewards.Add(this);
					}, true);
				}
			}
		}

		// Token: 0x06001D40 RID: 7488
		public abstract object GenerateSaveData();

		// Token: 0x06001D41 RID: 7489
		public abstract void SetupSaveData(object data);

		// Token: 0x06001D42 RID: 7490 RVA: 0x0006A738 File Offset: 0x00068938
		private void Awake()
		{
			this.Master.onStatusChanged += this.OnMasterStatusChanged;
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x0006A751 File Offset: 0x00068951
		private void OnDestroy()
		{
			this.Master.onStatusChanged -= this.OnMasterStatusChanged;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0006A76A File Offset: 0x0006896A
		public void OnMasterStatusChanged(Quest quest)
		{
			Action action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x0006A77C File Offset: 0x0006897C
		protected void ReportStatusChanged()
		{
			Action action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x0006A78E File Offset: 0x0006898E
		public virtual void NotifyReload(Quest questInstance)
		{
		}

		// Token: 0x04001475 RID: 5237
		[SerializeField]
		private int id;

		// Token: 0x04001476 RID: 5238
		[SerializeField]
		private Quest master;
	}
}
