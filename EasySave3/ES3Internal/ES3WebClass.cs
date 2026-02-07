using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ES3Internal
{
	// Token: 0x020000E4 RID: 228
	public class ES3WebClass
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0001E31C File Offset: 0x0001C51C
		public float uploadProgress
		{
			get
			{
				if (this._webRequest == null)
				{
					return 0f;
				}
				return this._webRequest.uploadProgress;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0001E337 File Offset: 0x0001C537
		public float downloadProgress
		{
			get
			{
				if (this._webRequest == null)
				{
					return 0f;
				}
				return this._webRequest.downloadProgress;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001E352 File Offset: 0x0001C552
		public bool isError
		{
			get
			{
				return !string.IsNullOrEmpty(this.error) || this.errorCode > 0L;
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001E36D File Offset: 0x0001C56D
		public static bool IsNetworkError(UnityWebRequest www)
		{
			return www.result == UnityWebRequest.Result.ConnectionError;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001E378 File Offset: 0x0001C578
		protected ES3WebClass(string url, string apiKey)
		{
			this.url = url;
			this.apiKey = apiKey;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001E399 File Offset: 0x0001C599
		public void AddPOSTField(string fieldName, string value)
		{
			this.formData.Add(new KeyValuePair<string, string>(fieldName, value));
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001E3AD File Offset: 0x0001C5AD
		protected string GetUser(string user, string password)
		{
			if (string.IsNullOrEmpty(user))
			{
				return "";
			}
			if (!string.IsNullOrEmpty(password))
			{
				user += password;
			}
			user = ES3Hash.SHA1Hash(user);
			return user;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
		protected WWWForm CreateWWWForm()
		{
			WWWForm wwwform = new WWWForm();
			foreach (KeyValuePair<string, string> keyValuePair in this.formData)
			{
				wwwform.AddField(keyValuePair.Key, keyValuePair.Value);
			}
			return wwwform;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001E440 File Offset: 0x0001C640
		protected bool HandleError(UnityWebRequest webRequest, bool errorIfDataIsDownloaded)
		{
			if (ES3WebClass.IsNetworkError(webRequest))
			{
				this.errorCode = 1L;
				this.error = "Error: " + webRequest.error;
			}
			else if (webRequest.responseCode >= 400L)
			{
				this.errorCode = webRequest.responseCode;
				if (string.IsNullOrEmpty(webRequest.downloadHandler.text))
				{
					this.error = string.Format("Server returned {0} error with no message", webRequest.responseCode);
				}
				else
				{
					this.error = webRequest.downloadHandler.text;
				}
			}
			else
			{
				if (!errorIfDataIsDownloaded || webRequest.downloadedBytes <= 0UL)
				{
					return false;
				}
				this.errorCode = 2L;
				this.error = "Server error: '" + webRequest.downloadHandler.text + "'";
			}
			return true;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001E50E File Offset: 0x0001C70E
		protected IEnumerator SendWebRequest(UnityWebRequest webRequest)
		{
			this._webRequest = webRequest;
			yield return webRequest.SendWebRequest();
			yield break;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001E524 File Offset: 0x0001C724
		protected virtual void Reset()
		{
			this.error = null;
			this.errorCode = 0L;
			this.isDone = false;
		}

		// Token: 0x04000152 RID: 338
		protected string url;

		// Token: 0x04000153 RID: 339
		protected string apiKey;

		// Token: 0x04000154 RID: 340
		protected List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();

		// Token: 0x04000155 RID: 341
		protected UnityWebRequest _webRequest;

		// Token: 0x04000156 RID: 342
		public bool isDone;

		// Token: 0x04000157 RID: 343
		public string error;

		// Token: 0x04000158 RID: 344
		public long errorCode;
	}
}
