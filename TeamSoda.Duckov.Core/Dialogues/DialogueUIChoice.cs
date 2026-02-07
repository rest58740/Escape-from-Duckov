using System;
using System.Collections.Generic;
using DG.Tweening;
using NodeCanvas.DialogueTrees;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogues
{
	// Token: 0x02000225 RID: 549
	public class DialogueUIChoice : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00041946 File Offset: 0x0003FB46
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00041950 File Offset: 0x0003FB50
		private void Awake()
		{
			MenuItem menuItem = this.menuItem;
			menuItem.onSelected = (Action<MenuItem>)Delegate.Combine(menuItem.onSelected, new Action<MenuItem>(this.Refresh));
			MenuItem menuItem2 = this.menuItem;
			menuItem2.onDeselected = (Action<MenuItem>)Delegate.Combine(menuItem2.onDeselected, new Action<MenuItem>(this.Refresh));
			MenuItem menuItem3 = this.menuItem;
			menuItem3.onFocusStatusChanged = (Action<MenuItem, bool>)Delegate.Combine(menuItem3.onFocusStatusChanged, new Action<MenuItem, bool>(this.Refresh));
			MenuItem menuItem4 = this.menuItem;
			menuItem4.onConfirmed = (Action<MenuItem>)Delegate.Combine(menuItem4.onConfirmed, new Action<MenuItem>(this.OnConfirm));
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x000419F9 File Offset: 0x0003FBF9
		private void OnConfirm(MenuItem item)
		{
			this.Confirm();
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00041A04 File Offset: 0x0003FC04
		private void AnimateConfirm()
		{
			ShortcutExtensions.DOKill(this.confirmIndicator, false);
			TweenSettingsExtensions.OnKill<Sequence>(TweenSettingsExtensions.OnComplete<Sequence>(DOTweenModuleUI.DOGradientColor(this.confirmIndicator, this.confirmAnimationColor, this.confirmAnimationDuration), delegate()
			{
				this.confirmIndicator.color = Color.clear;
			}), delegate()
			{
				this.confirmIndicator.color = Color.clear;
			});
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00041A58 File Offset: 0x0003FC58
		private void Refresh(MenuItem item, bool focus)
		{
			this.selectionIndicator.SetActive(this.menuItem.IsSelected);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00041A70 File Offset: 0x0003FC70
		private void Refresh(MenuItem item)
		{
			this.selectionIndicator.SetActive(this.menuItem.IsSelected);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00041A88 File Offset: 0x0003FC88
		private void Confirm()
		{
			this.master.NotifyChoiceConfirmed(this);
			this.AnimateConfirm();
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00041A9C File Offset: 0x0003FC9C
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Confirm();
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00041AA4 File Offset: 0x0003FCA4
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.menuItem.Select();
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00041AB4 File Offset: 0x0003FCB4
		internal void Setup(DialogueUI master, KeyValuePair<IStatement, int> cur)
		{
			this.master = master;
			this.index = cur.Value;
			this.text.text = cur.Key.text;
			this.confirmIndicator.color = Color.clear;
			this.Refresh(this.menuItem);
		}

		// Token: 0x04000D6A RID: 3434
		[SerializeField]
		private MenuItem menuItem;

		// Token: 0x04000D6B RID: 3435
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04000D6C RID: 3436
		[SerializeField]
		private Image confirmIndicator;

		// Token: 0x04000D6D RID: 3437
		[SerializeField]
		private Gradient confirmAnimationColor;

		// Token: 0x04000D6E RID: 3438
		[SerializeField]
		private float confirmAnimationDuration = 0.2f;

		// Token: 0x04000D6F RID: 3439
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000D70 RID: 3440
		private DialogueUI master;

		// Token: 0x04000D71 RID: 3441
		private int index;
	}
}
