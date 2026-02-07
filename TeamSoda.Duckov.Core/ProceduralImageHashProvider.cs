using System;
using LeTai.TrueShadow;
using LeTai.TrueShadow.PluginInterfaces;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000210 RID: 528
[ExecuteInEditMode]
public class ProceduralImageHashProvider : MonoBehaviour, ITrueShadowCustomHashProvider
{
	// Token: 0x06000FA5 RID: 4005 RVA: 0x0003E589 File Offset: 0x0003C789
	private void Awake()
	{
		if (this.trueShadow == null)
		{
			this.trueShadow = base.GetComponent<TrueShadow>();
		}
		if (this.proceduralImage == null)
		{
			this.proceduralImage = base.GetComponent<ProceduralImage>();
		}
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x0003E5BF File Offset: 0x0003C7BF
	private void Refresh()
	{
		this.trueShadow.CustomHash = this.Hash();
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0003E5D2 File Offset: 0x0003C7D2
	private void OnValidate()
	{
		if (this.trueShadow == null)
		{
			this.trueShadow = base.GetComponent<TrueShadow>();
		}
		if (this.proceduralImage == null)
		{
			this.proceduralImage = base.GetComponent<ProceduralImage>();
		}
		this.Refresh();
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0003E60E File Offset: 0x0003C80E
	private void OnRectTransformDimensionsChange()
	{
		this.Refresh();
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x0003E618 File Offset: 0x0003C818
	private int Hash()
	{
		return this.proceduralImage.InfoCache.GetHashCode() + this.proceduralImage.color.GetHashCode();
	}

	// Token: 0x04000CDB RID: 3291
	[SerializeField]
	private ProceduralImage proceduralImage;

	// Token: 0x04000CDC RID: 3292
	[SerializeField]
	private TrueShadow trueShadow;
}
