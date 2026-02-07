using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000099 RID: 153
public class StrJson
{
	// Token: 0x06000550 RID: 1360 RVA: 0x0001820C File Offset: 0x0001640C
	private StrJson(params string[] contentPairs)
	{
		this.entries = new List<StrJson.Entry>();
		for (int i = 0; i < contentPairs.Length - 1; i += 2)
		{
			this.entries.Add(new StrJson.Entry(contentPairs[i], contentPairs[i + 1]));
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00018252 File Offset: 0x00016452
	public StrJson Add(string key, string value)
	{
		this.entries.Add(new StrJson.Entry(key, value));
		return this;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00018267 File Offset: 0x00016467
	public static StrJson Create(params string[] contentPairs)
	{
		return new StrJson(contentPairs);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00018270 File Offset: 0x00016470
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("{");
		for (int i = 0; i < this.entries.Count; i++)
		{
			StrJson.Entry entry = this.entries[i];
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(string.Concat(new string[]
			{
				"\"",
				entry.key,
				"\":\"",
				entry.value,
				"\""
			}));
		}
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}

	// Token: 0x040004DF RID: 1247
	public List<StrJson.Entry> entries;

	// Token: 0x02000467 RID: 1127
	public struct Entry
	{
		// Token: 0x06002758 RID: 10072 RVA: 0x0008A1C2 File Offset: 0x000883C2
		public Entry(string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		// Token: 0x04001B8B RID: 7051
		public string key;

		// Token: 0x04001B8C RID: 7052
		public string value;
	}
}
