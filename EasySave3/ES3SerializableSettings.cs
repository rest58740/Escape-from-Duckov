using System;
using System.ComponentModel;

// Token: 0x02000011 RID: 17
[EditorBrowsable(EditorBrowsableState.Never)]
[Serializable]
public class ES3SerializableSettings : ES3Settings
{
	// Token: 0x0600014B RID: 331 RVA: 0x00005B65 File Offset: 0x00003D65
	public ES3SerializableSettings() : base(false)
	{
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00005B6E File Offset: 0x00003D6E
	public ES3SerializableSettings(bool applyDefaults) : base(applyDefaults)
	{
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00005B77 File Offset: 0x00003D77
	public ES3SerializableSettings(string path) : base(false)
	{
		this.path = path;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00005B87 File Offset: 0x00003D87
	public ES3SerializableSettings(string path, ES3.Location location) : base(false)
	{
		base.location = location;
	}
}
