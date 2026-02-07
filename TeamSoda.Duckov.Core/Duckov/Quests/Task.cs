using System;
using Saves;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000351 RID: 849
	[Serializable]
	public abstract class Task : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001D56 RID: 7510 RVA: 0x0006A907 File Offset: 0x00068B07
		// (set) Token: 0x06001D57 RID: 7511 RVA: 0x0006A90F File Offset: 0x00068B0F
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

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0006A918 File Offset: 0x00068B18
		// (set) Token: 0x06001D59 RID: 7513 RVA: 0x0006A920 File Offset: 0x00068B20
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

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001D5A RID: 7514 RVA: 0x0006A929 File Offset: 0x00068B29
		public virtual string Description
		{
			get
			{
				return "未定义Task描述。";
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x0006A930 File Offset: 0x00068B30
		public virtual string[] ExtraDescriptsions
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001D5C RID: 7516 RVA: 0x0006A938 File Offset: 0x00068B38
		public virtual Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06001D5D RID: 7517 RVA: 0x0006A93C File Offset: 0x00068B3C
		// (remove) Token: 0x06001D5E RID: 7518 RVA: 0x0006A974 File Offset: 0x00068B74
		public event Action<Task> onStatusChanged;

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x0006A9A9 File Offset: 0x00068BA9
		public virtual bool Interactable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001D60 RID: 7520 RVA: 0x0006A9AC File Offset: 0x00068BAC
		public virtual bool PossibleValidInteraction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x0006A9AF File Offset: 0x00068BAF
		public virtual bool NeedInspection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x0006A9B2 File Offset: 0x00068BB2
		public virtual string InteractText
		{
			get
			{
				return "交互";
			}
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0006A9B9 File Offset: 0x00068BB9
		public virtual void Interact()
		{
			Debug.LogWarning(string.Format("{0}可能未定义交互行为", base.GetType()));
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0006A9D0 File Offset: 0x00068BD0
		public bool IsFinished()
		{
			return this.forceFinish || this.CheckFinished();
		}

		// Token: 0x06001D65 RID: 7525
		protected abstract bool CheckFinished();

		// Token: 0x06001D66 RID: 7526
		public abstract object GenerateSaveData();

		// Token: 0x06001D67 RID: 7527
		public abstract void SetupSaveData(object data);

		// Token: 0x06001D68 RID: 7528 RVA: 0x0006A9E2 File Offset: 0x00068BE2
		protected void ReportStatusChanged()
		{
			Action<Task> action = this.onStatusChanged;
			if (action != null)
			{
				action(this);
			}
			if (this.IsFinished())
			{
				Quest quest = this.Master;
				if (quest == null)
				{
					return;
				}
				quest.NotifyTaskFinished(this);
			}
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0006AA0F File Offset: 0x00068C0F
		internal void Init()
		{
			if (this.IsFinished())
			{
				base.enabled = false;
				return;
			}
			this.OnInit();
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x0006AA27 File Offset: 0x00068C27
		protected virtual void OnInit()
		{
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x0006AA29 File Offset: 0x00068C29
		internal void ForceFinish()
		{
			this.forceFinish = true;
			Action<Task> action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x0400147E RID: 5246
		[SerializeField]
		private Quest master;

		// Token: 0x0400147F RID: 5247
		[SerializeField]
		private int id;

		// Token: 0x04001481 RID: 5249
		[SerializeField]
		private bool forceFinish;
	}
}
