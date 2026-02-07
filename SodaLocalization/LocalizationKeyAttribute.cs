using System;

// Token: 0x02000002 RID: 2
[AttributeUsage(384)]
public class LocalizationKeyAttribute : Attribute
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public LocalizationKeyAttribute(string fallbackFile = "Default")
	{
		this.fallbackFile = fallbackFile;
	}

	// Token: 0x04000001 RID: 1
	public string fallbackFile;
}
