using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Buffs;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000391 RID: 913
	public class BuffsDisplayEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06001FB8 RID: 8120 RVA: 0x0006FFB4 File Offset: 0x0006E1B4
		// (remove) Token: 0x06001FB9 RID: 8121 RVA: 0x0006FFE8 File Offset: 0x0006E1E8
		public static event Action<BuffsDisplayEntry, PointerEventData> OnBuffsDisplayEntryClicked;

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x0007001B File Offset: 0x0006E21B
		public Image Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x00070024 File Offset: 0x0006E224
		public void Setup(BuffsDisplay master, Buff target)
		{
			this.master = master;
			this.target = target;
			this.icon.sprite = target.Icon;
			if (this.displayName)
			{
				this.displayName.text = target.DisplayName;
			}
			this.fadeGroup.Show();
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00070079 File Offset: 0x0006E279
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00070084 File Offset: 0x0006E284
		private void Refresh()
		{
			if (this.target == null)
			{
				this.Release();
				return;
			}
			if (this.target.LimitedLifeTime)
			{
				this.remainingTimeText.text = string.Format(this.timeFormat, this.target.RemainingTime);
			}
			else
			{
				this.remainingTimeText.text = "";
			}
			if (this.target.MaxLayers > 1)
			{
				this.layersText.text = this.target.CurrentLayers.ToString();
				return;
			}
			this.layersText.text = "";
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x00070128 File Offset: 0x0006E328
		public void Release()
		{
			if (this.releasing)
			{
				return;
			}
			this.releasing = true;
			this.ReleaseTask().Forget();
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x00070148 File Offset: 0x0006E348
		private UniTask ReleaseTask()
		{
			BuffsDisplayEntry.<ReleaseTask>d__19 <ReleaseTask>d__;
			<ReleaseTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ReleaseTask>d__.<>4__this = this;
			<ReleaseTask>d__.<>1__state = -1;
			<ReleaseTask>d__.<>t__builder.Start<BuffsDisplayEntry.<ReleaseTask>d__19>(ref <ReleaseTask>d__);
			return <ReleaseTask>d__.<>t__builder.Task;
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001FC0 RID: 8128 RVA: 0x0007018B File Offset: 0x0006E38B
		public Buff Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x00070193 File Offset: 0x0006E393
		public void NotifyPooled()
		{
			this.pooled = true;
			this.releasing = false;
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x000701A3 File Offset: 0x0006E3A3
		public void NotifyReleased()
		{
			this.pooled = false;
			this.target = null;
			this.releasing = false;
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000701BA File Offset: 0x0006E3BA
		public void OnPointerClick(PointerEventData eventData)
		{
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			Action<BuffsDisplayEntry, PointerEventData> onBuffsDisplayEntryClicked = BuffsDisplayEntry.OnBuffsDisplayEntryClicked;
			if (onBuffsDisplayEntryClicked == null)
			{
				return;
			}
			onBuffsDisplayEntryClicked(this, eventData);
		}

		// Token: 0x040015AE RID: 5550
		[SerializeField]
		private Image icon;

		// Token: 0x040015AF RID: 5551
		[SerializeField]
		private TextMeshProUGUI remainingTimeText;

		// Token: 0x040015B0 RID: 5552
		[SerializeField]
		private TextMeshProUGUI layersText;

		// Token: 0x040015B1 RID: 5553
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040015B2 RID: 5554
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015B3 RID: 5555
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x040015B4 RID: 5556
		[SerializeField]
		private string timeFormat = "{0:0}s";

		// Token: 0x040015B5 RID: 5557
		private BuffsDisplay master;

		// Token: 0x040015B6 RID: 5558
		private Buff target;

		// Token: 0x040015B7 RID: 5559
		private bool releasing;

		// Token: 0x040015B8 RID: 5560
		private bool pooled;
	}
}
