using System;

// Token: 0x0200006E RID: 110
[Serializable]
public struct ElementFactor
{
	// Token: 0x06000438 RID: 1080 RVA: 0x00012C48 File Offset: 0x00010E48
	public ElementFactor(ElementTypes _type, float _factor)
	{
		this.elementType = _type;
		this.factor = _factor;
	}

	// Token: 0x04000341 RID: 833
	public ElementTypes elementType;

	// Token: 0x04000342 RID: 834
	public float factor;
}
