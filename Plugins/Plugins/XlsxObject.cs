using System;
using System.Text;
using FlexFramework.Excel;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class XlsxObject : ScriptableObject
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000036 RID: 54 RVA: 0x0000283C File Offset: 0x00000A3C
	public string Info
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = string.Format("Binary File {0} bytes", this.bytes.Length);
			stringBuilder.AppendLine(text);
			return stringBuilder.ToString();
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000037 RID: 55 RVA: 0x00002873 File Offset: 0x00000A73
	public string preview
	{
		get
		{
			return this.Preview();
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000287B File Offset: 0x00000A7B
	public void Load(byte[] bytes)
	{
		this.bytes = bytes;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002884 File Offset: 0x00000A84
	public override int GetHashCode()
	{
		return XlsxObject.ComputeHash(this.bytes);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002894 File Offset: 0x00000A94
	private static int ComputeHash(params byte[] data)
	{
		int num = -2128831035;
		for (int i = 0; i < data.Length; i++)
		{
			num = (num ^ (int)data[i]) * 16777619;
		}
		return num;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000028C2 File Offset: 0x00000AC2
	public WorkBook Read()
	{
		return new WorkBook(this.bytes);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000028CF File Offset: 0x00000ACF
	[ContextMenu("Refresh Preview")]
	private void RefreshPreview()
	{
		this._previewWorkBook = this.Read();
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000028E0 File Offset: 0x00000AE0
	private string Preview()
	{
		if (this._previewWorkBook == null)
		{
			this._previewWorkBook = this.Read();
		}
		if (this._previewWorkBook == null)
		{
			return "ERROR: Cannot Read Workbook";
		}
		if (this._previewWorkBook.Count <= 0)
		{
			return "*Nothing to display";
		}
		StringBuilder stringBuilder = new StringBuilder();
		WorkSheet workSheet = this._previewWorkBook[0];
		if (workSheet == null)
		{
			return "ERROR: no content in workbook";
		}
		stringBuilder.Append(string.Format("Line Count: {0}\n", workSheet.Count));
		stringBuilder.Append("\n");
		stringBuilder.Append("Content:\n");
		int num = Mathf.Min(workSheet.Count, 10);
		for (int i = 0; i < num; i++)
		{
			foreach (Cell cell in workSheet[i])
			{
				if (cell != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(cell.Text);
					stringBuilder.Append("\t\t|");
				}
			}
			stringBuilder.Append("\n_\n");
		}
		if (num < workSheet.Count - 1)
		{
			stringBuilder.AppendLine("...");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04000013 RID: 19
	[HideInInspector]
	[SerializeField]
	public byte[] bytes;

	// Token: 0x04000014 RID: 20
	private WorkBook _previewWorkBook;
}
