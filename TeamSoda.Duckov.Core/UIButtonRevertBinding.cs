using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001E7 RID: 487
public class UIButtonRevertBinding : MonoBehaviour
{
	// Token: 0x06000EB2 RID: 3762 RVA: 0x0003BEB3 File Offset: 0x0003A0B3
	private void Awake()
	{
		if (this.button == null)
		{
			this.button = base.GetComponent<Button>();
		}
		this.button.onClick.AddListener(new UnityAction(this.OnBtnClick));
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0003BEEB File Offset: 0x0003A0EB
	public void OnBtnClick()
	{
		InputRebinder.Clear();
		InputRebinder.Save();
	}

	// Token: 0x04000C39 RID: 3129
	[SerializeField]
	private Button button;
}
