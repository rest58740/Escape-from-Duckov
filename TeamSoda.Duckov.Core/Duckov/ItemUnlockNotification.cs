using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov
{
	// Token: 0x0200024B RID: 587
	public class ItemUnlockNotification : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00047720 File Offset: 0x00045920
		public string MainTextFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.mainTextFormatKey);
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x0004772D File Offset: 0x0004592D
		private string SubTextFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.subTextFormatKey);
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x0004773A File Offset: 0x0004593A
		// (set) Token: 0x0600127E RID: 4734 RVA: 0x00047741 File Offset: 0x00045941
		public static ItemUnlockNotification Instance { get; private set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00047749 File Offset: 0x00045949
		private bool showing
		{
			get
			{
				return this.showingTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x00047759 File Offset: 0x00045959
		public static bool Showing
		{
			get
			{
				return !(ItemUnlockNotification.Instance == null) && ItemUnlockNotification.Instance.showing;
			}
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00047774 File Offset: 0x00045974
		private void Awake()
		{
			if (ItemUnlockNotification.Instance == null)
			{
				ItemUnlockNotification.Instance = this;
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00047789 File Offset: 0x00045989
		private void Update()
		{
			if (!this.showing && ItemUnlockNotification.pending.Count > 0)
			{
				this.BeginShow();
			}
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x000477A6 File Offset: 0x000459A6
		private void BeginShow()
		{
			this.showingTask = this.ShowTask();
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x000477B4 File Offset: 0x000459B4
		private UniTask ShowTask()
		{
			ItemUnlockNotification.<ShowTask>d__26 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<ItemUnlockNotification.<ShowTask>d__26>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x000477F8 File Offset: 0x000459F8
		private UniTask DisplayContent(int itemTypeID)
		{
			ItemUnlockNotification.<DisplayContent>d__27 <DisplayContent>d__;
			<DisplayContent>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayContent>d__.<>4__this = this;
			<DisplayContent>d__.itemTypeID = itemTypeID;
			<DisplayContent>d__.<>1__state = -1;
			<DisplayContent>d__.<>t__builder.Start<ItemUnlockNotification.<DisplayContent>d__27>(ref <DisplayContent>d__);
			return <DisplayContent>d__.<>t__builder.Task;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00047844 File Offset: 0x00045A44
		private void Setup(int itemTypeID)
		{
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
			string displayName = metaData.DisplayName;
			Sprite icon = metaData.icon;
			this.image.sprite = icon;
			this.textMain.text = StringExtensions.Format(this.MainTextFormat, new
			{
				itemDisplayName = displayName
			});
			this.textSub.text = this.SubTextFormat;
			DisplayQuality displayQuality = metaData.displayQuality;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(displayQuality).Apply(this.shadow);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x000478BD File Offset: 0x00045ABD
		public void OnPointerClick(PointerEventData eventData)
		{
			this.pointerClicked = true;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x000478C6 File Offset: 0x00045AC6
		public static void Push(int itemTypeID)
		{
			ItemUnlockNotification.pending.Add(itemTypeID);
		}

		// Token: 0x04000E32 RID: 3634
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000E33 RID: 3635
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000E34 RID: 3636
		[SerializeField]
		private Image image;

		// Token: 0x04000E35 RID: 3637
		[SerializeField]
		private TrueShadow shadow;

		// Token: 0x04000E36 RID: 3638
		[SerializeField]
		private TextMeshProUGUI textMain;

		// Token: 0x04000E37 RID: 3639
		[SerializeField]
		private TextMeshProUGUI textSub;

		// Token: 0x04000E38 RID: 3640
		[SerializeField]
		private float contentDelay = 0.5f;

		// Token: 0x04000E39 RID: 3641
		[SerializeField]
		[LocalizationKey("Default")]
		private string mainTextFormatKey = "UI_ItemUnlockNotification";

		// Token: 0x04000E3A RID: 3642
		[SerializeField]
		[LocalizationKey("Default")]
		private string subTextFormatKey = "UI_ItemUnlockNotification_Sub";

		// Token: 0x04000E3B RID: 3643
		private static List<int> pending = new List<int>();

		// Token: 0x04000E3D RID: 3645
		private UniTask showingTask;

		// Token: 0x04000E3E RID: 3646
		private bool pointerClicked;
	}
}
