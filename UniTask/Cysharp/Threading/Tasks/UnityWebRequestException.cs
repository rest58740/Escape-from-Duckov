using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200006A RID: 106
	public class UnityWebRequestException : Exception
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000A6CC File Offset: 0x000088CC
		public UnityWebRequest UnityWebRequest { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000A6D4 File Offset: 0x000088D4
		public UnityWebRequest.Result Result { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000A6DC File Offset: 0x000088DC
		public string Error { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000A6E4 File Offset: 0x000088E4
		public string Text { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000A6EC File Offset: 0x000088EC
		public long ResponseCode { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000A6F4 File Offset: 0x000088F4
		public Dictionary<string, string> ResponseHeaders { get; }

		// Token: 0x060002DB RID: 731 RVA: 0x0000A6FC File Offset: 0x000088FC
		public UnityWebRequestException(UnityWebRequest unityWebRequest)
		{
			this.UnityWebRequest = unityWebRequest;
			this.Result = unityWebRequest.result;
			this.Error = unityWebRequest.error;
			this.ResponseCode = unityWebRequest.responseCode;
			if (this.UnityWebRequest.downloadHandler != null)
			{
				DownloadHandlerBuffer downloadHandlerBuffer = unityWebRequest.downloadHandler as DownloadHandlerBuffer;
				if (downloadHandlerBuffer != null)
				{
					this.Text = downloadHandlerBuffer.text;
				}
			}
			this.ResponseHeaders = unityWebRequest.GetResponseHeaders();
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000A770 File Offset: 0x00008970
		public override string Message
		{
			get
			{
				if (this.msg == null)
				{
					if (!string.IsNullOrWhiteSpace(this.Text))
					{
						this.msg = this.Error + Environment.NewLine + this.Text;
					}
					else
					{
						this.msg = this.Error;
					}
				}
				return this.msg;
			}
		}

		// Token: 0x040000EB RID: 235
		private string msg;
	}
}
