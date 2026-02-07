using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class CostTakerHUD_Entry : MonoBehaviour
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0001ED40 File Offset: 0x0001CF40
	// (set) Token: 0x060006D3 RID: 1747 RVA: 0x0001ED48 File Offset: 0x0001CF48
	public CostTaker Target { get; private set; }

	// Token: 0x060006D4 RID: 1748 RVA: 0x0001ED51 File Offset: 0x0001CF51
	private void Awake()
	{
		this.rectTransform = (base.transform as RectTransform);
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0001ED64 File Offset: 0x0001CF64
	private void LateUpdate()
	{
		this.UpdatePosition();
		this.UpdateFadeGroup();
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0001ED72 File Offset: 0x0001CF72
	internal void Setup(CostTaker cur)
	{
		this.Target = cur;
		this.nameText.text = cur.InteractName;
		this.costDisplay.Setup(cur.Cost, 1);
		this.UpdatePosition();
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0001EDA4 File Offset: 0x0001CFA4
	private void UpdatePosition()
	{
		this.directionPositive = this.rectTransform.MatchWorldPosition(this.Target.transform.TransformPoint(this.Target.interactMarkerOffset), Vector3.up * 0.5f);
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0001EDE4 File Offset: 0x0001CFE4
	private void UpdateFadeGroup()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		bool flag = false;
		if (this.directionPositive && !(this.Target == null) && !(main == null))
		{
			Vector3 vector = main.transform.position - this.Target.transform.position;
			if (Mathf.Abs(vector.y) <= 2.5f && vector.magnitude <= 10f)
			{
				flag = true;
			}
		}
		if (flag && !this.fadeGroup.IsShown)
		{
			this.fadeGroup.Show();
			return;
		}
		if (!flag && this.fadeGroup.IsShown)
		{
			this.fadeGroup.Hide();
		}
	}

	// Token: 0x040006A1 RID: 1697
	private RectTransform rectTransform;

	// Token: 0x040006A2 RID: 1698
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x040006A3 RID: 1699
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x040006A4 RID: 1700
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x040006A5 RID: 1701
	private const float HideDistance = 10f;

	// Token: 0x040006A6 RID: 1702
	private const float HideDistanceYLimit = 2.5f;

	// Token: 0x040006A7 RID: 1703
	private bool directionPositive;
}
