using System;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class MultiInteractionMenu : MonoBehaviour
{
	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0003D8FE File Offset: 0x0003BAFE
	// (set) Token: 0x06000F4F RID: 3919 RVA: 0x0003D905 File Offset: 0x0003BB05
	public static MultiInteractionMenu Instance { get; private set; }

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0003D910 File Offset: 0x0003BB10
	private PrefabPool<MultiInteractionMenuButton> ButtonPool
	{
		get
		{
			if (this._buttonPool == null)
			{
				this._buttonPool = new PrefabPool<MultiInteractionMenuButton>(this.buttonTemplate, this.buttonTemplate.transform.parent, null, null, null, true, 10, 10000, null);
				this.buttonTemplate.gameObject.SetActive(false);
			}
			return this._buttonPool;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0003D969 File Offset: 0x0003BB69
	public MultiInteraction Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x0003D971 File Offset: 0x0003BB71
	private void Awake()
	{
		if (MultiInteractionMenu.Instance == null)
		{
			MultiInteractionMenu.Instance = this;
		}
		this.buttonTemplate.gameObject.SetActive(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x0003D9A4 File Offset: 0x0003BBA4
	private void Setup(MultiInteraction target)
	{
		this.target = target;
		ReadOnlyCollection<InteractableBase> interactables = target.Interactables;
		this.ButtonPool.ReleaseAll();
		foreach (InteractableBase interactableBase in interactables)
		{
			if (!(interactableBase == null))
			{
				MultiInteractionMenuButton multiInteractionMenuButton = this.ButtonPool.Get(null);
				multiInteractionMenuButton.Setup(interactableBase);
				multiInteractionMenuButton.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x0003DA24 File Offset: 0x0003BC24
	private int CreateNewToken()
	{
		this.currentTaskToken = Random.Range(int.MinValue, int.MaxValue);
		return this.currentTaskToken;
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x0003DA41 File Offset: 0x0003BC41
	private bool TokenChanged(int token)
	{
		return token != this.currentTaskToken;
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x0003DA50 File Offset: 0x0003BC50
	public UniTask SetupAndShow(MultiInteraction target)
	{
		MultiInteractionMenu.<SetupAndShow>d__17 <SetupAndShow>d__;
		<SetupAndShow>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<SetupAndShow>d__.<>4__this = this;
		<SetupAndShow>d__.target = target;
		<SetupAndShow>d__.<>1__state = -1;
		<SetupAndShow>d__.<>t__builder.Start<MultiInteractionMenu.<SetupAndShow>d__17>(ref <SetupAndShow>d__);
		return <SetupAndShow>d__.<>t__builder.Task;
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x0003DA9C File Offset: 0x0003BC9C
	public UniTask Hide()
	{
		MultiInteractionMenu.<Hide>d__18 <Hide>d__;
		<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Hide>d__.<>4__this = this;
		<Hide>d__.<>1__state = -1;
		<Hide>d__.<>t__builder.Start<MultiInteractionMenu.<Hide>d__18>(ref <Hide>d__);
		return <Hide>d__.<>t__builder.Task;
	}

	// Token: 0x04000CB5 RID: 3253
	[SerializeField]
	private MultiInteractionMenuButton buttonTemplate;

	// Token: 0x04000CB6 RID: 3254
	[SerializeField]
	private float delayEachButton = 0.25f;

	// Token: 0x04000CB7 RID: 3255
	private PrefabPool<MultiInteractionMenuButton> _buttonPool;

	// Token: 0x04000CB8 RID: 3256
	private MultiInteraction target;

	// Token: 0x04000CB9 RID: 3257
	private int currentTaskToken;
}
