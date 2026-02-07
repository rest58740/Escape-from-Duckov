using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003DD RID: 989
	public abstract class View : ManagedUIElement
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x0007EB31 File Offset: 0x0007CD31
		// (set) Token: 0x0600240C RID: 9228 RVA: 0x0007EB38 File Offset: 0x0007CD38
		public static View ActiveView
		{
			get
			{
				return View._activeView;
			}
			private set
			{
				Object activeView = View._activeView;
				View._activeView = value;
				if (activeView != View._activeView)
				{
					Action onActiveViewChanged = View.OnActiveViewChanged;
					if (onActiveViewChanged == null)
					{
						return;
					}
					onActiveViewChanged();
				}
			}
		}

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x0600240D RID: 9229 RVA: 0x0007EB60 File Offset: 0x0007CD60
		// (remove) Token: 0x0600240E RID: 9230 RVA: 0x0007EB94 File Offset: 0x0007CD94
		public static event Action OnActiveViewChanged;

		// Token: 0x0600240F RID: 9231 RVA: 0x0007EBC8 File Offset: 0x0007CDC8
		protected override void Awake()
		{
			base.Awake();
			if (this.exitButton != null)
			{
				this.exitButton.onClick.AddListener(new UnityAction(base.Close));
			}
			UIInputManager.OnNavigate += this.OnNavigate;
			UIInputManager.OnConfirm += this.OnConfirm;
			UIInputManager.OnCancel += this.OnCancel;
			this.viewTabs = base.transform.parent.parent.GetComponent<ViewTabs>();
			if (this.autoClose)
			{
				base.Close();
			}
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x0007EC61 File Offset: 0x0007CE61
		protected override void OnDestroy()
		{
			base.OnDestroy();
			UIInputManager.OnNavigate -= this.OnNavigate;
			UIInputManager.OnConfirm -= this.OnConfirm;
			UIInputManager.OnCancel -= this.OnCancel;
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x0007EC9C File Offset: 0x0007CE9C
		protected override void OnOpen()
		{
			this.autoClose = false;
			if (View.ActiveView != null && View.ActiveView != this)
			{
				View.ActiveView.Close();
			}
			View.ActiveView = this;
			ItemUIUtilities.Select(null);
			if (this.viewTabs != null)
			{
				this.viewTabs.Show();
			}
			if (base.gameObject == null)
			{
				Debug.LogError("GameObject不存在", base.gameObject);
			}
			InputManager.DisableInput(base.gameObject);
			AudioManager.Post(this.sfx_Open);
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x0007ED2E File Offset: 0x0007CF2E
		protected override void OnClose()
		{
			if (View.ActiveView == this)
			{
				View.ActiveView = null;
			}
			InputManager.ActiveInput(base.gameObject);
			AudioManager.Post(this.sfx_Close);
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0007ED5A File Offset: 0x0007CF5A
		internal virtual void TryQuit()
		{
			base.Close();
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x0007ED62 File Offset: 0x0007CF62
		public void OnNavigate(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView != this)
			{
				return;
			}
			this.OnNavigate(eventData.vector);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x0007ED87 File Offset: 0x0007CF87
		public void OnConfirm(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView != this)
			{
				return;
			}
			this.OnConfirm();
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0007EDA6 File Offset: 0x0007CFA6
		public void OnCancel(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView == null || View.ActiveView != this)
			{
				return;
			}
			this.OnCancel();
			if (!eventData.Used)
			{
				this.TryQuit();
				eventData.Use();
			}
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x0007EDE6 File Offset: 0x0007CFE6
		protected virtual void OnNavigate(Vector2 vector)
		{
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x0007EDE8 File Offset: 0x0007CFE8
		protected virtual void OnConfirm()
		{
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0007EDEA File Offset: 0x0007CFEA
		protected virtual void OnCancel()
		{
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x0007EDEC File Offset: 0x0007CFEC
		protected static T GetViewInstance<T>() where T : View
		{
			return GameplayUIManager.GetViewInstance<T>();
		}

		// Token: 0x04001885 RID: 6277
		[HideInInspector]
		private static View _activeView;

		// Token: 0x04001887 RID: 6279
		[SerializeField]
		private ViewTabs viewTabs;

		// Token: 0x04001888 RID: 6280
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001889 RID: 6281
		[SerializeField]
		private string sfx_Open;

		// Token: 0x0400188A RID: 6282
		[SerializeField]
		private string sfx_Close;

		// Token: 0x0400188B RID: 6283
		private bool autoClose = true;
	}
}
