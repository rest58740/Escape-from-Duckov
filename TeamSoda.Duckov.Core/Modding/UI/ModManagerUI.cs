using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Modding.UI
{
	// Token: 0x0200027D RID: 637
	public class ModManagerUI : MonoBehaviour
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0004BD5D File Offset: 0x00049F5D
		private ModManager Master
		{
			get
			{
				return ModManager.Instance;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0004BD64 File Offset: 0x00049F64
		private PrefabPool<ModEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<ModEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004BDA0 File Offset: 0x00049FA0
		private void Awake()
		{
			this.agreementBtn.onClick.AddListener(new UnityAction(this.OnAgreementBtnClicked));
			this.quitBtn.onClick.AddListener(new UnityAction(this.Quit));
			this.rejectBtn.onClick.AddListener(new UnityAction(this.OnRejectBtnClicked));
			this.needRebootIndicator.SetActive(false);
			ModManager.OnReorder += this.OnReorder;
			ModManager.OnModStatusChanged += this.OnModStatusChanged;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0004BE2F File Offset: 0x0004A02F
		private void OnDestroy()
		{
			ModManager.OnReorder -= this.OnReorder;
			ModManager.OnModStatusChanged -= this.OnModStatusChanged;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004BE53 File Offset: 0x0004A053
		private void OnModStatusChanged()
		{
			this.needRebootIndicator.SetActive(true);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004BE61 File Offset: 0x0004A061
		private void OnReorder()
		{
			this.Refresh();
			this.needRebootIndicator.SetActive(true);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0004BE75 File Offset: 0x0004A075
		private void OnRejectBtnClicked()
		{
			ModManager.AllowActivatingMod = false;
			this.Quit();
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004BE83 File Offset: 0x0004A083
		private void OnAgreementBtnClicked()
		{
			ModManager.AllowActivatingMod = true;
			this.agreementFadeGroup.Hide();
			this.contentFadeGroup.Show();
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0004BEA1 File Offset: 0x0004A0A1
		private void Show()
		{
			this.mainFadeGroup.Show();
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0004BEB0 File Offset: 0x0004A0B0
		private void OnEnable()
		{
			ModManager.Rescan();
			this.Refresh();
			this.uploaderFadeGroup.SkipHide();
			if (!ModManager.AllowActivatingMod)
			{
				this.contentFadeGroup.SkipHide();
				this.agreementFadeGroup.Show();
				return;
			}
			this.agreementFadeGroup.SkipHide();
			this.contentFadeGroup.Show();
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0004BF08 File Offset: 0x0004A108
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			int num = 0;
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				this.Pool.Get(null).Setup(this, modInfo, num);
				num++;
			}
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0004BF78 File Offset: 0x0004A178
		private void Hide()
		{
			this.mainFadeGroup.Hide();
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0004BF85 File Offset: 0x0004A185
		private void Quit()
		{
			UnityEvent unityEvent = this.onQuit;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.Hide();
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0004BFA0 File Offset: 0x0004A1A0
		internal UniTask BeginUpload(ModInfo info)
		{
			ModManagerUI.<BeginUpload>d__28 <BeginUpload>d__;
			<BeginUpload>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<BeginUpload>d__.<>4__this = this;
			<BeginUpload>d__.info = info;
			<BeginUpload>d__.<>1__state = -1;
			<BeginUpload>d__.<>t__builder.Start<ModManagerUI.<BeginUpload>d__28>(ref <BeginUpload>d__);
			return <BeginUpload>d__.<>t__builder.Task;
		}

		// Token: 0x04000F0B RID: 3851
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000F0C RID: 3852
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000F0D RID: 3853
		[SerializeField]
		private FadeGroup agreementFadeGroup;

		// Token: 0x04000F0E RID: 3854
		[SerializeField]
		private FadeGroup uploaderFadeGroup;

		// Token: 0x04000F0F RID: 3855
		[SerializeField]
		private ModUploadPanel uploadPanel;

		// Token: 0x04000F10 RID: 3856
		[SerializeField]
		private Button rejectBtn;

		// Token: 0x04000F11 RID: 3857
		[SerializeField]
		private Button agreementBtn;

		// Token: 0x04000F12 RID: 3858
		[SerializeField]
		private ModEntry entryTemplate;

		// Token: 0x04000F13 RID: 3859
		[SerializeField]
		private Button quitBtn;

		// Token: 0x04000F14 RID: 3860
		[SerializeField]
		private GameObject needRebootIndicator;

		// Token: 0x04000F15 RID: 3861
		public UnityEvent onQuit;

		// Token: 0x04000F16 RID: 3862
		private PrefabPool<ModEntry> _pool;

		// Token: 0x04000F17 RID: 3863
		private bool uploading;
	}
}
