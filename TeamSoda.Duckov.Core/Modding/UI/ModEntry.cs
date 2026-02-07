using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Modding.UI
{
	// Token: 0x0200027C RID: 636
	public class ModEntry : MonoBehaviour
	{
		// Token: 0x06001426 RID: 5158 RVA: 0x0004B9FC File Offset: 0x00049BFC
		private void Awake()
		{
			this.toggleButton.onClick.AddListener(new UnityAction(this.OnToggleButtonClicked));
			this.uploadButton.onClick.AddListener(new UnityAction(this.OnUploadButtonClicked));
			ModManager.OnModLoadingFailed = (Action<string, string>)Delegate.Combine(ModManager.OnModLoadingFailed, new Action<string, string>(this.OnModLoadingFailed));
			this.failedIndicator.SetActive(false);
			this.btnReorderDown.onClick.AddListener(new UnityAction(this.OnButtonReorderDownClicked));
			this.btnReorderUp.onClick.AddListener(new UnityAction(this.OnButtonReorderUpClicked));
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0004BAA5 File Offset: 0x00049CA5
		private void OnButtonReorderUpClicked()
		{
			ModManager.Reorder(this.index, this.index - 1);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0004BABB File Offset: 0x00049CBB
		private void OnButtonReorderDownClicked()
		{
			ModManager.Reorder(this.index, this.index + 1);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0004BAD1 File Offset: 0x00049CD1
		private void OnDestroy()
		{
			ModManager.OnModLoadingFailed = (Action<string, string>)Delegate.Remove(ModManager.OnModLoadingFailed, new Action<string, string>(this.OnModLoadingFailed));
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0004BAF3 File Offset: 0x00049CF3
		private void OnModLoadingFailed(string dllPath, string message)
		{
			if (dllPath != this.info.dllPath)
			{
				return;
			}
			Debug.LogError(message);
			this.failedIndicator.SetActive(true);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0004BB1B File Offset: 0x00049D1B
		private void OnUploadButtonClicked()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.BeginUpload(this.info).Forget();
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0004BB44 File Offset: 0x00049D44
		private void OnToggleButtonClicked()
		{
			if (ModManager.Instance == null)
			{
				Debug.LogError("ModManager.Instance Not Found");
				return;
			}
			ModBehaviour modBehaviour;
			bool flag = ModManager.IsModActive(this.info, out modBehaviour);
			bool flag2 = flag && modBehaviour.info.path.Trim() == this.info.path.Trim();
			if (flag && flag2)
			{
				ModManager.Instance.DeactivateMod(this.info);
				return;
			}
			ModManager.Instance.ActivateMod(this.info);
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0004BBC8 File Offset: 0x00049DC8
		private void OnEnable()
		{
			ModManager.OnModStatusChanged += this.OnModStatusChanged;
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0004BBDB File Offset: 0x00049DDB
		private void OnDisable()
		{
			ModManager.OnModStatusChanged -= this.OnModStatusChanged;
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0004BBEE File Offset: 0x00049DEE
		private void OnModStatusChanged()
		{
			this.RefreshStatus();
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0004BBF8 File Offset: 0x00049DF8
		private void RefreshStatus()
		{
			ModBehaviour modBehaviour;
			bool flag = ModManager.IsModActive(this.info, out modBehaviour);
			bool flag2 = flag && modBehaviour.info.path.Trim() == this.info.path.Trim();
			bool active = flag && !flag2;
			this.activeIndicator.SetActive(flag2);
			this.nameCollisionIndicator.SetActive(active);
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0004BC60 File Offset: 0x00049E60
		private void RefreshInfo()
		{
			this.textTitle.text = this.info.displayName;
			this.textName.text = this.info.name;
			this.textDescription.text = this.info.description;
			this.preview.texture = this.info.preview;
			this.steamItemIndicator.SetActive(this.info.isSteamItem);
			this.notSteamItemIndicator.SetActive(!this.info.isSteamItem);
			bool flag = SteamWorkshopManager.IsOwner(this.info);
			this.steamItemOwnerIndicator.SetActive(flag);
			bool active = flag || !this.info.isSteamItem;
			this.uploadButton.gameObject.SetActive(active);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0004BD32 File Offset: 0x00049F32
		public void Setup(ModManagerUI master, ModInfo modInfo, int index)
		{
			this.master = master;
			this.info = modInfo;
			this.index = index;
			this.RefreshInfo();
			this.RefreshStatus();
		}

		// Token: 0x04000EFA RID: 3834
		[SerializeField]
		private TextMeshProUGUI textTitle;

		// Token: 0x04000EFB RID: 3835
		[SerializeField]
		private TextMeshProUGUI textName;

		// Token: 0x04000EFC RID: 3836
		[SerializeField]
		private TextMeshProUGUI textDescription;

		// Token: 0x04000EFD RID: 3837
		[SerializeField]
		private RawImage preview;

		// Token: 0x04000EFE RID: 3838
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x04000EFF RID: 3839
		[SerializeField]
		private GameObject nameCollisionIndicator;

		// Token: 0x04000F00 RID: 3840
		[SerializeField]
		private Button toggleButton;

		// Token: 0x04000F01 RID: 3841
		[SerializeField]
		private GameObject steamItemIndicator;

		// Token: 0x04000F02 RID: 3842
		[SerializeField]
		private GameObject steamItemOwnerIndicator;

		// Token: 0x04000F03 RID: 3843
		[SerializeField]
		private GameObject notSteamItemIndicator;

		// Token: 0x04000F04 RID: 3844
		[SerializeField]
		private Button uploadButton;

		// Token: 0x04000F05 RID: 3845
		[SerializeField]
		private GameObject failedIndicator;

		// Token: 0x04000F06 RID: 3846
		[SerializeField]
		private Button btnReorderUp;

		// Token: 0x04000F07 RID: 3847
		[SerializeField]
		private Button btnReorderDown;

		// Token: 0x04000F08 RID: 3848
		[SerializeField]
		private int index;

		// Token: 0x04000F09 RID: 3849
		private ModManagerUI master;

		// Token: 0x04000F0A RID: 3850
		private ModInfo info;
	}
}
