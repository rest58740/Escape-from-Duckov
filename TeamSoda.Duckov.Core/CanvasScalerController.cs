using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015A RID: 346
public class CanvasScalerController : MonoBehaviour
{
	// Token: 0x06000AD3 RID: 2771 RVA: 0x0002FE31 File Offset: 0x0002E031
	private void OnValidate()
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0002FE4D File Offset: 0x0002E04D
	private void Awake()
	{
		this.OnValidate();
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0002FE55 File Offset: 0x0002E055
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0002FE60 File Offset: 0x0002E060
	private void Refresh()
	{
		if (this.canvasScaler == null)
		{
			return;
		}
		Vector2Int currentResolution = this.GetCurrentResolution();
		float num = (float)currentResolution.x / (float)currentResolution.y;
		Vector2 referenceResolution = this.canvasScaler.referenceResolution;
		float num2 = referenceResolution.x / referenceResolution.y;
		if (num > num2)
		{
			this.canvasScaler.matchWidthOrHeight = 1f;
		}
		else
		{
			this.canvasScaler.matchWidthOrHeight = 0f;
		}
		this.cachedResolution = currentResolution;
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0002FEDB File Offset: 0x0002E0DB
	private void FixedUpdate()
	{
		if (!this.ResolutionMatch())
		{
			this.Refresh();
		}
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0002FEEC File Offset: 0x0002E0EC
	private bool ResolutionMatch()
	{
		Vector2Int currentResolution = this.GetCurrentResolution();
		return this.cachedResolution.x == currentResolution.x && this.cachedResolution.y == currentResolution.y;
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0002FF2A File Offset: 0x0002E12A
	private Vector2Int GetCurrentResolution()
	{
		return new Vector2Int(Display.main.renderingWidth, Display.main.renderingHeight);
	}

	// Token: 0x04000989 RID: 2441
	[SerializeField]
	private CanvasScaler canvasScaler;

	// Token: 0x0400098A RID: 2442
	private Vector2Int cachedResolution;
}
